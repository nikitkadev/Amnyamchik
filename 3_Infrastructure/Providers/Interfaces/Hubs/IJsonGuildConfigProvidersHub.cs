using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.App;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Guild;

namespace MlkAdmin._3_Infrastructure.Providers.Interfaces.Hubs;

public interface IJsonGuildConfigProvidersHub
{
    IJsonGuildCategoriesProvider Categories { get; }
    IJsonGuildChannelsProvider Channels { get; }
    IJsonGuildConfigurationProvider GuildConfig { get; }
    IJsonGuildRolesProvider Roles { get; }
}
