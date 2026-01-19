using Discord;
using MediatR;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Amnyam._2_Application.Events.UserJoined;
using Amnyam._2_Application.Events.UserLeft;
using Amnyam._2_Application.Events.ModalSubmitted;
using Amnyam._2_Application.Events.ButtonExecuted;
using Amnyam._2_Application.Events.GuildAvailable;
using Amnyam._2_Application.Events.SelectMenuExecuted;
using Amnyam._2_Application.Events.UserVoiceStateUpdated;
using Amnyam._2_Application.Events.Ready;
using Amnyam._2_Application.Events.MessageReceived;
using Amnyam._2_Application.Events.ReactionAdded;
using Amnyam._2_Application.Events.UserUpdated;
using Amnyam._2_Application.Events.SlashCommandExecuted;
using Amnyam._3_Infrastructure.Interfaces;
using Amnyam.Shared.Results;
using Amnyam._4_Presentation.Discord;

namespace Amnyam.Presentation.DiscordListeners;

public class DiscordEventsService(
    ILogger<DiscordEventsService> logger,
    IDiscordService discordService, 
    IMediator mediator) : IDiscordEventsService
{
    public void SubscribeOnEvents()
    {
        discordService.DiscordClient.UserJoined += OnUserJoined;
        discordService.DiscordClient.UserLeft += OnUserLeft;
        discordService.DiscordClient.ModalSubmitted += OnModalSubmitted;
        discordService.DiscordClient.ButtonExecuted += OnButtonExecuted;
        discordService.DiscordClient.GuildAvailable += OnGuildAvailable;
        discordService.DiscordClient.UserVoiceStateUpdated += OnUserVoiceStateUpdated;
        discordService.DiscordClient.SelectMenuExecuted += OnSelectMenuExecuted;
        discordService.DiscordClient.Ready += OnReady;
        discordService.DiscordClient.MessageReceived += OnMessageReceived;
        discordService.DiscordClient.ReactionAdded += OnReactionAdded;
        discordService.DiscordClient.GuildMemberUpdated += GuildMemberUpdated;
        discordService.DiscordClient.SlashCommandExecuted += OnSlashCommandExecuted;
        
        logger.LogInformation(
                "Подписывание на события прошло успешно");
    }

    public void UnsubscribeOnEvents()
    {
        discordService.DiscordClient.UserJoined -= OnUserJoined;
        discordService.DiscordClient.UserLeft -= OnUserLeft;
        discordService.DiscordClient.ModalSubmitted -= OnModalSubmitted;
        discordService.DiscordClient.ButtonExecuted -= OnButtonExecuted;
        discordService.DiscordClient.GuildAvailable -= OnGuildAvailable;
        discordService.DiscordClient.UserVoiceStateUpdated -= OnUserVoiceStateUpdated;
        discordService.DiscordClient.SelectMenuExecuted -= OnSelectMenuExecuted;
        discordService.DiscordClient.Ready -= OnReady;
        discordService.DiscordClient.MessageReceived -= OnMessageReceived;
        discordService.DiscordClient.ReactionAdded -= OnReactionAdded;
        discordService.DiscordClient.GuildMemberUpdated -= GuildMemberUpdated;
        discordService.DiscordClient.SlashCommandExecuted -= OnSlashCommandExecuted;

        logger.LogInformation(
            "Отписывание от событий прошло успешно");
    }

    private async Task OnUserJoined(SocketGuildUser socketGuildUser)
    {
        await mediator.Publish(new UserJoined(socketGuildUser));
    }

    private async Task OnMessageReceived(SocketMessage socketMessage)
    {
        await mediator.Publish(new MessageReceived(socketMessage));
    }

    private async Task OnUserLeft(SocketGuild socketGuild, SocketUser socketUser)
    {
        await mediator.Publish(new UserLeft(socketGuild, socketUser));
    }

    private async Task OnModalSubmitted(SocketModal socketModal)
    {
        await mediator.Publish(new ModalSubmitted(socketModal));
    }

    private async Task OnButtonExecuted(SocketMessageComponent socketMessageComponent)
    {
        await mediator.Publish(new ButtonExecuted(socketMessageComponent));
    }

    private async Task OnGuildAvailable(SocketGuild socketGuild)
    {
        await mediator.Publish(new GuildAvailable(socketGuild));
    }

    private async Task OnUserVoiceStateUpdated(SocketUser socketUser, SocketVoiceState oldState, SocketVoiceState newState)
    {

        await mediator.Publish(new UserVoiceStateUpdated(socketUser, oldState, newState));
    }
   
    private async Task OnSelectMenuExecuted(SocketMessageComponent socketMessageComponent)
    {

        await mediator.Publish(new SelectMenuExecuted(socketMessageComponent));
    }

    private async Task OnReady()
    {

        await mediator.Publish(new Ready());
    }

    private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        await mediator.Publish(new ReactionAdded(message, channel, reaction));
    }

    private async Task GuildMemberUpdated(Cacheable<SocketGuildUser, ulong> oldUserState, SocketGuildUser newUserState)
    {
        await mediator.Publish(new GuildMemberUpdated(oldUserState, newUserState));
    }

    private async Task OnSlashCommandExecuted(SocketSlashCommand socketSlashCommand)
    {
        await mediator.Publish(new SlashCommandExecuted(socketSlashCommand));
    }
}
