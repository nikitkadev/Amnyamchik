using Discord.WebSocket;

namespace MlkAdmin._2_Application.Interfaces.Managers;

public interface IGuildVoiceSessionsManager
{
    Task HandleVoiceStateUpdateAsync(SocketVoiceChannel? newVoiceChannel, SocketVoiceChannel? oldVoiceChannel, ulong guildMemberDiscordId);
}
