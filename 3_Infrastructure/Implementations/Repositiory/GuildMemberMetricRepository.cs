using Microsoft.EntityFrameworkCore;
using Amnyam._1_Domain.Entities;
using Amnyam._1_Domain.Interfaces;
using Amnyam._3_Infrastructure.DataBase.EF;

namespace Amnyam._3_Infrastructure.Implementations.Repositiory;

public class GuildMemberMetricRepository(
    MlkAdminDbContext dbContext) : IGuildMemberMetricRepository
{
    private async Task ChangeDbPropertyAsync(ulong guildMemberDiscordId, Action<GuildMemberMetric> action)
    {
        var metric = await dbContext.GuildMemberMetrics.FindAsync(guildMemberDiscordId);

        if (metric is null)
        {
            metric = new GuildMemberMetric()
            {
                MemberDiscordId = guildMemberDiscordId
            };

            await dbContext.GuildMemberMetrics.AddAsync(metric);
        }

        action(metric);

        await dbContext.SaveChangesAsync();
    }

    public async Task IncrementMessageSentCountAsync(ulong guildMemberDiscordId, int increment = 1)
    {
        await ChangeDbPropertyAsync(guildMemberDiscordId, member => member.MessageSentCount += increment);
    }
    public async Task IncrementReactionAddedCountAsync(ulong guildMemberDiscordId, int increment = 1)
    {
        await ChangeDbPropertyAsync(guildMemberDiscordId, member => member.ReactionAddedCount += increment);
    }
    public async Task IncrementCommandSentCountAsync(ulong guildMemberDiscordId, int increment = 1)
    {
        await ChangeDbPropertyAsync(guildMemberDiscordId, member => member.CommandsSentCount += increment);
    }
    public async Task IncrementStickerCountAsync(ulong guildMemberDiscordId, int increment = 1)
    {
        await ChangeDbPropertyAsync(guildMemberDiscordId, member => member.StickersSentCount += increment);
    }
    public async Task IncrementGifSentCountAsync(ulong guildMemberDiscordId, int increment = 1)
    {
        await ChangeDbPropertyAsync(guildMemberDiscordId, member => member.GifsSentCount += increment);
    }
    public async Task IncrementPngPictureSentCountAsync(ulong guildMemberDiscordId, int increment = 1)
    {
        await ChangeDbPropertyAsync(guildMemberDiscordId, member => member.PngPicturesSentCount += increment);
    }
    public async Task UpdateLastMessageDateAsync(ulong guildMemberDiscordId)
    {
        await ChangeDbPropertyAsync(guildMemberDiscordId, member => member.LastMessage = DateTimeOffset.UtcNow);
    }

    public async Task<GuildMemberMetric> GetGuildMemberMetricAsync(ulong guildMemberDiscordId)
    {
        var memberMetric = await dbContext.GuildMemberMetrics.FirstOrDefaultAsync(metric => metric.MemberDiscordId == guildMemberDiscordId);

        if(memberMetric is null)
            return new GuildMemberMetric()
            {
                MemberDiscordId = guildMemberDiscordId
            };

        return memberMetric;
    }
}