using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Exceptions;
using Amnyam._2_Application.Interfaces.Services;
using Amnyam._3_Infrastructure.Interfaces;

namespace Amnyam._3_Infrastructure.Services;

public class GuildRolesService(
    ILogger<GuildRolesService> logger,
    IDiscordService discordService) : IGuildRolesService
{
    public async Task AssignRoleAsync(ulong guildMemberDiscordId, ulong guildRoleDiscordId)
    {
        var socketGuildMember = await discordService.GetGuildMemberAsync(guildMemberDiscordId)
            ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        await socketGuildMember.AddRoleAsync(guildRoleDiscordId);

        logger.LogInformation(
            "Роль {GuildRoleDiscordId} была добавлена пользователю {GuildMemberName}:{GuildMemberDiscordId}",
            guildRoleDiscordId,
            socketGuildMember.DisplayName,
            socketGuildMember.Id);
    }

    public async Task AssignRolesAsync(ulong guildMemberDiscordId, IReadOnlyCollection<ulong> guildRolesDiscordIds)
    {
        var socketGuildMember = await discordService.GetGuildMemberAsync(guildMemberDiscordId)
            ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        await socketGuildMember.AddRolesAsync(guildRolesDiscordIds);

        logger.LogInformation(
            "Роли {GuildRolesDiscordId} были добавлена пользователю {GuildMemberName}:{GuildMemberDiscordId}",
            string.Join(",", guildRolesDiscordIds),
            socketGuildMember.DisplayName,
            socketGuildMember.Id);
    }

    public async Task RemoveRoleAsync(ulong guildMemberDiscordId, ulong guildRoleDiscordId)
    {
        var socketGuildMember = await discordService.GetGuildMemberAsync(guildMemberDiscordId)
            ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        await socketGuildMember.RemoveRoleAsync(guildRoleDiscordId);

        logger.LogInformation(
            "Роль {GuildRoleDiscordId} была удалена у пользователя {GuildMemberName}:{GuildMemberDiscordId}",
            guildRoleDiscordId,
            socketGuildMember.DisplayName,
            socketGuildMember.Id);
    }

    public async Task RemoveRolesAsync(ulong guildMemberDiscordId, IReadOnlyCollection<ulong> guildRolesDiscordIds)
    {
        var socketGuildMember = await discordService.GetGuildMemberAsync(guildMemberDiscordId)
            ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        await socketGuildMember.RemoveRolesAsync(guildRolesDiscordIds);

        logger.LogInformation(
            "Роли {GuildRolesDiscordId} были удалены у пользователя {GuildMemberName}:{GuildMemberDiscordId}",
            string.Join(",", guildRolesDiscordIds),
            socketGuildMember.DisplayName,
            socketGuildMember.Id);
    }

    public async Task RemoveRolesByFilterModeAsync(ulong guildMemberDiscordId, IReadOnlyCollection<ulong> guildRolesDiscordIds, bool isMatching)
    {
        var socketGuildMember = await discordService.GetGuildMemberAsync(guildMemberDiscordId)
            ?? throw new GuildMemberNotFoundException(guildMemberDiscordId);

        var guildMemberRoles = socketGuildMember.Roles;

        var toRemove = isMatching
            ? [.. guildMemberRoles.Where(role => guildRolesDiscordIds.Contains(role.Id))]
            : guildMemberRoles.Where(role => !guildRolesDiscordIds.Contains(role.Id)).ToList();

        if (toRemove.Count == 0)
        {
            logger.LogInformation(
                "У пользователя {memberId} нет ролей, IDs которых содержатся в переданной коллекции\n" +
                "Роли пользователя: {guildMemberRoles}\n" +
                "Роли в переданной коллекции: {rolesIds}",
                guildMemberDiscordId,
                string.Join(":", guildMemberRoles.Select(memberRole => memberRole.Id).ToList()),
                string.Join(":", guildRolesDiscordIds));

            return;
        }

        await socketGuildMember.RemoveRolesAsync(toRemove);

        logger.LogInformation(
            "Роли {toRemove} успешно удалены у пользователя {GuildMemberDiscordId}",
            string.Join(",", toRemove),
            guildMemberDiscordId);
    }
}