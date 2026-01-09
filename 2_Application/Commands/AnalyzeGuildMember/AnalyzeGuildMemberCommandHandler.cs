using MediatR;
using MlkAdmin.Shared.Dtos;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._2_Application.Commands.AnalyzeGuildMember;

public class AnalyzeGuildMemberCommandHandler : IRequestHandler<AnalyzeGuildMemberCommand, BaseResult<GuildMemberAnalyzeData>>
{
    public async Task<BaseResult<GuildMemberAnalyzeData>> Handle(AnalyzeGuildMemberCommand request, CancellationToken cancellationToken)
    {
        return default;
    }
}
