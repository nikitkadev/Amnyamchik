using Discord;
using Discord.WebSocket;
using Amnyam._1_Domain.Exceptions;
using Amnyam._3_Infrastructure.Interfaces;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._3_Infrastructure.Services;

public class DiscordService(
	IJsonProvidersHub providersHub,
	DiscordSocketClient discordSocketClient) : IDiscordService
{
	public DiscordSocketClient DiscordClient => discordSocketClient;

    public async Task<SocketGuildUser> GetGuildMemberAsync(ulong guildMemberDiscordId)
    {
		var socketGuildMember = discordSocketClient
            .GetGuild(providersHub.GuildConfigProvidersHub.GuildConfig.GuildDetails.DiscordId)
            .GetUser(guildMemberDiscordId) ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

		return socketGuildMember;
    }

    public async Task<string> GetGuildMemberMentionByIdAsync(ulong guildMemberDiscordId)
    {
        return discordSocketClient.GetGuild(
            providersHub.GuildConfigProvidersHub.GuildConfig.GuildDetails.DiscordId).Users
                .FirstOrDefault(
                    x => x.Id == guildMemberDiscordId).Mention;
    }

    public SocketGuild GetSocketGuild()
    {
        return discordSocketClient
            .GetGuild(
                providersHub.GuildConfigProvidersHub.GuildConfig.GuildDetails.DiscordId);
    }

    public GuildEmote? GetGuildEmote(string emoteName)
    {
        return GetSocketGuild().Emotes
            .FirstOrDefault(
                emote => emote.Name == emoteName);
    }

}
