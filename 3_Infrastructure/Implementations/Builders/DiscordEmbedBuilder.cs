using Discord;
using Microsoft.Extensions.Logging;
using Amnyam._2_Application.Interfaces.Builders;
using Amnyam.Shared.Dtos;

namespace Amnyam._3_Infrastructure.Implementations.Builders;

public class DiscordEmbedBuilder(ILogger<DiscordEmbedBuilder> logger) : IDiscordEmbedBuilder
{
    public async Task<Embed> BuildEmbedAsync(GuildMessageEmbedDto embedDto)
    {
        try
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle(embedDto.Title)
                .WithDescription(embedDto.Description)
                .WithColor(embedDto.Color);

            if (embedDto.AuthorDto is not null)
            {
                embedBuilder.WithAuthor(new EmbedAuthorBuilder()
                    .WithUrl(embedDto.AuthorDto.Url)
                    .WithName(embedDto.AuthorDto.Name)
                    .WithIconUrl(embedDto.AuthorDto.IconUrl));
            }

            if (embedDto.FooterDto is not null)
            {
                embedBuilder.WithFooter(new EmbedFooterBuilder()
                    .WithText(embedDto.FooterDto.Text)
                    .WithIconUrl(embedDto.FooterDto.IconUrl));
            }

            if (embedDto.WithTimestamp)
            {
                embedBuilder.WithCurrentTimestamp();
            }

            if (embedDto.EmbedFields is not null)
            {
                embedBuilder.WithFields(embedDto.EmbedFields);
            }

            logger.LogInformation(
                "Embed успешно сформирован");


            return embedBuilder.Build();
            
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Ошибка при попытке построить Embed");

            return new EmbedBuilder().Build();
        }
    }
}
