namespace Amnyam._2_Application.Interfaces.Services;

public interface IGuildRolesService
{
    Task AssignRoleAsync(ulong guildMemberDiscordId, ulong guildRoleDiscordId);
    Task AssignRolesAsync(ulong guildMemberDiscordId, IReadOnlyCollection<ulong> guildRolesDiscordIds);
    Task RemoveRoleAsync(ulong guildMemberDiscordId, ulong guildRoleDiscordId);
    Task RemoveRolesAsync(ulong guildMemberDiscordId, IReadOnlyCollection<ulong> guildRolesDiscordIds);
    Task RemoveRolesByFilterModeAsync(ulong guildMemberDiscordId, IReadOnlyCollection<ulong> guildRolesDiscordIds, bool isMatching);
}