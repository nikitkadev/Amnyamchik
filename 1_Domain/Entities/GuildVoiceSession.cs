namespace MlkAdmin._1_Domain.Entities;

public class GuildVoiceSession
{
    public int Id { get; set; }
    public ulong GuildMemberDiscordId { get; set; }
    public ulong VChannelDiscordId { get; set; }
    public string VChannelName { get; set; } = string.Empty;
    public DateTimeOffset? StartingAt { get; set; }
    public DateTimeOffset? EndingAt { get; set; }
    public long TotalSeconds { get; set; }
}