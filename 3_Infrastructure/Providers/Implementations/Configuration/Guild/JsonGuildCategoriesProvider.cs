using Microsoft.Extensions.Logging;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
using MlkAdmin._3_Infrastructure.Providers.Models.Guild;
using MlkAdmin.Shared.JsonProviders;

namespace MlkAdmin._3_Infrastructure.Providers.Implementations.Configuration.Guild;

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