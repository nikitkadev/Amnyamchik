using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MlkAdmin._1_Domain.Interfaces;
using MlkAdmin._2_Application.Events.ButtonExecuted;
using MlkAdmin._2_Application.Events.GuildAvailable;
using MlkAdmin._2_Application.Events.MessageReceived;
using MlkAdmin._2_Application.Events.ModalSubmitted;
using MlkAdmin._2_Application.Events.ReactionAdded;
using MlkAdmin._2_Application.Events.Ready;
using MlkAdmin._2_Application.Events.SelectMenuExecuted;
using MlkAdmin._2_Application.Events.SlashCommandExecuted;
using MlkAdmin._2_Application.Events.UserJoined;
using MlkAdmin._2_Application.Events.UserLeft;
using MlkAdmin._2_Application.Events.UserUpdated;
using MlkAdmin._2_Application.Events.UserVoiceStateUpdated;
using MlkAdmin._2_Application.Implementations.Managers;
using MlkAdmin._2_Application.Implementations.Services;
using MlkAdmin._2_Application.Interfaces.Builders;
using MlkAdmin._2_Application.Interfaces.Managers;
using MlkAdmin._2_Application.Interfaces.Services;
using MlkAdmin._2_Application.Managers.Channels.VoiceChannels;
using MlkAdmin._2_Application.Managers.Messages;
using MlkAdmin._2_Application.Managers.Users;
using MlkAdmin._3_Infrastructure.Cache;
using MlkAdmin._3_Infrastructure.DataBase.EF;
using MlkAdmin._3_Infrastructure.Implementations.Builders;
using MlkAdmin._3_Infrastructure.Implementations.Repositiory;
using MlkAdmin._3_Infrastructure.Implementations.Services;
using MlkAdmin._3_Infrastructure.Interfaces;
using MlkAdmin._3_Infrastructure.Providers.Implementations.Configuration.App;
using MlkAdmin._3_Infrastructure.Providers.Implementations.Configuration.Guild;
using MlkAdmin._3_Infrastructure.Providers.Implementations.Configuration.Messages;
using MlkAdmin._3_Infrastructure.Providers.Implementations.Hubs;
using MlkAdmin._3_Infrastructure.Providers.Implementations.Hubs.Messages;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.App;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Messages;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Hubs;
using MlkAdmin._3_Infrastructure.Services;
using MlkAdmin._4_Presentation.Discord;
using MlkAdmin._4_Presentation.Interfaces;
using MlkAdmin.Presentation.DiscordListeners;
using MlkAdmin.Presentation.PresentationServices;
using MlkAdmin.Shared.JsonProviders;
using OpenAI.Chat;

namespace MlkAdmin.Presentation.DI;

public static class GuildBotDIRegistrator
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IGuildMemberMetricRepository, GuildMemberMetricRepository>();
        services.AddScoped<IGuildChannelsRepository, GuildChannelsRepository>();
        services.AddScoped<IGuildMembersRepository, GuildMembersRepository>();
        services.AddScoped<IGuildMessagesRepository, GuildMessagesRepository>();
        services.AddScoped<IGuildVoiceSessionRepository, GuildVoiceSessionRepository>();

        return services;
    }
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IDiscordEmbedBuilder, DiscordEmbedBuilder>();
        services.AddSingleton<IDiscordMessageComponentsBuilder, DiscordMessageComponentsBuilder>();
        services.AddScoped<IGuildMessagesManager, GuildMessagesManager>();
        services.AddScoped<IGuildMembersManager, GuildMembersManager >();
        services.AddScoped<IGuildChannelsService, GuildChannelsService>();
        services.AddScoped<IGuildInitializationService, GuildInitializationService>();
        services.AddScoped<IGuildMessagesService, GuildMessagesService>();
        services.AddSingleton<IGuildRolesService, GuildRolesService>();
        services.AddSingleton<IGuildVoiceSessionCacheService, GuildVoiceSessionCacheService>();
        services.AddScoped<IGuildVoiceSessionsManager, GuildVoiceSessionsManager>();

        return services;
    }
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDiscordService, DiscordService>();
        services.AddSingleton<IChatGPTService, ChatGPTService>();

        services.AddSingleton<IJsonProvidersHub, JsonProvidersHub>();

        services.AddSingleton<IJsonGuildConfigProvidersHub, JsonGuildConfigProvidersHub>();
        services.AddSingleton<IMlkAdminAppConfigProvidersHub, MlkAdminAppConfigProvidersHub>();
        services.AddSingleton<IJsonMessageProvidersHub, JsonMessageProvidersHub>();

        services.AddJsonProvider<IJsonMlkAdminAppConfigurationProvider, JsonMlkAdminAppConfigurationProvider>("../../../3_Infrastructure/Providers/Data/App/MlkAdminAppConfiguration.json");

        services.AddJsonProvider<IJsonGuildCategoriesProvider, JsonGuildCategoriesProvider>("../../../3_Infrastructure/Providers/Data/Guild/GuildCategories.json");
        services.AddJsonProvider<IJsonGuildChannelsProvider, JsonGuildChannelsProvider>("../../../3_Infrastructure/Providers/Data/Guild/GuildChannels.json");
        services.AddJsonProvider<IJsonGuildConfigurationProvider, JsonGuildConfigurationProvider>("../../../3_Infrastructure/Providers/Data/Guild/GuildConfiguration.json");
        services.AddJsonProvider<IJsonGuildRolesProvider, JsonGuildRolesProvider>("../../../3_Infrastructure/Providers/Data/Guild/GuildRoles.json");
        
        services.AddJsonProvider<IWelcomeMessageProvider, JsonWelcomeMessageProvider>("../../../3_Infrastructure/Providers/Data/Messages/Welcome.json");
        services.AddJsonProvider<IColorMessageProvider, JsonColorsMessageProvider>("../../../3_Infrastructure/Providers/Data/Messages/Colors.json");
        services.AddJsonProvider<IRulesMessageProvider, JsonRulesMessageProvider>("../../../3_Infrastructure/Providers/Data/Messages/Rules.json");
        services.AddJsonProvider<IHubMessageProvider, JsonHubMessageProvider>("../../../3_Infrastructure/Providers/Data/Messages/Hub.json");

        services.AddDbContext<MlkAdminDbContext>(
            (sp, options) =>
            {
                var config = sp.GetRequiredService<IJsonMlkAdminAppConfigurationProvider>();
                options.UseNpgsql(config.ConnectionString);
            }
        );

        return services;
    }
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssemblies(
                typeof(GuildBotStartup).Assembly,
                typeof(UserJoinedHandler).Assembly,
                typeof(UserLeftHandler).Assembly,
                typeof(ModalSubmittedHandler).Assembly,
                typeof(ButtonExecutedHandler).Assembly,
                typeof(GuildAvailableHandler).Assembly,
                typeof(UserVoiceStateUpdatedHandler).Assembly,
                typeof(SelectMenuExecutedHandler).Assembly,
                typeof(ReadyHandler).Assembly,
                typeof(MessageReceivedHandler).Assembly,
                typeof(ReactionAddedHandler).Assembly,
                typeof(GuildMemberUpdated).Assembly,
                typeof(SlashCommandExecutedHandler).Assembly,
                typeof(SlashCommandCountHandler).Assembly,
                typeof(VoiceSessionTimeHandler).Assembly
            )
        );

        services.AddHostedService<GuildBotHostedService>();
        services.AddSingleton<IDiscordEventsService, DiscordEventsService>();
        services.AddSingleton<IDiscordSlashCommandsService, DiscordSlashCommandsService>();
        
        services.AddSingleton(
            new DiscordSocketClient(
                    new DiscordSocketConfig() 
                    { 
                        GatewayIntents = GatewayIntents.All
                    }
                )
            );

        services.AddSingleton<ChatClient>(
            service =>
            {
                var apiKey = service.GetRequiredService<IJsonMlkAdminAppConfigurationProvider>().OpenAIApiKey;
                var model = "gpt-5-mini";

                return new ChatClient(model, apiKey);
            });

        return services;
    }
}
