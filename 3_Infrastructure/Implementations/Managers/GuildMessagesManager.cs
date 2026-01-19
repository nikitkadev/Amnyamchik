using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Amnyam._1_Domain.Exceptions;
using Amnyam._2_Application.Interfaces.Builders;
using Amnyam._2_Application.Interfaces.Managers;
using Amnyam._2_Application.Interfaces.Services;
using Amnyam._3_Infrastructure.Interfaces;
using Amnyam.Shared.Constants;
using Amnyam.Shared.Dtos;
using Amnyam.Shared.Extensions;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._2_Application.Managers.Messages;

public class GuildMessagesManager(
    ILogger<GuildMessagesManager> logger,
    IJsonProvidersHub providersHub,
    IGuildChannelsService channelsService,
    IDiscordMessageComponentsBuilder componentsBuilder,
    IDiscordService discordService) : IGuildMessagesManager
{
    public async Task RefreshDynamicMessagesAsync()
    {
        await SendMessageHubAsync();
    }

    private async Task SendOrUpdateDynamicMessageAsync(ulong targetMessageDiscordId, MessageComponent messageComponent)
    {
        try
        {
            var socketGuildChannel = await channelsService.GetGuildChannelByDiscordIdAsync(providersHub.GuildConfigProvidersHub.Channels.TextChannels.HubText.DiscordId);

            if (socketGuildChannel is not SocketTextChannel socketTextChannel)
            {
                logger.LogWarning(
                    "Канал с DiscordId {ChannelId} не является текстовым",
                    providersHub.GuildConfigProvidersHub.Channels.TextChannels.HubText.DiscordId);

                return;
            }

            if (await socketTextChannel.GetMessageAsync(targetMessageDiscordId) is IUserMessage sentMessage)
            {
                await sentMessage.ModifyAsync(
                    async message =>
                    {
                        message.Components = messageComponent;
                    }
                );
            }
            else
            {
                await socketTextChannel.SendMessageAsync(
                    components: messageComponent
                    );
            }
        }
        catch (GuildChannelNotFoundException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке оправить или обновить сообщение {DynamicMessage}",
                targetMessageDiscordId);

        }
    }
    private async Task SendMessageHubAsync()
    {
        var messageContent = providersHub.MessageProvidersHub.Hub;

        await SendOrUpdateDynamicMessageAsync(
            providersHub.GuildConfigProvidersHub.GuildConfig.DynamicMessages.Hub.DiscordId,
            new ComponentBuilderV2()
               .WithContainer(container =>
               {
                   container
                   .WithTextDisplay(
                       text =>
                       {
                           text.WithContent(messageContent.Title);
                       })
                   .WithTextDisplay(
                       text =>
                       {
                           text.WithContent(messageContent.Heading.Replace("invite_link", providersHub.GuildConfigProvidersHub.GuildConfig.GuildDetails.InviteLink));
                       })
                   .WithTextDisplay(
                       text =>
                       {
                           text.WithContent(messageContent.Description);
                       })
                   .WithTextDisplay(
                       async text =>
                       {
                           text.WithContent(messageContent.Footer
                               .Replace(
                               "admin_replace", 
                               await discordService.GetGuildMemberMentionByIdAsync(
                                   providersHub.GuildConfigProvidersHub.GuildConfig.Founder.DiscordId)));
                       })
                   .WithSeparator()
                   .WithActionRow(
                       row =>
                       {
                           row.WithButton(
                               async button =>
                               {
                                   button
                                      .WithLabel("Авторизоваться")
                                      .WithStyle(ButtonStyle.Secondary)
                                      .WithEmote(discordService.GetGuildEmote(MlkAdminConstants.EMOJI_AU_BUTTON))
                                      .WithCustomId(MlkAdminConstants.BUTTONS_AUTHORIZATION_CUSTOM_ID);
                               });
                           row.WithButton(
                               button =>
                               {
                                   button
                                      .WithLabel("Правила")
                                      .WithStyle(ButtonStyle.Secondary)
                                      .WithEmote(discordService.GetGuildEmote(MlkAdminConstants.EMOJI_RULES_BUTTON))
                                      .WithCustomId(MlkAdminConstants.BUTTONS_RULES_CUSTOM_ID);
                               });
                           row.WithButton(
                               button =>
                               {
                                   button
                                      .WithLabel("Цвет имени")
                                      .WithStyle(ButtonStyle.Secondary)
                                      .WithEmote(discordService.GetGuildEmote(MlkAdminConstants.EMOJI_COLOR_BUTTON))
                                      .WithCustomId(MlkAdminConstants.BUTTONS_COLORS_CUSTOM_ID);
                               });
                       });
               })
               .Build());
    }

    public async Task SendDefaultResponseAsync(SocketMessageComponent messageAction, string clientMessage)
    {
        await messageAction.FollowupAsync(
            components:
            new ComponentBuilderV2()
            .WithContainer(
                container =>
                {
                    container.WithTextDisplay(
                        text =>
                        {
                            text.WithContent(clientMessage);
                        });
                })
            .Build()
        , ephemeral: true);
    }
    public async Task SendDefaultResponseAsync(SocketSlashCommand messageAction, string clientMessage)
    {
        await messageAction.FollowupAsync(
            components:
            new ComponentBuilderV2()
            .WithContainer(
                container =>
                {
                    container.WithTextDisplay(
                        text =>
                        {
                            text.WithContent(clientMessage);
                        });
                })
            .Build()
        , ephemeral: true);
    }
    public async Task SendRulesMessageResponseAsync(SocketMessageComponent messageAction)
    {
        var emote = discordService.GetGuildEmote(MlkAdminConstants.EMOJI_DOT_MARK_NAME);
        var messageContent = providersHub.MessageProvidersHub.Rules;
        var messageDescription = string.Join(
            "\n", messageContent.ContentList.Select(rule => $"{emote}{rule}"));

        await messageAction.FollowupAsync(
            components: new ComponentBuilderV2()
            .WithContainer(
                container =>
                {
                    container
                    .WithTextDisplay(
                        text =>
                        {
                            text.WithContent(messageContent.Title);
                        })
                    .WithTextDisplay(
                        text =>
                        {
                            text.WithContent(messageDescription);
                        });
                })
            .Build(), 
            ephemeral: true);
    }
    public async Task SendColorsMenuResponseAsync(SocketMessageComponent messageAction)
    {
        var emote = discordService.GetGuildEmote(MlkAdminConstants.EMOJI_DOT_MARK_NAME);
        var messageContent = providersHub.MessageProvidersHub.Color;
        var messageDescription = messageContent.Description;
        var messageList = string.Join($"{emote}", messageContent.ContentList);

        var menu = (
            await componentsBuilder.BuildSelectionMenuAsync(
                new SelectionMenuConfigDto()
                {
                    Placeholder = "Выбрать цвет...",
                    CustomId = "GUILD_NAMECOLOR_CHANGE",
                    Options = {
                        new SelectOptionConfigDto() { Label = "Удалить все цвета", Value = "remove_color"},
                        new SelectOptionConfigDto() { Label = "💜", Value = "VioletColor"},
                        new SelectOptionConfigDto() { Label = "💙", Value = "SlateblueColor" },
                        new SelectOptionConfigDto() { Label = "🧡", Value = "CoralColor" },
                        new SelectOptionConfigDto() { Label = "💛", Value = "KhakiColor" },
                        new SelectOptionConfigDto() { Label = "💖", Value = "CrimsonColor" },
                        new SelectOptionConfigDto() { Label = "💚", Value = "LimeColor" }}
                }))
                .Value;

        var messageComponent = new ComponentBuilderV2()
        .WithContainer(
            container =>
            {
                container
                .WithTextDisplay(
                    text =>
                    {
                        text.WithContent(messageContent.Title);
                    })
                .WithTextDisplay(
                    text =>
                    {
                        text.WithContent(messageDescription);

                    })
                .WithTextDisplay(
                    text =>
                    {
                        text.WithContent(messageList);

                    })
                .WithSeparator()
                .WithActionRow(
                    row =>
                    {
                        row.WithSelectMenu(menu);
                    });
            })
        .Build();

        await messageAction.FollowupAsync(
            components: messageComponent,
            ephemeral: true);
    }
    public async Task SendWelcomeMessageAsync(ulong newMemberDiscordId)
    {
        var messageContent = providersHub.MessageProvidersHub.Welcome;
        var guildTextChannel = await channelsService.GetGuildChannelByDiscordIdAsync(providersHub.GuildConfigProvidersHub.Channels.TextChannels.WelcomeText.DiscordId) as SocketTextChannel;
        var messageComponent = new ComponentBuilderV2()
            .WithContainer(
                container =>
                {
                    container.WithTextDisplay(
                        text =>
                        {
                            text.WithContent(messageContent.Title);
                        })
                    .WithTextDisplay(
                        async text =>
                        {
                            text.WithContent(messageContent.Heading.Replace("new_member_mention", await discordService.GetGuildMemberMentionByIdAsync(newMemberDiscordId)));
                        })
                    .WithTextDisplay(
                        text =>
                        {
                            text.WithContent(messageContent.Description
                                .Replace(
                                    "server_hub_discordid", 
                                    providersHub.GuildConfigProvidersHub.Channels.TextChannels.HubText.DiscordId.ToString()));
                        })
                    .WithSeparator()
                    .WithActionRow(
                        row =>
                        {
                            row.WithButton(
                                button =>
                                {
                                    button.WithLabel("Прыгнуть в хаб")
                                        .WithStyle(ButtonStyle.Link)
                                        .WithEmote(discordService.GetGuildEmote(MlkAdminConstants.EMOJI_SERVERHUB_LINK_BUTTON))
                                        .WithUrl(providersHub.GuildConfigProvidersHub.Channels.TextChannels.HubText.Https);
                                });
                            row.WithButton(
                                button =>
                                {
                                    button.WithLabel("Узнать тотемное животное")
                                       .WithStyle(ButtonStyle.Secondary)
                                       .WithEmote(discordService.GetGuildEmote(MlkAdminConstants.EMOJI_TOTEMANIMAL_BUTTON))
                                       .WithCustomId(MlkAdminConstants.BUTTONS_TOTEMANIMAL_CUSTOM_ID);
                                });
                        });
                })
            .Build();

        await guildTextChannel.SendMessageAsync(
            components: messageComponent);
    }
    public async Task SendLogMessageAsync(LogMessageDto logMessageDto)
    {
        var guildTextChannel = await channelsService.GetGuildChannelByDiscordIdAsync(providersHub.GuildConfigProvidersHub.Channels.TextChannels.LogsText.DiscordId) as SocketTextChannel
            ?? throw new GuildChannelNotFoundException(providersHub.GuildConfigProvidersHub.Channels.TextChannels.LogsText.DiscordId);

        var messageComponent = new ComponentBuilderV2()
            .WithContainer(
                container =>
                {
                    container.WithTextDisplay(
                        text =>
                        {
                            text.WithContent(logMessageDto.Title);
                        })
                    .WithTextDisplay(
                        text =>
                        {
                            text.WithContent(logMessageDto.Message);
                        })
                    .WithTextDisplay(
                        text =>
                        {
                            text.WithContent($"Время отправки: {logMessageDto.Created} UTC");
                        });
                })
            .Build();

        await guildTextChannel.SendMessageAsync(
            components: messageComponent);
    }
    public async Task SendAnalyzeResultMessageAsync(SocketSlashCommand messageAction, GuildMemberAnalysisResultData data)
    {
        if (data is null)
            return;

        var component = new ComponentBuilderV2()
            .WithContainer(
                container =>
                {
                    container.WithTextDisplay(
                        async text =>
                        {
                            text.WithContent($"Анализ участника {await discordService.GetGuildMemberMentionByIdAsync(data.GuildMemberDiscordId)}");
                        });

                    container.WithTextDisplay(
                        async text =>
                        {
                            text.WithContent($"Дата вступления: {data.JoinedAt}\n" +
                                $"Количество дней на сервере: {data.DaysSinceJoined}\n" +
                                $"Дата первого сообщения: {data.FirstMessageDate:g}\n" +
                                $"Дата крайнего сообщения: {data.LastMessageDate:g}");
                        });

                    container.WithTextDisplay(
                        text =>
                        {
                            text.WithContent($"Метрики");
                        });

                    container.WithTextDisplay(
                        text =>
                        {
                            text.WithContent($"Отправлено сообщений: {data.MessageCount}\n" +
                                $"Реакций добавлено: {data.ReactionCount}\n" +
                                $"Отправлено картинок: {data.PicturesSentCount}\n" +
                                $"Отправлено гифов: {data.GifsSentCount}\n" +
                                $"Вызвано слеш-команд: {data.CommandsSentCount}\n" +
                                $"Времени в голосовых каналах: {TimeExtension.ConvertTimeFromSecond(data.VoiceChannelsTimeSpent)}\n");
                        });

                    container.WithTextDisplay(
                        text =>
                        {
                            text.WithContent("AI - анализ последней активности");
                        });

                    container.WithTextDisplay(
                        text =>
                        {
                            text.WithContent($"Средний процент токсичности: {data.AvgToxicity}\n" +
                                $"Самое токсичное сообщение: {data.MostToxicMessage}\n" +
                                $"Стиль речи: {data.SpeechStyle}\n" +
                                $"Тональность речи: {data.Tonality}\n" +
                                $"Средняя длина сообщений: {data.AvgCharsInMessage} символов / сообщение");
                        });

                    container.WithSeparator();

                    container.WithActionRow(
                        row =>
                        {
                            row.WithButton(
                                button =>
                                {
                                    button.WithLabel("Расширенный анализ");
                                    button.WithStyle(ButtonStyle.Secondary);
                                    button.WithCustomId(MlkAdminConstants.BUTTONS_ADVANCED_ANALYSIS_CUSTOM_ID);
                                });
                        });
                })
            .Build();

        await messageAction.FollowupAsync(components: component);
    }
}