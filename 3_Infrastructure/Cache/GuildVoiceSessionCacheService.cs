using Amnyam._2_Application.Interfaces.Services;
using System.Collections.Concurrent;

namespace Amnyam._3_Infrastructure.Cache
{
    public class GuildVoiceSessionCacheService : IGuildVoiceSessionCacheService
    {
        private readonly ConcurrentDictionary<ulong, DateTimeOffset> _sessions = [];

        public bool AlreadySet(ulong guildMemberDiscordId)
        {
            return _sessions.TryGetValue(guildMemberDiscordId, out _);
        }

        public DateTimeOffset? GetVoiceSessionStartByMemberDiscordId(ulong guildMemberDiscordId)
        {
            return _sessions.TryGetValue(guildMemberDiscordId, out DateTimeOffset start) 
                ? start 
                : null;
        }

        public void RemoveVoiceSessionFromDictionary(ulong guildMemberDiscordId)
        {
            _sessions.TryRemove(guildMemberDiscordId, out _);
        }

        public void SetVoiceSessionStart(ulong guildMemberDiscordId)
        {
            _sessions[guildMemberDiscordId] = DateTimeOffset.UtcNow;
        }
    }
}
