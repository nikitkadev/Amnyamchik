using Discord.Rest;
using Discord.WebSocket;

namespace MlkAdmin._2_Application.Interfaces.Services;

public interface IGuildChannelsService
{
    Task<SocketGuildChannel> GetGuildChannelByDiscordIdAsync(ulong guildChannelDiscordId);
    Task<RestVoiceChannel> CreateVoiceChannelAsync(ulong guildMemberDiscordId);
}
