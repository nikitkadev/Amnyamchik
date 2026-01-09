using Discord.WebSocket;
using MediatR;

namespace MlkAdmin._2_Application.Events.GuildAvailable;

class GuildAvailable(SocketGuild socketGuild) : INotification
{
    public SocketGuild SocketGuild { get; } = socketGuild;
}
