using MediatR;
using Microsoft.Extensions.Logging;
using MlkAdmin._1_Domain.Entities;
using MlkAdmin._2_Application.Interfaces.Managers;

namespace MlkAdmin._2_Application.Events.UserJoined;

class UserJoinedHandler(
    ILogger<UserJoinedHandler> logger,
    IGuildMembersManager membersManager) : INotificationHandler<UserJoined>
{
    public async Task Handle(UserJoined notification, CancellationToken cancellationToken)
    {
        try
        {
            if (notification.SocketGuildUser.IsBot)
            {
                logger.LogInformation(
                    "Участник {MemberName} является ботом",
                    notification.SocketGuildUser.GlobalName);

                return;
            }

            await membersManager.WelcomeNewMemberAsync(
                new GuildMember()
                {
                    DiscordId = notification.SocketGuildUser.Id,
                    DisplayName = notification.SocketGuildUser.DisplayName,
                    JoinedAt = notification.SocketGuildUser.JoinedAt ?? DateTimeOffset.Now,
                    IsAuthorized = false
                });
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Ошибка при работе события UserJoinedHandler");
        }
    }
}