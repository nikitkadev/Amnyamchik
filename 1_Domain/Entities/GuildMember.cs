namespace Amnyam._1_Domain.Entities;

public class GuildMember
{
    public int Id { get; set; }
    public ulong DiscordId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public bool IsAuthorized { get; set; } = false;
    public DateTimeOffset JoinedAt { get; set; } = DateTimeOffset.Now;
    public string? VoiceRoomName { get; set; }
    public string? RealName {  get; set; }
    public string? TgName { get; set; }
    public DateTimeOffset? Birthday { get; set; } 
}