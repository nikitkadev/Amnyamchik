using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.App;

namespace Amnyam._3_Infrastructure.Providers.Interfaces.Hubs;

public interface IMlkAdminAppConfigProvidersHub
{
    IJsonMlkAdminAppConfigurationProvider MlkAdminAppConfig { get; }

}
