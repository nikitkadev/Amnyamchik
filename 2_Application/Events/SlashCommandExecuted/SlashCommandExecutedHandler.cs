using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Amnyam._2_Application.Commands.AnalyzeGuildMember;
using Amnyam._2_Application.Commands.SetupGuildVoiceRoom;
using Amnyam._2_Application.Commands.Test;
using Amnyam._2_Application.Interfaces.Managers;
using Amnyam.Shared.Constants;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._2_Application.Events.SlashCommandExecuted;

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

            case MlkAdminConstants.GUILD_MEMBER_ANALYSIS_COMMAND_NAME:

                try
                {
                    var member = command.Data.Options.FirstOrDefault(option => option.Name == "member").Value as SocketUser;

                    var analysisResult = await mediator.Send(
                        new AnalyzeGuildMemberCommand()
                        {
                            GuildMemberDiscordId = member.Id
                        },
                        token);

                    await messagesManager.SendAnalyzeResultMessageAsync(command, analysisResult.Value);

                    break;
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError(
                        ex,
                        "Ошибка при попытке анализа участника с DiscordId {GuildMemberDiscordId}",
                        command.User.Id);

                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Ошибка при попытке обработать команду {CommandName} настройки личной голосовой комнаты",
                        MlkAdminConstants.GUILD_MEMBER_ANALYSIS_COMMAND_NAME);

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