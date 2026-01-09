using MediatR;
using Microsoft.Extensions.Logging;
using MlkAdmin._2_Application.Commands.SetupGuildVoiceRoom;
using MlkAdmin._2_Application.Commands.Test;
using MlkAdmin._2_Application.Interfaces.Managers;
using MlkAdmin.Shared.Constants;
using MlkAdmin.Shared.JsonProviders;

namespace MlkAdmin._2_Application.Events.SlashCommandExecuted;

public class SlashCommandExecutedHandler(
    ILogger<SlashCommandExecutedHandler> logger,
    IMediator mediator,
    IJsonProvidersHub providersHub,
    IGuildMessagesManager messagesManager) : INotificationHandler<SlashCommandExecuted>
{
    public async Task Handle(SlashCommandExecuted notification, CancellationToken token)
    {
        await notification.SocketSlashCommand.DeferAsync(ephemeral: true);

        var command = notification.SocketSlashCommand;

        if (command.Channel.Id != providersHub.GuildConfigProvidersHub.Channels.TextChannels.BotCommandsText.DiscordId)
        {
            await messagesManager.SendDefaultResponseAsync(
                notification.SocketSlashCommand, 
                $"Команды бота можно вызывать только в канале {providersHub.GuildConfigProvidersHub.Channels.TextChannels.BotCommandsText.Https}");

            return;
        }

        switch (command.CommandName)
        {
            case MlkAdminConstants.SET_VOICEROOM_COMMAND_NAME:

                try
                {
                    var voiceRoomName = command.Data.Options
                        .FirstOrDefault(
                            x => x.Name.Equals("name", StringComparison.Ordinal)).Value.ToString() ?? string.Empty;

                    var updateVoiceRoomNameResult = await mediator.Send(
                        new SetupGuildVoiceRoomCommand()
                        {
                            GuildMemberDiscordId = command.User.Id,
                            GuildVoiceRoomName = voiceRoomName,
                        },
                        token
                    );

                    await messagesManager.SendDefaultResponseAsync(notification.SocketSlashCommand, updateVoiceRoomNameResult.ClientMessage);

                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Ошибка при попытке обработать команду {CommandName} настройки личной голосовой комнаты",
                        MlkAdminConstants.SET_VOICEROOM_COMMAND_NAME);

                    break;
                }

            case MlkAdminConstants.TESTING_COMMAND_NAME:

                try
                {
                    var testResponse = await mediator.Send(
                        new TestCommand()
                        {
                            MemberPrompt = command.Data.Options
                                .FirstOrDefault(
                                    x => x.Name == "prompt").Value.ToString() ?? "Технические шоколадки"
                        },
                        token
                    );

                    await messagesManager.SendDefaultResponseAsync(command, testResponse.ClientMessage);

                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Ошибка при попытке обработать команду {CommandName} настройки личной голосовой комнаты",
                        MlkAdminConstants.TESTING_COMMAND_NAME);

                    break;
                }

        }
    }
}
