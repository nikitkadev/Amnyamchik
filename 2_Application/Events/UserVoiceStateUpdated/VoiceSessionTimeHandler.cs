using MediatR;
using MlkAdmin._2_Application.Interfaces.Managers;

namespace MlkAdmin._2_Application.Events.UserVoiceStateUpdated;

public class VoiceSessionTimeHandler(
    IGuildVoiceSessionsManager voiceSessionsManager) : INotificationHandler<UserVoiceStateUpdated>
{
    public async Task Handle(UserVoiceStateUpdated notification, CancellationToken cancellationToken)
    {
        await voiceSessionsManager.HandleVoiceStateUpdateAsync(
            notification.NewState.VoiceChannel, 
            notification.OldState.VoiceChannel, 
            notification.SocketUser.Id);
    }
}
