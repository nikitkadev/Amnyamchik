using Discord.Rest;
using Discord.WebSocket;

namespace Amnyam._2_Application.Interfaces.Services;

public interface IGuildChannelsService
{
    Task<SocketGuildChannel> GetGuildChannelByDiscordIdAsync(ulong guildChannelDiscordId);
    Task<RestVoiceChannel> CreateVoiceChannelAsync(ulong guildMemberDiscordId);
}
