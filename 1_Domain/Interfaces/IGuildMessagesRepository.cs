using MlkAdmin._1_Domain.Entities;

namespace MlkAdmin._1_Domain.Interfaces;

public interface IGuildMessagesRepository
{
    public Task AddMessageAsync(GuildMessage message, CancellationToken token);
    Task<IReadOnlyCollection<string?>> GetMessagesColectionByMemberAsync(ulong guildMemberDiscordId);

}
