using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.App;
using Amnyam._3_Infrastructure.Providers.Interfaces.Hubs;

namespace Amnyam.Shared.JsonProviders;

public class JsonProvidersHub(
    IJsonGuildConfigProvidersHub guildConfigProvidersHub,
    IJsonMessageProvidersHub messageProvidersHub,
    IJsonMlkAdminAppConfigurationProvider mlkAdminAppConfigurationProvider) : IJsonProvidersHub
{
    public IJsonGuildConfigProvidersHub GuildConfigProvidersHub => guildConfigProvidersHub;
    public IJsonMessageProvidersHub MessageProvidersHub => messageProvidersHub;
    public IJsonMlkAdminAppConfigurationProvider AppConfigProvidersHub => mlkAdminAppConfigurationProvider;
}
