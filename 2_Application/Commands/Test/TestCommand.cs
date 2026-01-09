using MediatR;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._2_Application.Commands.Test;

public class TestCommand : IRequest<BaseResult>
{
    public string MemberPrompt { get; set; } = string.Empty;
}
