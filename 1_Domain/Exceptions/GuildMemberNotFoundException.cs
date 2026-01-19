namespace Amnyam._1_Domain.Exceptions;

public sealed class GuildMemberNotFoundException(ulong guildMemberDiscordId) : DomainException($"Участник с DiscordId {guildMemberDiscordId} не найден")
{
    public ulong GuildMemberDiscordId { get; private set; } = guildMemberDiscordId;
}
