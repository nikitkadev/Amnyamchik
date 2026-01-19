using Discord;
using Amnyam.Shared.Dtos;

namespace Amnyam._2_Application.Interfaces.Builders;

public interface IDiscordEmbedBuilder
{
    Task<Embed> BuildEmbedAsync(GuildMessageEmbedDto embedDto);
}