using Discord;
using MlkAdmin.Shared.Dtos;

namespace MlkAdmin._2_Application.Interfaces.Builders;

public interface IDiscordEmbedBuilder
{
    Task<Embed> BuildEmbedAsync(GuildMessageEmbedDto embedDto);
}