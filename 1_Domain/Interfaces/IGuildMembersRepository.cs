using MlkAdmin._1_Domain.Entities;

namespace MlkAdmin._1_Domain.Interfaces;

public interface IGuildMembersRepository
{
    Task UpsertGuildMemberAsync(GuildMember guildMember, CancellationToken token = default);
    Task <GuildMember> GetGuildMemberEntityAsync(ulong guildMemberDiscordId, CancellationToken token = default);
    Task RemoveGuildMemberEntityFromDbAsync(ulong guildMemberDiscordId, CancellationToken token = default);
    Task SyncGuildMembersWithDbAsync(CancellationToken token = default);
    Task <string> GetVoiceRoomNameAsync(ulong guildMemberDiscordId, CancellationToken token = default);
    Task UpdateVoiceRoomNameAsync(ulong guildMemberDiscordId, string voiceRoomName, CancellationToken token = default);
    Task<bool> IsAuthorizedAsync(ulong guildMemberDiscordId, CancellationToken token = default);
}
