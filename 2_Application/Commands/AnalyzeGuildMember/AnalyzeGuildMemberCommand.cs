using MediatR;
using MlkAdmin.Shared.Results;
using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._2_Application.Commands.AnalyzeGuildMember;

public class AnalyzeGuildMemberCommand : IRequest<BaseResult<GuildMemberAnalysisResultData>>
{
    public ulong GuildMemberDiscordId { get; set; }
}
