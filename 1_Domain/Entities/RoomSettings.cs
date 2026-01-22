namespace Amnyam._1_Domain.Entities;

public class RoomSettings
{
    public ulong GuildMemberDiscordId { get; set; }
    public string? VoiceRoomName { get; set; }
    public int? MembersLimit { get; set; }
    public string? Region { get; set; }
    public bool? IsNSFW { get; set; }
    public int? SlowModeLimit { get; set; }
}
