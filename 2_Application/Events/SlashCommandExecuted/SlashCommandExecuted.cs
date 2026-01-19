using Discord.WebSocket;
using MediatR;

namespace Amnyam._2_Application.Events.SlashCommandExecuted
{
    public class SlashCommandExecuted(SocketSlashCommand socketSlashCommand) : INotification
    {
        public SocketSlashCommand SocketSlashCommand { get; set; } = socketSlashCommand;
    }
}
