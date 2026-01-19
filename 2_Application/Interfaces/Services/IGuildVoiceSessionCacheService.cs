namespace Amnyam._2_Application.Interfaces.Services;

public interface IGuildVoiceSessionCacheService
{
    void SetVoiceSessionStart(ulong guildMemberDiscordId);
    DateTimeOffset? GetVoiceSessionStartByMemberDiscordId(ulong guildMemberDiscordId);
    void RemoveVoiceSessionFromDictionary(ulong guildMemberDiscordId);
    bool AlreadySet(ulong guildMemberDiscordId);
}
