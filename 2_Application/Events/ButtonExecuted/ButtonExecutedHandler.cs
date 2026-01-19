using MediatR;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Amnyam._2_Application.Interfaces.Managers;
using Amnyam.Shared.Constants;

namespace Amnyam._2_Application.Events.ButtonExecuted;

public class ButtonExecutedHandler(
    ILogger<ButtonExecutedHandler> logger,
    IGuildMembersManager membersManager,
    IGuildMessagesManager messagesManager) : INotificationHandler<ButtonExecuted>
{
    public async Task Handle(ButtonExecuted notification, CancellationToken cancellationToken)
    {
        try
        {
            await notification.SocketMessageComponent.DeferAsync(ephemeral: true);
            
            var socketGuildUser = notification.SocketMessageComponent.User as SocketGuildUser 
                ?? throw new InvalidOperationException("Пользователь не является участником сервера");

            switch (notification.SocketMessageComponent.Data.CustomId)
            {
                case MlkAdminConstants.BUTTONS_AUTHORIZATION_CUSTOM_ID:

                    var authResult = await membersManager.AuthorizeGuildMemberAsync(
                        socketGuildUser.Id, 
                        socketGuildUser.Mention);

                    await messagesManager.SendDefaultResponseAsync(
                        notification.SocketMessageComponent, 
                        authResult.ClientMessage);

                    break;

                case MlkAdminConstants.BUTTONS_RULES_CUSTOM_ID:

                    await messagesManager.SendRulesMessageResponseAsync(
                        notification.SocketMessageComponent);

                    break;

                case MlkAdminConstants.BUTTONS_COLORS_CUSTOM_ID:

                    await messagesManager.SendColorsMenuResponseAsync(
                        notification.SocketMessageComponent);

                    break;

                default:

                    await messagesManager.SendDefaultResponseAsync(
                        notification.SocketMessageComponent, 
                        "Кнопелка еще не запрограммирована..");

                    break;
            }
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex,
                "Ошибка при попытке обработать взаимодействие с кнопкой с CustomId {CustomId}",
                notification.SocketMessageComponent.Data.CustomId);
        }
    }
}
