using MediatR;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._2_Application.Commands.SetupGuildVoiceRoom;

public class SetupGuildVoiceRoomCommand : IRequest<BaseResult>
{
    public ulong GuildMemberDiscordId { get; set; }
    public string GuildVoiceRoomName { get; set; } = string.Empty;
}
