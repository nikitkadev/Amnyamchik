using Amnyam.Shared.Dtos;

namespace Amnyam._4_Presentation.Interfaces;

public interface IChatGPTService
{
    Task<GuildMemberAIAnalysisResultDto> AnalyzeWithAIGuildMemberAsync(IReadOnlyCollection<string?> guildMemberMessages);
}
