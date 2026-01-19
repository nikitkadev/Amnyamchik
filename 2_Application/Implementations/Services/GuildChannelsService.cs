using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Exceptions;
using Amnyam._1_Domain.Interfaces;
using Amnyam._2_Application.Interfaces.Services;
using Amnyam._3_Infrastructure.Interfaces;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._2_Application.Implementations.Services;

public class GuildChannelsService(
    ILogger<GuildChannelsService> logger,
    IJsonProvidersHub providersHub,
    IDiscordService discordService,
    IGuildMembersRepository membersRepository) : IGuildChannelsService
{
    public async Task<SocketGuildChannel> GetGuildChannelByDiscordIdAsync(ulong guildChannelDiscordId)
    {
        if (await discordService.DiscordClient.GetChannelAsync(guildChannelDiscordId) is not SocketGuildChannel channel)
        {
            logger.LogWarning
                ("Канал с DiscordId {GuildChannelDiscordId} не является каналом сервера",
                guildChannelDiscordId);

            throw new GuildChannelNotFoundException(guildChannelDiscordId);
        }

        return channel;
    }
    public async Task<RestVoiceChannel> CreateVoiceChannelAsync(ulong guildMemberDiscordId)
    {
        var guild = discordService.GetSocketGuild();

        var leader = await discordService.GetGuildMemberAsync(guildMemberDiscordId) 
            ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        var voiceChannel = await guild.CreateVoiceChannelAsync(
            name: $"🔉 | {await membersRepository.GetVoiceRoomNameAsync(guildMemberDiscordId)}",
            func: properties =>
            {
                properties.CategoryId = providersHub.GuildConfigProvidersHub.Categories.Lobby.DiscordId;
                properties.Bitrate = 64000;
                properties.PermissionOverwrites = new Overwrite[]
                {
                    new(
                        providersHub.GuildConfigProvidersHub.Roles.GetGuildRoleByKey("PlayersClub").Id,
                        PermissionTarget.Role,
                        new OverwritePermissions(
                            connect: PermValue.Allow,
                            sendMessages: PermValue.Allow,
                            manageChannel: PermValue.Deny)
                    ),
                    new(
                        leader.Id,
                        PermissionTarget.User,
                        new OverwritePermissions(manageChannel: PermValue.Allow)
                    )
                };
            }
        );

        return voiceChannel;
    }
}