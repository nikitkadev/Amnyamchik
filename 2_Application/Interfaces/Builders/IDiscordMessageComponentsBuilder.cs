using Discord;
using MlkAdmin.Shared.Dtos;
using MlkAdmin.Shared.Results;

namespace MlkAdmin._2_Application.Interfaces.Builders;

public interface IDiscordMessageComponentsBuilder
{
    Task<BaseResult<SelectMenuBuilder>> BuildSelectionMenuAsync(SelectionMenuConfigDto config);
}
