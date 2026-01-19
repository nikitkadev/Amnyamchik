using MediatR;
using Discord.WebSocket;

namespace Amnyam._2_Application.Events.ButtonExecuted;

public class ButtonExecuted(SocketMessageComponent socketMessageComponent) : INotification
{
    public SocketMessageComponent SocketMessageComponent { get; } = socketMessageComponent;
}
