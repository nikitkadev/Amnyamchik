using MlkAdmin._1_Domain.Entities;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._1_Domain.Interfaces;

public interface IGuildMessagesRepository
{
    public Task<BaseResult> AddMessageAsync(GuildMessage message, CancellationToken token);
}
