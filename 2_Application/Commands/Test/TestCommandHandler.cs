using MediatR;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._2_Application.Commands.Test;

public class TestCommandHandler : IRequestHandler<TestCommand, BaseResult>
{
    public async Task<BaseResult> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return default;
    }
}