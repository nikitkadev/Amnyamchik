using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.App;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Hubs;

namespace MlkAdmin.Shared.JsonProviders;

public class JsonProvidersHub(
    IJsonGuildConfigProvidersHub guildConfigProvidersHub,
    IJsonMessageProvidersHub messageProvidersHub,
    IJsonMlkAdminAppConfigurationProvider mlkAdminAppConfigurationProvider) : IJsonProvidersHub
{
    public IJsonGuildConfigProvidersHub GuildConfigProvidersHub => guildConfigProvidersHub;
    public IJsonMessageProvidersHub MessageProvidersHub => messageProvidersHub;
    public IJsonMlkAdminAppConfigurationProvider AppConfigProvidersHub => mlkAdminAppConfigurationProvider;
}
