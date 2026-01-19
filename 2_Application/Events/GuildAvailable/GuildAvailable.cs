using Discord.WebSocket;
using MediatR;

namespace Amnyam._2_Application.Events.GuildAvailable;

class GuildAvailable(SocketGuild socketGuild) : INotification
{
    public SocketGuild SocketGuild { get; } = socketGuild;
}
