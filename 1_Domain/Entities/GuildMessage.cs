namespace Amnyam._1_Domain.Entities;

public class GuildMessage
{
    public int Id { get; set; }
    public ulong MessageDiscordId { get; set; }
    public ulong SenderDiscordId { get; set; }
    public string? Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public ulong TChannelId { get; set; }
    public string TChannelName { get; set; } = string.Empty;
}