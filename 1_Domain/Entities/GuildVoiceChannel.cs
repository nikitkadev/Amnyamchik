namespace Amnyam._1_Domain.Entities;

public class GuildVoiceChannel
{
    public int Id { get; set; }
    public ulong DiscordId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public bool IsGen { get; set; }
    public bool IsTemp { get; set; }

}