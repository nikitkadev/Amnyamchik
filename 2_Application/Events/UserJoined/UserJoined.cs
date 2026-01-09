using Discord.WebSocket;
using MediatR;

namespace MlkAdmin._2_Application.Events.UserJoined
{
    class UserJoined(SocketGuildUser socketGuildUser) : INotification
    {
        public SocketGuildUser SocketGuildUser { get; } = socketGuildUser;
    }
}
