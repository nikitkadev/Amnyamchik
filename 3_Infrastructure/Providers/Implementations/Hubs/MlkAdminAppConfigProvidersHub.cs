using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.App;
using Amnyam._3_Infrastructure.Providers.Interfaces.Hubs;

namespace Amnyam._3_Infrastructure.Providers.Implementations.Hubs
{
    public class MlkAdminAppConfigProvidersHub(
        IJsonMlkAdminAppConfigurationProvider mlkAdminAppConfigurationProvider) : IMlkAdminAppConfigProvidersHub
    {
        public IJsonMlkAdminAppConfigurationProvider MlkAdminAppConfig => mlkAdminAppConfigurationProvider;
    }
}
