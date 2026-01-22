using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Interfaces;
using Amnyam._3_Infrastructure.DataBase.EF;

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

            if(roomSettings.IsNSFW is not null)
                settings.IsNSFW = roomSettings.IsNSFW;

            if (roomSettings.SlowModeLimit is not null)
                settings.SlowModeLimit = roomSettings.SlowModeLimit;

            return;
        }

        await dbContext.RoomSettings.AddAsync(roomSettings, token);
        await dbContext.SaveChangesAsync(token);
    }

    public async Task<RoomSettings> GetRoomSettingsByGuildMemberDiscordIdAsync(ulong guildMemberDiscordId, CancellationToken token = default)
    {
        var settings = await dbContext.RoomSettings.FindAsync(guildMemberDiscordId);

        return settings ?? new RoomSettings() { GuildMemberDiscordId = guildMemberDiscordId};
    }
}
