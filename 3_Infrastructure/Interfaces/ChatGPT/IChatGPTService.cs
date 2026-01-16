using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._4_Presentation.Interfaces;

public interface IChatGPTService
{
    Task<GuildMemberAIAnalysisResultDto> AnalyzeWithAIGuildMemberAsync(IReadOnlyCollection<string> guildMemberMessages);
}
