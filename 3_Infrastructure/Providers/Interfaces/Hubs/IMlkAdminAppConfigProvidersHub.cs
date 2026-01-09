using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.App;

namespace MlkAdmin._3_Infrastructure.Providers.Interfaces.Hubs;

public interface IMlkAdminAppConfigProvidersHub
{
    IJsonMlkAdminAppConfigurationProvider MlkAdminAppConfig { get; }

}
