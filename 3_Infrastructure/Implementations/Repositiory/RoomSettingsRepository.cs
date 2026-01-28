using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Interfaces;
using Amnyam._3_Infrastructure.DataBase.EF;
using Microsoft.EntityFrameworkCore;

namespace Amnyam._3_Infrastructure.Implementations.Repositiory;

public class RoomSettingsRepository(MlkAdminDbContext dbContext) : IRoomSettingsRepository
{
    public async Task UpsertRoomSettingsAsync(RoomSettings roomSettings, CancellationToken token = default)
    {
        var settings = dbContext.RoomSettings
            .FirstOrDefault(rs => rs.GuildMemberDiscordId == roomSettings.GuildMemberDiscordId);

        if (settings is not null)
        {
            if(roomSettings.VoiceRoomName is not null)
                settings.VoiceRoomName = roomSettings.VoiceRoomName;

            if(roomSettings.MembersLimit is not null)
                settings.MembersLimit = roomSettings.MembersLimit;

            if(roomSettings.Region is not null)
                settings.Region = roomSettings.Region;

            return;
        }

        await dbContext.RoomSettings.AddAsync(roomSettings, token);
        await dbContext.SaveChangesAsync(token);
    }

    public async Task<RoomSettings> GetRoomSettingsByGuildMemberDiscordIdAsync(ulong guildMemberDiscordId, CancellationToken token = default)
    {
        var settings = await dbContext.RoomSettings.FindAsync([guildMemberDiscordId], cancellationToken: token);

        return settings ?? new RoomSettings() { GuildMemberDiscordId = guildMemberDiscordId};
    }

    public async Task RemoveRoomSettingsByGuildMemberDiscordIdAsync(ulong guildMemberDiscordId, CancellationToken token = default)
    {
        var settings = await dbContext.RoomSettings
            .FirstOrDefaultAsync(rs => rs.GuildMemberDiscordId == guildMemberDiscordId, cancellationToken: token) ?? throw new ArgumentNullException($"Настроек комнаты для участника с DiscordId {guildMemberDiscordId} не найдены");

        dbContext.RoomSettings.Remove(settings);

        await dbContext.SaveChangesAsync(token);
    }
}
