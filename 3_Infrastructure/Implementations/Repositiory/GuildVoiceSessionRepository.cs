using MlkAdmin._1_Domain.Entities;
using MlkAdmin._1_Domain.Interfaces;
using MlkAdmin._3_Infrastructure.DataBase.EF;

namespace MlkAdmin._3_Infrastructure.Implementations.Services;

public class GuildVoiceSessionRepository(
    MlkAdminDbContext dbContext) : IGuildVoiceSessionRepository
{
    public async Task AddGuildVoiceSessionAsync(GuildVoiceSession session)
    {
        await dbContext.GuildVoiceSessions.AddAsync(session);
        await dbContext.SaveChangesAsync(); 
    }
}
