using Amnyam._1_Domain.Entities;
using Amnyam.Shared.Dtos;

namespace Amnyam._2_Application.Interfaces.Services;

public interface IAnalysisService
{
    Task<GuildMemberAIAnalysisResultDto?> GetGuildMemberAIAnalysisAsync(ulong guildMemberDiscordId);
}
