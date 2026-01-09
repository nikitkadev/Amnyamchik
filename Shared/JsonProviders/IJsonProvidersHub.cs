using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.App;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Hubs;


namespace MlkAdmin.Shared.JsonProviders;

public interface IJsonProvidersHub
{
    IJsonGuildConfigProvidersHub GuildConfigProvidersHub { get; }
    IJsonMessageProvidersHub MessageProvidersHub { get; }
    IJsonMlkAdminAppConfigurationProvider AppConfigProvidersHub { get; }
}
