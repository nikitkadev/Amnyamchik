using MediatR;
using Microsoft.Extensions.Logging;
using Amnyam._2_Application.Interfaces.Managers;

namespace Amnyam._2_Application.Events.UserLeft;

class UserLeftHandler(
    ILogger<UserLeftHandler> logger,
    IGuildMembersManager membersManager) : INotificationHandler<UserLeft>
{
    public async Task Handle(UserLeft notification, CancellationToken cancellationToken)
    {
        try
        {
            await membersManager.DeauthorizeGuildMemberAsync(notification.SocketUser.Id, notification.SocketUser.GlobalName);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ошибка про попытке обработать уход участника {GuildMemberDiscordName}:{GuildMemberDiscordId} с сервера",
                notification.SocketUser.GlobalName, 
                notification.SocketUser.Id);

            return;
        }
    }
}
