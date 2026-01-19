using MediatR;

namespace Amnyam._2_Application.Events.UserUpdated;

public class GuildMemberUpdatedHandler : INotificationHandler<GuildMemberUpdated>
{
    public async Task Handle(GuildMemberUpdated notification, CancellationToken cancellationToken)
    {
		
    }
}
