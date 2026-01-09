using MediatR;
using MlkAdmin.Shared.Results;
using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._2_Application.Commands.AnalyzeGuildMember;

public class AnalyzeGuildMemberCommand : IRequest<BaseResult<GuildMemberAnalyzeData>>
{
    public ulong GuildMemberDiscordId { get; set; }
}
