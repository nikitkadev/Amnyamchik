using Discord;
using Amnyam.Shared.Results;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Enums;
using Amnyam._2_Application.Interfaces.Builders;
using Amnyam.Shared.Dtos;

namespace Amnyam._3_Infrastructure.Services;

public class DiscordMessageComponentsBuilder(ILogger<DiscordMessageComponentsBuilder> logger) : IDiscordMessageComponentsBuilder
{
    public async Task<BaseResult<SelectMenuBuilder>> BuildSelectionMenuAsync(SelectionMenuConfigDto config)
    {
        try
        {
            var menu = new SelectMenuBuilder()
            .WithPlaceholder(config.Placeholder)
            .WithCustomId(config.CustomId);

            foreach (var option in config.Options)
                menu.AddOption(option.Label, option.Value);

            return BaseResult<SelectMenuBuilder>.Success(menu);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Ошибка при попытке построить компонент сообщения\nСообщение: {ErrorMessage}",
                exception.Message);

            return BaseResult<SelectMenuBuilder>.Fail(
                new(
                    ErrorCodes.INTERNAL_ERROR,
                    "Смотреть logger"));
        }
    }
}
