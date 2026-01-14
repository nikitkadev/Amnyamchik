using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MlkAdmin._1_Domain.Entities;
using MlkAdmin._1_Domain.Interfaces;

namespace MlkAdmin._2_Application.Events.MessageReceived;

public class MessageReceivedHandler(
    ILogger<MessageReceivedHandler> logger,
    IGuildMessagesRepository messageRepository,
    IGuildMemberMetricRepository metricRepository) : INotificationHandler<MessageReceived>
{
    public async Task Handle(MessageReceived notification, CancellationToken token)
    {
        try
        {
            if (notification.SocketMessage.Author.IsBot)
                return;


            await messageRepository.AddMessageAsync(
                new GuildMessage()
                {
                    SenderDiscordId = notification.SocketMessage.Author.Id,
                    MessageDiscordId = notification.SocketMessage.Id,
                    SentAt = DateTime.UtcNow,
                    TChannelId = notification.SocketMessage.Channel.Id,
                    Content = notification.SocketMessage.Content ?? null
                }, 
                token
            );

            await metricRepository.UpdateLastMessageDateAsync(notification.SocketMessage.Author.Id);
            await metricRepository.IncrementMessageSentCountAsync(notification.SocketMessage.Author.Id);

            var attachments = notification.SocketMessage.Attachments;

            if (notification.SocketMessage.Stickers.Count > 0)
                await metricRepository.IncrementStickerCountAsync(notification.SocketMessage.Author.Id);

            if (attachments.Any(message => message.Filename.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)))
                await metricRepository.IncrementGifSentCountAsync(notification.SocketMessage.Author.Id);

            if (attachments.Any(message => message.Filename.EndsWith(".png", StringComparison.OrdinalIgnoreCase)))
                await metricRepository.IncrementPngPictureSentCountAsync(notification.SocketMessage.Author.Id);

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