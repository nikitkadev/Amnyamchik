using Amnyam._1_Domain.Entities;

namespace Amnyam._1_Domain.Interfaces;

public interface IGuildVoiceSessionRepository
{
    public Task AddGuildVoiceSessionAsync(GuildVoiceSession session);
}
