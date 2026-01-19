using Discord;
using Amnyam.Shared.Dtos;
using Amnyam.Shared.Results;

namespace Amnyam._2_Application.Interfaces.Builders;

public interface IDiscordMessageComponentsBuilder
{
    Task<BaseResult<SelectMenuBuilder>> BuildSelectionMenuAsync(SelectionMenuConfigDto config);
}
