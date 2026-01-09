using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.App;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Hubs;

namespace MlkAdmin._3_Infrastructure.Providers.Implementations.Hubs
{
    public class MlkAdminAppConfigProvidersHub(
        IJsonMlkAdminAppConfigurationProvider mlkAdminAppConfigurationProvider) : IMlkAdminAppConfigProvidersHub
    {
        public IJsonMlkAdminAppConfigurationProvider MlkAdminAppConfig => mlkAdminAppConfigurationProvider;
    }
}
