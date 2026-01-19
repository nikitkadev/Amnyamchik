using Discord.WebSocket;
using MediatR;

namespace Amnyam._2_Application.Events.UserJoined
{
    class UserJoined(SocketGuildUser socketGuildUser) : INotification
    {
        public SocketGuildUser SocketGuildUser { get; } = socketGuildUser;
    }
}
