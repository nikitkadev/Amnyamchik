using MediatR;
using Microsoft.Extensions.Logging;
using Amnyam._4_Presentation.Interfaces;

namespace Amnyam._2_Application.Events.Ready;

public class ReadyHandler(
    ILogger<ReadyHandler> logger,
    IDiscordSlashCommandsService commandsService) : INotificationHandler<Ready>
{
    public async Task Handle(Ready notification, CancellationToken cancellationToken)
    {
        try
        {
            await commandsService.RegistrateCommandsAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception, 
                "Error: {Message} StackTrace: {StackTrace}", 
                exception.Message, 
                exception.StackTrace);
        }
    }
}
