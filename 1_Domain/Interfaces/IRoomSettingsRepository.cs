using Amnyam._1_Domain.Entities;

namespace Amnyam._1_Domain.Interfaces;

public interface IRoomSettingsRepository
{
    Task<RoomSettings> GetRoomSettingsByGuildMemberDiscordIdAsync(ulong guildMemberDiscordId, CancellationToken token = default);
    Task UpsertRoomSettingsAsync(RoomSettings roomSettings, CancellationToken token = default);
}
