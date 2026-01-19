using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Amnyam._2_Application.Interfaces.Builders;
using Amnyam._2_Application.Interfaces.Services;
using Amnyam.Shared.Dtos;

namespace Amnyam._2_Application.Implementations.Services;

public class GuildMessagesService(
    ILogger<GuildMessagesService> logger,
    IGuildChannelsService channelsService,
	IDiscordEmbedBuilder embedBuilder) : IGuildMessagesService
{
    public async Task SendMessageComponentInChannelAsync(ulong channelId, MessageComponent component)
    {
        var channel = await channelsService.GetGuildChannelByDiscordIdAsync(channelId);

        if (channel is not SocketTextChannel textChannel)
        {
            logger.LogWarning(
                "Канал с Id {ChannelId} не найден или не является текстовым", channelId);

            return;
        }

        await textChannel.SendMessageAsync(
            components: component);
    }

    public async Task SendMessageInChannelAsync(ulong channelId, GuildMessageDto content)
    {
		try
		{
            var channel = await channelsService.GetGuildChannelByDiscordIdAsync(channelId);

			if(channel is not SocketTextChannel textChannel)
			{
                logger.LogWarning(
                    "Канал с Id {ChannelId} не найден или не является текстовым", channelId);

                return;
            }

			var embed = await embedBuilder.BuildEmbedAsync(
				new GuildMessageEmbedDto()
				{
                    Title = content.Embed.Title,
                    Description = content.Embed.Description
                }
			);

            await textChannel.SendMessageAsync(embed: embed);
			
        }
		catch (Exception exception)
		{
			logger.LogError(
				exception, 
				"Ошибка при попытке отправить сообщение в канал {ChannelId}. Ошибка {Exception}",
				channelId,
				exception.Message);

			return;
		}
    }
}
