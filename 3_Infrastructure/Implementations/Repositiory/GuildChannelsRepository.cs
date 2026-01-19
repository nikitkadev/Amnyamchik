using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Interfaces;
using Amnyam._3_Infrastructure.DataBase.EF;
using Amnyam._3_Infrastructure.Interfaces;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._2_Application.Managers.Channels.VoiceChannels;

public class GuildChannelsRepository(
    ILogger<GuildChannelsRepository> logger, 
    IJsonProvidersHub providersHub,
    IDiscordService discordService,
    MlkAdminDbContext mlkAdminDbContext) : IGuildChannelsRepository
{
    private async Task UpsertEntityAsync<TEntity>(
        TEntity entity, 
        Expression<Func<TEntity, bool>> predicate,
        Action<TEntity, TEntity>? updateProperties = null) where TEntity : class
    {
        try
        {
            var dbSet = mlkAdminDbContext.Set<TEntity>();
            var existingEntity = await dbSet.FirstOrDefaultAsync(predicate);

            if (existingEntity is null)
                await dbSet.AddAsync(entity);
            else
            {
                if (updateProperties != null)
                    updateProperties(existingEntity, entity);
                else
                    mlkAdminDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            await mlkAdminDbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "");
            throw;
        }
    }

    private async Task RemoveEntityAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        try
        {
            var dbSet = mlkAdminDbContext.Set<TEntity>();
            var existingEntity = await dbSet.FirstOrDefaultAsync(predicate);

            if (existingEntity is null) return;

            dbSet.Remove(existingEntity);

            await mlkAdminDbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Filed to remove TEntity");

            throw;
        }
    }

    private async Task<bool> CheckVoiceChannelPropertyAsync(
        ulong channelId,
        Func<GuildVoiceChannel, bool> propertySelector,
        CancellationToken token = default) 
    {
        try
        {
            var vChannel = await mlkAdminDbContext.VChannels.FirstOrDefaultAsync(x => x.DiscordId == channelId, token);
            if (vChannel is null) return false;

            return propertySelector(vChannel);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "");

            throw;
        }
    }

    public async Task SyncDbVoiceChannelsWithGuildAsync(CancellationToken token = default)
    {
        var guild = discordService.GetSocketGuild();

        if (guild is null) return;
        if (guild.VoiceChannels is null || guild.VoiceChannels.Count == 0) return;

        await using var transaction = await mlkAdminDbContext.Database.BeginTransactionAsync(token);

        try
        {
            var dbVoiceChannelsIds = (await mlkAdminDbContext.VChannels
                .Select(x => x.DiscordId)
                .ToListAsync(token))
                .ToHashSet();

            var clientVoiceChannelsIds = guild.VoiceChannels.Select(x => x.Id).ToHashSet();
            var guildVoiceChannels = guild.VoiceChannels;


            var toUpdate = guildVoiceChannels.Where(x => dbVoiceChannelsIds.Contains(x.Id));
            if (toUpdate.Any())
            {
                foreach (var vChannel in toUpdate)
                {
                    var dbChannel = await mlkAdminDbContext.VChannels.FirstOrDefaultAsync(x => x.DiscordId == vChannel.Id, token);

                    if (dbChannel is not null)
                    {
                        dbChannel.Name = vChannel.Name;
                        dbChannel.Category = vChannel.Category.Name;
                        dbChannel.IsGen = vChannel.Id == providersHub.GuildConfigProvidersHub.Channels.VoiceChannels.GeneratingVoice.DiscordId;
                        dbChannel.IsTemp = vChannel.Id != 
                            providersHub.GuildConfigProvidersHub.Channels.VoiceChannels.GeneratingVoice.DiscordId 
                            && vChannel.Category.Id == providersHub.GuildConfigProvidersHub.Categories.Lobby.DiscordId;
                    }
                }
            }

            var toRemove = dbVoiceChannelsIds.Where(x => !clientVoiceChannelsIds.Contains(x));
            if (toRemove.Any())
            {
                await mlkAdminDbContext.VChannels
                .Where(x => toRemove.Contains(x.DiscordId))
                .ExecuteDeleteAsync(token);
            }

            var toAdd = guildVoiceChannels.Where(x => !dbVoiceChannelsIds.Contains(x.Id));
            if (toAdd.Any())
            {
                foreach (var vChannel in toAdd)
                {
                    mlkAdminDbContext.VChannels.Add(
                        new GuildVoiceChannel()
                        {
                            DiscordId = vChannel.Id,
                            Category = vChannel.Category?.Name ?? string.Empty,
                            Name = vChannel.Name,
                            IsGen = vChannel.Id == providersHub.GuildConfigProvidersHub.Channels.VoiceChannels.GeneratingVoice.DiscordId,
                            IsTemp = vChannel.Id !=
                                providersHub.GuildConfigProvidersHub.Channels.VoiceChannels.GeneratingVoice.DiscordId
                                && vChannel.Category.Id == providersHub.GuildConfigProvidersHub.Categories.Lobby.DiscordId
                        }
                    );
                }
            }

            await mlkAdminDbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch (OperationCanceledException operationException)
        {
            await transaction.RollbackAsync(token);

            logger.LogError(
                operationException,
                "");
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(token);

            logger.LogError(
                exception,
                "Неудача при попытке синхронизировать каналы гильдии {Guild} ({GuildName}). Откат транзакции.", 
                guild.Id,
                guild.Name ?? "-");

            throw;
        }
    }
    public async Task UpsertDbTextChannelAsync(GuildTextChannel channel)
    {
        try
        {
            await UpsertEntityAsync(
                channel,
                x => x.DiscordId == channel.DiscordId,
                (existing, newChannel) =>
                {
                    existing.Name = channel.Name;
                    existing.Category = channel.Category;
                });
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception, 
                "Filed to upsert TChannel entity");

            throw;
        }
    }
    public async Task UpsertDbVoiceChannelAsync(GuildVoiceChannel channel)
    {
        try
        {
            await UpsertEntityAsync(
                channel,
                x => x.DiscordId == channel.DiscordId,
                (existing, newChannel) =>
                {
                    existing.Name = channel.Name;
                    existing.Category = channel.Category;
                    existing.IsTemp = channel.IsTemp;
                    existing.IsGen = channel.IsGen;
                });
        }
        catch (Exception exception)
        {
            logger.LogError(
            exception,
            "Filed to upsert VChannel entity");

            throw;
        }
    }
    public async Task RemoveDbTextChannelAsync(ulong id)
    {
        try
        {
            await RemoveEntityAsync<GuildTextChannel>(
                x => x.DiscordId == id);

        }
        catch (Exception exception)
        {
            logger.LogError(
                exception, 
                "");

            throw;
        }
    }
    public async Task RemoveDbVoiceChannelAsync(ulong id)
    {
        try
        {
            await RemoveEntityAsync<GuildVoiceChannel>(
                x => x.DiscordId == id);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception, 
                "");

            throw;
        }
    }
    public async Task<bool> IsTemporaryVoiceChannel(ulong id, CancellationToken token = default)
    {
        try
        {
            return await CheckVoiceChannelPropertyAsync(
                id,
                channel => channel.IsTemp,
                token);
                
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception, 
                "Неудача при попытке получить тип голосового канала {Id}",
                id);

            return false;
        }

    }
    public async Task<bool> IsGeneratingVoiceChannel(ulong id, CancellationToken token = default)
    {
        try
        {
            return await CheckVoiceChannelPropertyAsync(
                id,
                channel => channel.IsGen,
                token);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception, 
                "Неудача при попытке получить тип голосового канала {Id}", 
                id);

            return false;
        }
    }
}