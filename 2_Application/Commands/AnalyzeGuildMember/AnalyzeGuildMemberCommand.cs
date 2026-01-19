using MediatR;
using Amnyam.Shared.Results;
using Amnyam.Shared.Dtos;

namespace Amnyam._2_Application.Commands.AnalyzeGuildMember;

public class AnalyzeGuildMemberCommand : IRequest<BaseResult<GuildMemberAnalysisResultData>>
{
    public ulong GuildMemberDiscordId { get; set; }
}
