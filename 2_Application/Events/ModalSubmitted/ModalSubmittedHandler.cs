using MediatR;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Amnyam._2_Application.Events.ModalSubmitted;

class ModalSubmittedHandler(
    ILogger<ModalSubmittedHandler> logger) : INotificationHandler<ModalSubmitted>
{
    public async Task Handle(ModalSubmitted notification, CancellationToken cancellationToken)
    {
        await notification.Modal.DeferAsync(ephemeral: true);

        if (notification.Modal.User is not SocketGuildUser socketGuildUser)
        {
            logger.LogWarning(
                "Пользователь, отправивший модальное окно, не является участником сервера");

            return;
        }

        switch (notification.Modal.Data.CustomId)
        {
            default:
                logger.LogInformation(
                    "Неизвестный CustomId: {CustomId}",
                    notification.Modal.Data.CustomId);

                break;

        }

        await Task.CompletedTask;
    }
}