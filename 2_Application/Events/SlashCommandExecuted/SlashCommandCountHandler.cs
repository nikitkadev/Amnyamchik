using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MlkAdmin._1_Domain.Interfaces;

namespace MlkAdmin._2_Application.Events.SlashCommandExecuted;

public class SlashCommandCountHandler(
    ILogger<SlashCommandCountHandler> logger,
    IGuildMemberMetricRepository metricRepository) : INotificationHandler<SlashCommandExecuted>
{
    public async Task Handle(SlashCommandExecuted notification, CancellationToken cancellationToken)
    {
		try
		{
			await metricRepository.IncrementCommandSentCountAsync(notification.SocketSlashCommand.User.Id);
		}
		catch (DbUpdateException ex)
		{
			logger.LogError(
				ex,
				"Ошибка при попытке взаимодействия с базой данных");
		}
    }
}
