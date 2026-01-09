using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Messages;

namespace MlkAdmin._3_Infrastructure.Providers.Interfaces.Hubs;

public interface IJsonMessageProvidersHub
{
    IWelcomeMessageProvider Welcome { get; }
    IHubMessageProvider Hub { get; }
    IRulesMessageProvider Rules { get; }
    IColorMessageProvider Color { get; }
}
