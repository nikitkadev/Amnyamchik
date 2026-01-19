using Microsoft.Extensions.Logging;
using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.App;
using Amnyam._3_Infrastructure.Providers.Models.App;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._3_Infrastructure.Providers.Implementations.Configuration.App;

public class JsonMlkAdminAppConfigurationProvider(string path, ILogger<JsonMlkAdminAppConfigurationProvider> logger) : JsonProviderBase<RootMlkAdminAppConfigurationModel>(path, logger), IJsonMlkAdminAppConfigurationProvider
{
    public string MalenkieApiKey => GetConfig().InfrastructureConfig.DiscordApiKey;
    public string OpenAIApiKey => GetConfig().InfrastructureConfig.OpenAIApiKey;
    public string ConnectionString => GetConfig().InfrastructureConfig.ConnectionString;

    public BotDetails BotDetails => GetConfig().BotDetails;
}