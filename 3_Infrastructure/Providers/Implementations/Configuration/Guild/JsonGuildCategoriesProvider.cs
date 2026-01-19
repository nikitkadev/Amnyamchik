using Microsoft.Extensions.Logging;
using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
using Amnyam._3_Infrastructure.Providers.Models.Guild;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._3_Infrastructure.Providers.Implementations.Configuration.Guild;

public class JsonGuildCategoriesProvider(
    string path,
    ILogger logger) : JsonProviderBase<RootCategoriesModel>(path, logger), IJsonGuildCategoriesProvider
{
    public ConfigCategory Admin => GetConfig().Admin;
    public ConfigCategory Server => GetConfig().Server;
    public ConfigCategory Base => GetConfig().Base;
    public ConfigCategory Game => GetConfig().Game;
    public ConfigCategory Lobby => GetConfig().Lobby;
    public ConfigCategory Rest => GetConfig().Rest;
}