using Discord;
using Amnyam.Shared.Dtos;

namespace Amnyam._2_Application.Interfaces.Services;

public interface IGuildMessagesService
{
    Task SendMessageInChannelAsync(ulong channelId, GuildMessageDto content);
    Task SendMessageComponentInChannelAsync(ulong channelId, MessageComponent component);

}
