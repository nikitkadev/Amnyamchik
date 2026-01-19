using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Interfaces;

namespace Amnyam._2_Application.Events.ReactionAdded;

public class ReactionAddedHandler(
    ILogger<ReactionAddedHandler> logger,
    IGuildMemberMetricRepository metricRepository) : INotificationHandler<ReactionAdded>
{
    public async Task Handle(ReactionAdded notification, CancellationToken cancellationToken)
    {
		try
		{
			if (notification.Reaction.User.Value.IsBot)
				return;

			await metricRepository.IncrementReactionAddedCountAsync(notification.Reaction.UserId);
		}
		catch (DbUpdateException ex)
		{
			logger.LogError(
				ex,
				"Ошибка при попытке взаимодействия с базой данных");

			return;
		}
    }
}