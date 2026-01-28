using Amnyam.Shared.Results;
using MediatR;

namespace Amnyam._2_Application.Commands.RemoveVoiceRoomSettingsCommand;

public class RemoveVoiceRoomSettingsCommand : IRequest<BaseResult>
{
    public ulong GuildMemberDiscordId { get; set; }
}
