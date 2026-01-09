using Microsoft.Extensions.Logging;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
using MlkAdmin._3_Infrastructure.Providers.Models.Guild;
using MlkAdmin.Shared.JsonProviders;

namespace MlkAdmin._3_Infrastructure.Providers.Implementations.Configuration.Guild;

public class JsonGuildConfigurationProvider(string path, ILogger<JsonGuildConfigurationProvider> logger) : JsonProviderBase<RootGuildConfigurationModel>(path, logger), IJsonGuildConfigurationProvider
{
    public GuildDetails GuildDetails => GetConfig().GuildDetails;
    public Founder Founder => GetConfig().Founder;
    public DynamicMessages DynamicMessages => GetConfig().DynamicMessages;
}
