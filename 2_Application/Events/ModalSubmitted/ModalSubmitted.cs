using MediatR;
using Discord.WebSocket;

namespace Amnyam._2_Application.Events.ModalSubmitted;

class ModalSubmitted(SocketModal modal) : INotification
{
    public SocketModal Modal { get; } = modal;
}
