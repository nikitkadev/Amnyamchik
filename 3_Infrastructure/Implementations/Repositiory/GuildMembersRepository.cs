using Microsoft.EntityFrameworkCore;
using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Exceptions;
using Amnyam._1_Domain.Interfaces;
using Amnyam._3_Infrastructure.DataBase.EF;
using Amnyam._3_Infrastructure.Interfaces;
using Amnyam.Shared.Constants;

namespace Amnyam._2_Application.Managers.Users;

public class GuildMembersRepository(
    IDiscordService discordService,
    MlkAdminDbContext mlkAdminDbContext) : IGuildMembersRepository
{
    public async Task UpsertGuildMemberAsync(GuildMember guildMember, CancellationToken token)
    {
        var dbMember = await mlkAdminDbContext.GuildMembers
            .FirstOrDefaultAsync(
                member => member.DiscordId == guildMember.DiscordId, token);

        if (dbMember is null)
        {
            await mlkAdminDbContext.GuildMembers.AddAsync(guildMember, token);
        }
        else
        {
            if (!string.IsNullOrEmpty(guildMember.DisplayName))
            {
                dbMember.DisplayName = guildMember.DisplayName;
            }
            
            if(!dbMember.IsAuthorized && guildMember.IsAuthorized)
            {
                dbMember.IsAuthorized = true;
            }
        }

        await mlkAdminDbContext.SaveChangesAsync(token);
    }

    public async Task<GuildMember> GetGuildMemberEntityAsync(ulong guildMemberDiscordId, CancellationToken token = default)
    {
        var dbMember = await mlkAdminDbContext.GuildMembers
            .FirstOrDefaultAsync(
                guildMember => guildMember.DiscordId == guildMemberDiscordId, token) ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        return dbMember;
    }

    public async Task RemoveGuildMemberEntityFromDbAsync(ulong guildMemberDiscordId, CancellationToken token = default)
    {
        var dbMember = await mlkAdminDbContext.GuildMembers
            .FirstOrDefaultAsync(
                guildMember => guildMember.DiscordId == guildMemberDiscordId, token) ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        mlkAdminDbContext.Remove(dbMember);

        await mlkAdminDbContext.SaveChangesAsync(token);
    }

    public async Task SyncGuildMembersWithDbAsync(CancellationToken token = default)
    {
        var guild = discordService.GetSocketGuild();

        await using var transaction = await mlkAdminDbContext.Database.BeginTransactionAsync(token);

        var dbMembersIds = (await mlkAdminDbContext.GuildMembers
            .Select(x => x.DiscordId)
            .ToListAsync(token))
            .ToHashSet();

        var clientMembersIds = guild.Users
            .Select(x => x.Id).ToHashSet();

        var clientMembers = guild.Users;

        var toUpdate = clientMembers
            .Where(x => dbMembersIds.Contains(x.Id));

        foreach (var guildMember in toUpdate)
        {
            var dbGuildMember = await mlkAdminDbContext.GuildMembers
                .FirstOrDefaultAsync(
                    x => x.DiscordId == guildMember.Id, token);

            dbGuildMember?.DisplayName = guildMember.DisplayName;
        }

        var toRemove = dbMembersIds
            .Where(x => !clientMembersIds.Contains(x));

        await mlkAdminDbContext.GuildMembers
            .Where(x => toRemove.Contains(x.DiscordId))
            .ExecuteDeleteAsync(token);

        var toAdd = clientMembers
            .Where(x => !dbMembersIds.Contains(x.Id));

        foreach (var guildMember in toAdd)
        {
            await mlkAdminDbContext.GuildMembers.AddAsync(
                new GuildMember()
                {
                    DiscordId = guildMember.Id,
                    DisplayName = guildMember.DisplayName,
                    JoinedAt = guildMember.JoinedAt ?? DateTimeOffset.Now,
                },
                token
            );
        }

        await mlkAdminDbContext.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
    }

    public async Task<bool> IsAuthorizedAsync(ulong guildMemberDiscordId, CancellationToken token = default)
    {
        var dbMember = await mlkAdminDbContext.GuildMembers
            .FirstOrDefaultAsync(
                guildMember => guildMember.DiscordId == guildMemberDiscordId, token) ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        return dbMember.IsAuthorized;
    }

    public async Task<long> GetTotalSecondsInVoiceChannelsByMemberDiscordIdAsync(ulong guildMemberDiscordId)
    {
        return await mlkAdminDbContext.GuildVoiceSessions.Where(session => session.GuildMemberDiscordId == guildMemberDiscordId).SumAsync(session => session.TotalSeconds);
    }
}