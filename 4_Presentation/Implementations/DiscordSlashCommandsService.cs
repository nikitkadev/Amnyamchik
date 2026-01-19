using Discord;
using Microsoft.Extensions.Logging;
using Amnyam._3_Infrastructure.Interfaces;
using Amnyam._4_Presentation.Interfaces;
using Amnyam.Shared.Constants;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._4_Presentation.Discord;
public class DiscordSlashCommandsService(
    ILogger<DiscordSlashCommandsService> logger,
    IDiscordService discordService,
    IJsonProvidersHub providersHub) : IDiscordSlashCommandsService
{
    private List<SlashCommandProperties?> SlashGuildCommands { get; set; } = [];

    public async Task RegistrateCommandsAsync()
    {
        try
        {
            await discordService.DiscordClient.Rest.BulkOverwriteGuildCommands([], providersHub.GuildConfigProvidersHub.GuildConfig.GuildDetails.DiscordId);

            SlashGuildCommands.Add(AddLobbyNameCommand());
            SlashGuildCommands.Add(AddAnalysisCommand());
            SlashGuildCommands.Add(AddTestCommand());

            foreach (SlashCommandProperties? command in SlashGuildCommands)
                await discordService.DiscordClient.Rest.CreateGuildCommand(command, providersHub.GuildConfigProvidersHub.GuildConfig.GuildDetails.DiscordId);

        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Ошибка при попытке добавить команды бота\nСообщение: {ErrorMessage}",
                exception.Message);
        }
    }

    private static SlashCommandProperties AddLobbyNameCommand()
    {
        return new SlashCommandBuilder()
            .WithName(MlkAdminConstants.SET_VOICEROOM_COMMAND_NAME)
            .WithDescription("Настраивает имя для создаваемой вами личной комнаты.")
            .AddOption("name", ApplicationCommandOptionType.String, "Имя комнаты", isRequired: true)
            .Build();
    }

    private static SlashCommandProperties? AddAnalysisCommand()
    {
        return new SlashCommandBuilder()
            .WithName(MlkAdminConstants.GUILD_MEMBER_ANALYSIS_COMMAND_NAME)
            .WithDescription("Проанализировать участника")
            .AddOption("member", ApplicationCommandOptionType.User, "Участника сервера", isRequired: true)
            .Build();
    }

    private static SlashCommandProperties? AddTestCommand()
    {
        return new SlashCommandBuilder()
            .WithName(MlkAdminConstants.TESTING_COMMAND_NAME)
            .WithDescription("команда для тестов")
            .AddOption("prompt", ApplicationCommandOptionType.String, "Запрос к ChatGPT", isRequired: true)
            .Build();
    }
   
}
