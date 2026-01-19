namespace Amnyam._1_Domain.Entities;

public class GuildMemberMetric
{
    public ulong MemberDiscordId { get; set; }

    public int MessageSentCount { get; set; } = 0;
    public int ReactionAddedCount { get; set; } = 0;
    public int CommandsSentCount { get; set; } = 0;
    public int StickersSentCount { get; set; } = 0;
    public int GifsSentCount { get; set; } = 0;
    public int PngPicturesSentCount { get; set; } = 0;
    
    public DateTimeOffset? FirstMessage { get; set; }
    public DateTimeOffset? LastMessage { get; set; }
}
