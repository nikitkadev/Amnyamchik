using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Amnyam._3_Infrastructure.Interfaces;
using Amnyam._4_Presentation.Discord;
using Amnyam.Shared.JsonProviders;

namespace Amnyam.Presentation.PresentationServices;

public class GuildBotHostedService(
    IServiceScopeFactory scopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken token)
    {
        using var scope = scopeFactory.CreateScope();

        var eventsService = scope.ServiceProvider
            .GetRequiredService<IDiscordEventsService>();

        var providersHub = scope.ServiceProvider
            .GetRequiredService<IJsonProvidersHub>();

        var discordService = scope.ServiceProvider
            .GetRequiredService<IDiscordService>();

        eventsService.SubscribeOnEvents();

        await discordService.DiscordClient.LoginAsync(
            TokenType.Bot,
            providersHub.AppConfigProvidersHub.MalenkieApiKey);

        await discordService.DiscordClient.StartAsync();
    }

    public async Task StopAsync(CancellationToken token)
    {
        using var scope = scopeFactory.CreateScope();

        var eventsService = scope.ServiceProvider
            .GetRequiredService<IDiscordEventsService>();

        var discordService = scope.ServiceProvider
            .GetRequiredService<IDiscordService>();

        eventsService.UnsubscribeOnEvents();
        await discordService.DiscordClient.StopAsync();
    }
}