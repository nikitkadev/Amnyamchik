using Discord.WebSocket;
using MediatR;

namespace Amnyam._2_Application.Events.MessageReceived;

public class MessageReceived(SocketMessage socketMessage) : INotification
{
    public SocketMessage SocketMessage { get; set; } = socketMessage;
}
