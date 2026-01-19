using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.App;
using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Guild;

namespace Amnyam._3_Infrastructure.Providers.Interfaces.Hubs;

public interface IJsonGuildConfigProvidersHub
{
    IJsonGuildCategoriesProvider Categories { get; }
    IJsonGuildChannelsProvider Channels { get; }
    IJsonGuildConfigurationProvider GuildConfig { get; }
    IJsonGuildRolesProvider Roles { get; }
}
