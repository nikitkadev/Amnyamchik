using MediatR;

namespace MlkAdmin._2_Application.Events.ReactionAdded;

public class ReactionAddedHandler : INotificationHandler<ReactionAdded>
{
    public async Task Handle(ReactionAdded notification, CancellationToken cancellationToken)
    {
        
    }
}