using Discord.WebSocket;
using MediatR;

namespace MlkAdmin._2_Application.Events.UserLeft;

class UserLeft(SocketGuild socketGuild, SocketUser socketUser) : INotification
{
    public SocketGuild SocketGuild { get; } = socketGuild;
    public SocketUser SocketUser { get; } = socketUser;
}
