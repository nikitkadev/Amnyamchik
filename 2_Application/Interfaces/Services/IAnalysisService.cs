using MlkAdmin._1_Domain.Entities;
using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._2_Application.Interfaces.Services;

public interface IAnalysisService
{
    Task<GuildMemberMetric> GetGuildMemberMetricsAsync(ulong guildMemberDiscordId);
    Task<GuildMemberAIAnalysisResultDto> GetGuildMemberAIAnalysisAsync(ulong guildMemberDiscordId);
}
