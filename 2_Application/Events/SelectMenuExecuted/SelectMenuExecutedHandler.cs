using MediatR;
using Microsoft.Extensions.Logging;
using Amnyam._2_Application.Interfaces.Managers;
using Amnyam.Shared.Constants;

namespace Amnyam._2_Application.Events.SelectMenuExecuted;

class SelectMenuExecutedHandler(
    ILogger<SelectMenuExecutedHandler> logger,
    IGuildMembersManager membersManager,
    IGuildMessagesManager messagesManager) : INotificationHandler<SelectMenuExecuted>
{
    public async Task Handle(SelectMenuExecuted notification, CancellationToken cancellationToken)
    {
        try
        {
            await notification.SocketMessageComponent.DeferAsync();

            var values = notification.SocketMessageComponent.Data.Values;

            switch (notification.SocketMessageComponent.Data.CustomId)
            {
                case MlkAdminConstants.SELECTION_MENU_COLORS_CUSTOM_ID:

                    var changeResult = await membersManager.UpdateGuildMemberColorRoleAsync(
                        notification.SocketMessageComponent.User.Id,
                        values.First());

                    await messagesManager.SendDefaultResponseAsync(
                        notification.SocketMessageComponent, 
                        changeResult.ClientMessage);

                    break;

                default:

                    logger.LogWarning(
                        "Использованное меню {SelectionMenuId} еще не добавлено в обработчик событий",
                        notification.SocketMessageComponent.Data.CustomId);

                    await messagesManager.SendDefaultResponseAsync(
                        notification.SocketMessageComponent, 
                        "Это меню выбора еще не запрограмммировано..");

                    break;
            }
		}
		catch (Exception ex)
		{
            logger.LogError(
                ex, 
                "Ошибка при попытке обработать вызов меню выбора с DiscordId {GuildMenuSelectionDiscordId}",
                notification.SocketMessageComponent.Id);

            return;
        }
    }
}