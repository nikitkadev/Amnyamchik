using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MlkAdmin._1_Domain.Interfaces;
using MlkAdmin._2_Application.Interfaces.Managers;
using MlkAdmin._2_Application.Interfaces.Services;

namespace MlkAdmin._3_Infrastructure.Implementations.Services;

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
