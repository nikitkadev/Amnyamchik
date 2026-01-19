using Discord;
using Discord.WebSocket;

namespace Amnyam._3_Infrastructure.Interfaces;

public interface IDiscordService
{
    DiscordSocketClient DiscordClient { get; }
    Task<SocketGuildUser> GetGuildMemberAsync(ulong guildMemberDiscordId);
    Task<string> GetGuildMemberMentionByIdAsync(ulong memberId);
    SocketGuild GetSocketGuild();
    GuildEmote? GetGuildEmote(string emoteKey);
}
