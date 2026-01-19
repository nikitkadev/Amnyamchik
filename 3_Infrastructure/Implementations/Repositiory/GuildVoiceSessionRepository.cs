using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Interfaces;
using Amnyam._3_Infrastructure.DataBase.EF;

namespace Amnyam._3_Infrastructure.Implementations.Services;

public class GuildVoiceSessionRepository(
    MlkAdminDbContext dbContext) : IGuildVoiceSessionRepository
{
    public async Task AddGuildVoiceSessionAsync(GuildVoiceSession session)
    {
        await dbContext.GuildVoiceSessions.AddAsync(session);
        await dbContext.SaveChangesAsync(); 
    }
}
