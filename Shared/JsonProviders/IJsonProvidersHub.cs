using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.App;
using Amnyam._3_Infrastructure.Providers.Interfaces.Hubs;


namespace Amnyam.Shared.JsonProviders;

public interface IJsonProvidersHub
{
    IJsonGuildConfigProvidersHub GuildConfigProvidersHub { get; }
    IJsonMessageProvidersHub MessageProvidersHub { get; }
    IJsonMlkAdminAppConfigurationProvider AppConfigProvidersHub { get; }
}
