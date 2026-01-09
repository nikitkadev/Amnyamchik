using Discord;
using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._2_Application.Interfaces.Services;

public interface IGuildMessagesService
{
    Task SendMessageInChannelAsync(ulong channelId, GuildMessageDto content);
    Task SendMessageComponentInChannelAsync(ulong channelId, MessageComponent component);

}
