namespace Amnyam.Shared.Dtos;

public record GuildMemberAnalysisResultData
{
    public ulong GuildMemberDiscordId { get; set; }

    public DateTimeOffset? JoinedAt { get; init; }
    public DateTimeOffset? FirstMessageDate { get; init; }
    public DateTimeOffset? LastMessageDate { get; init; }
    public int DaysSinceJoined { get; init; }

    public int MessageCount { get; init; }
    public int ReactionCount { get; init; }
    public int PicturesSentCount { get; init; }
    public int GifsSentCount { get; init; }
    public int CommandsSentCount { get; init; }
    public long VoiceChannelsTimeSpent { get; init; }

    public float AvgToxicity { get; init; }
    public string MostToxicMessage { get; init; } = string.Empty;
    public string SpeechStyle { get; init; } = string.Empty;
    public string Tonality { get; init; } = string.Empty;
    public float AvgCharsInMessage { get; init; }

}
