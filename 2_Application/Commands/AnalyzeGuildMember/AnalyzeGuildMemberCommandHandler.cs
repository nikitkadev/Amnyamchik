using MediatR;
using Amnyam._2_Application.Interfaces.Managers;
using Amnyam.Shared.Dtos;
using Amnyam.Shared.Results;

namespace Amnyam._2_Application.Commands.AnalyzeGuildMember;

public class AnalyzeGuildMemberCommandHandler(
    IGuildMembersManager membersManager) : IRequestHandler<AnalyzeGuildMemberCommand, BaseResult<GuildMemberAnalysisResultData>>
{
    public async Task<BaseResult<GuildMemberAnalysisResultData>> Handle(AnalyzeGuildMemberCommand request, CancellationToken cancellationToken)
    {
        return await membersManager.AnalyzeGuildMemberAsync(request.GuildMemberDiscordId);
    }
}
