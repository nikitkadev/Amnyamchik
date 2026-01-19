using MediatR;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Interfaces;
using Amnyam._2_Application.Interfaces.Services;

namespace Amnyam._2_Application.Events.UserVoiceStateUpdated;

class UserVoiceStateUpdatedHandler(
    ILogger<UserVoiceStateUpdatedHandler> logger,
    IGuildChannelsRepository channelsRepository,
    IGuildChannelsService channelsService) : INotificationHandler<UserVoiceStateUpdated>
{
    public async Task Handle(UserVoiceStateUpdated notification, CancellationToken token)
    {
        try
        {
            if (notification.SocketUser is not SocketGuildUser guildUser)
                return;


            if (notification.OldState.VoiceChannel != null)
            {
                if(await channelsRepository.IsTemporaryVoiceChannel(notification.OldState.VoiceChannel.Id, token) && notification.OldState.VoiceChannel.ConnectedUsers.Count == 0)
                {
                    await channelsRepository.RemoveDbVoiceChannelAsync(notification.OldState.VoiceChannel.Id);
                    await notification.OldState.VoiceChannel.DeleteAsync();
                }
            }

            if (notification.NewState.VoiceChannel != null)
            {
                if (await channelsRepository.IsGeneratingVoiceChannel(notification.NewState.VoiceChannel.Id, token))
                {
                    var brandNewRestChannel = await channelsService.CreateVoiceChannelAsync(notification.SocketUser.Id);

                    var dbVChannel = new GuildVoiceChannel()
                    {
                        Category = notification.NewState.VoiceChannel.Category.ToString(),
                        DiscordId = brandNewRestChannel.Id,
                        Name = brandNewRestChannel.Name,
                        IsGen = false,
                        IsTemp = true
                    };

                    await channelsRepository.UpsertDbVoiceChannelAsync(dbVChannel);
                    await guildUser.ModifyAsync(properties => properties.ChannelId = brandNewRestChannel.Id);
                }
            }
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Ошибка в обработчике события UserVoiceStateUpdated\nСообщение: {ErrorMessage}",
                exception.Message);

            return;
        }
    }
}