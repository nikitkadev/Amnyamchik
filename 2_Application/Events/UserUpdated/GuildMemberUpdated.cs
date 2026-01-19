using Discord;
using Discord.WebSocket;
using MediatR;

namespace Amnyam._2_Application.Events.UserUpdated;

public class GuildMemberUpdated(Cacheable<SocketGuildUser, ulong> oldUserState, SocketGuildUser newUserState) : INotification
{
    public Cacheable<SocketGuildUser, ulong> OldUserState { get; } = oldUserState;
    public SocketGuildUser NewUserState { get; } = newUserState;
}
