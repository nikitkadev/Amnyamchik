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

            SlashGuildCommands.Add(AddVoiceRoomSettingsCommand());
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

    private static SlashCommandProperties AddVoiceRoomSettingsCommand()
    {
        return new SlashCommandBuilder()
            .WithName(MlkAdminConstants.SET_VOICEROOM_COMMAND_NAME)
            .WithDescription("Задает настройки создаваемой голосовой комнаты")
            .AddOption("voice_name", ApplicationCommandOptionType.String, "Имя комнаты", isRequired: false)
            .AddOption("members_limit", ApplicationCommandOptionType.Integer, "Ограничения по участникам", isRequired: false)
            .AddOption("region", ApplicationCommandOptionType.String, "Регион подключения", choices: [
                new ApplicationCommandOptionChoiceProperties{ Name = "Brazil", Value = "brazil" },
                new ApplicationCommandOptionChoiceProperties{ Name = "Hong Kong", Value = "hongkong" },
                new ApplicationCommandOptionChoiceProperties{ Name = "India", Value = "india" },
                new ApplicationCommandOptionChoiceProperties{ Name = "Japan", Value = "japan" },
                new ApplicationCommandOptionChoiceProperties{ Name = "Rotterdam", Value = "rotterdam" },
                new ApplicationCommandOptionChoiceProperties{ Name = "Singapore", Value = "singapore" },
                new ApplicationCommandOptionChoiceProperties{ Name = "South Africa", Value = "southafrica" },
                new ApplicationCommandOptionChoiceProperties{ Name = "Sydney", Value = "sydney" },
                new ApplicationCommandOptionChoiceProperties{ Name = "US Central", Value = "us-central" },
                new ApplicationCommandOptionChoiceProperties{ Name = "US East", Value = "us-east" },
                new ApplicationCommandOptionChoiceProperties{ Name = "US South", Value = "us-south" },
                new ApplicationCommandOptionChoiceProperties{ Name = "US West", Value = "us-west" }
                ])
            .AddOption("is_nsfw", ApplicationCommandOptionType.Boolean, "Комната с возрастным ограничением?", isRequired: false)
            .AddOption("slowmode", ApplicationCommandOptionType.Integer, "Режим медленного набора сообщений (в секундах)", isRequired: false, choices: [
                new ApplicationCommandOptionChoiceProperties{ Name = "Выключен", Value = 0 },
                new ApplicationCommandOptionChoiceProperties{ Name = "5 секунд", Value = 5 },
                new ApplicationCommandOptionChoiceProperties{ Name = "10 секунд", Value = 10 },
                new ApplicationCommandOptionChoiceProperties{ Name = "15 секунд", Value = 15 },
                new ApplicationCommandOptionChoiceProperties{ Name = "30 секунд", Value = 30 }
                ])
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
