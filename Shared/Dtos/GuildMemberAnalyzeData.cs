namespace MlkAdmin.Shared.Dtos;

public record GuildMemberAnalyzeData
{
    public ulong MemberDiscordId { get; init; }
    public int MessageCount { get; init; }
    public int ReactionCount { get; init; }
    public int PicturesSent { get; init; }
    public int GifsSent { get; init; }
    public int CommandsSent { get; init; }
    public int BanWordsSent { get; init; }
    public int DaysSinceJoined { get; init; }
    public float Toxicity {  get; init; }
    public long VoiceChannelsTimeSpent { get; init; }


    public DateTime JoinedAt { get; init; }
    public DateTime FirstMessageDate { get; init; }
    public DateTime LastMessageDate { get; init; }

}
