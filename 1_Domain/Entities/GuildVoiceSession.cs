namespace MlkAdmin._1_Domain.Entities;

public class GuildVoiceSession
{
    public int Id { get; set; }
    public ulong VChannelDiscordId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartingAt { get; set; }
    public long TotalSeconds { get; set; }
}