using MlkAdmin._1_Domain.Entities;
using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._2_Application.Interfaces.Services;

public interface IAnalysisService
{
    Task<GuildMemberAIAnalysisResultDto?> GetGuildMemberAIAnalysisAsync(ulong guildMemberDiscordId);
}
