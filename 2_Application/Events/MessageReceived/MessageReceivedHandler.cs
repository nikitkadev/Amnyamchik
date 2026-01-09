using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MlkAdmin._1_Domain.Interfaces;

namespace MlkAdmin._2_Application.Events.MessageReceived;

public class MessageReceivedHandler(
    ILogger<MessageReceivedHandler> logger,
    IGuildMessagesRepository messageRepository) : INotificationHandler<MessageReceived>
{
    public async Task Handle(MessageReceived notification, CancellationToken token)
    {
        try
        {
            if (notification.SocketMessage.Author.IsBot)
                return;

            await messageRepository.AddMessageAsync(
                new()
                {
                    SenderDiscordId = notification.SocketMessage.Author.Id,
                    MessageDiscordId = notification.SocketMessage.Id,
                    SentAt = DateTime.UtcNow,
                    TChannelId = notification.SocketMessage.Channel.Id,
                    Content = notification.SocketMessage.Content
                }, 
                token
            );
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке обработать входящее пользовательское сообщение с DiscordId {GuildMessageDiscordId}",
                notification.SocketMessage.Id);

            return;
        }
    }
}