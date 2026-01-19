using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Interfaces;
using Amnyam._2_Application.Interfaces.Managers;
using Amnyam._2_Application.Interfaces.Services;

namespace Amnyam._3_Infrastructure.Implementations.Services;

internal class GuildInitializationService(
    ILogger<GuildInitializationService> logger,
	IGuildMembersRepository membersRepository,
	IGuildChannelsRepository channelsRepository,
    IGuildMessagesManager messageManager) : IGuildInitializationService
{
    public async Task InitializeAsync(ulong guildId, CancellationToken token)
    {
		try
		{
			await membersRepository.SyncGuildMembersWithDbAsync(token);
			await channelsRepository.SyncDbVoiceChannelsWithGuildAsync(token);
			await messageManager.RefreshDynamicMessagesAsync();
        }
		catch (DbUpdateException ex)
        {
			logger.LogError(
				ex,
				"Ошибка при попытке синхронизации участников или каналов сервера с DiscordId {GuildDiscordId}",
				guildId);

			return;
		}
    }
}
