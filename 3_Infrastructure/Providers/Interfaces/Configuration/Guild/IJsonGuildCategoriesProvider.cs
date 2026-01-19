using Amnyam._3_Infrastructure.Providers.Models.Guild;

namespace Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Guild;

public interface IJsonGuildCategoriesProvider
{
    ConfigCategory Admin { get; }
    ConfigCategory Server { get; }
    ConfigCategory Base { get; }
    ConfigCategory Game { get; }
    ConfigCategory Lobby{ get; }
    ConfigCategory Rest{ get; }
}