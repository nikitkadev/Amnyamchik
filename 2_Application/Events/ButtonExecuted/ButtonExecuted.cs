using MediatR;
using Discord.WebSocket;

namespace MlkAdmin._2_Application.Events.ButtonExecuted;

public class ButtonExecuted(SocketMessageComponent socketMessageComponent) : INotification
{
    public SocketMessageComponent SocketMessageComponent { get; } = socketMessageComponent;
}
