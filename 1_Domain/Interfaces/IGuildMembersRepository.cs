using Amnyam._1_Domain.Entities;

namespace Amnyam._1_Domain.Interfaces;

public interface IGuildMembersRepository
{
    Task UpsertGuildMemberAsync(GuildMember guildMember, CancellationToken token = default);
    Task <GuildMember> GetGuildMemberEntityAsync(ulong guildMemberDiscordId, CancellationToken token = default);
    Task RemoveGuildMemberEntityFromDbAsync(ulong guildMemberDiscordId, CancellationToken token = default);
    Task SyncGuildMembersWithDbAsync(CancellationToken token = default);
    Task<bool> IsAuthorizedAsync(ulong guildMemberDiscordId, CancellationToken token = default);
    Task<long> GetTotalSecondsInVoiceChannelsByMemberDiscordIdAsync(ulong guildMemberDiscordId);
}
