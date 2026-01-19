using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Messages;

namespace Amnyam._3_Infrastructure.Providers.Interfaces.Hubs;

public interface IJsonMessageProvidersHub
{
    IWelcomeMessageProvider Welcome { get; }
    IHubMessageProvider Hub { get; }
    IRulesMessageProvider Rules { get; }
    IColorMessageProvider Color { get; }
}
