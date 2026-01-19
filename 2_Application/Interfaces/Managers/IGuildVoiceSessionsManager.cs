using Discord.WebSocket;

namespace Amnyam._2_Application.Interfaces.Managers;

public interface IGuildVoiceSessionsManager
{
    Task HandleVoiceStateUpdateAsync(SocketVoiceChannel? newVoiceChannel, SocketVoiceChannel? oldVoiceChannel, ulong guildMemberDiscordId);
}
