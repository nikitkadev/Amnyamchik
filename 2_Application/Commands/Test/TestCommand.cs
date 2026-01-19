using MediatR;
using Amnyam.Shared.Results;

namespace Amnyam._2_Application.Commands.Test;

public class TestCommand : IRequest<BaseResult>
{
    public string MemberPrompt { get; set; } = string.Empty;
}
