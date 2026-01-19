using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Messages;
using Amnyam._3_Infrastructure.Providers.Interfaces.Hubs;

namespace Amnyam._3_Infrastructure.Providers.Implementations.Hubs.Messages;

public class JsonMessageProvidersHub(
    IWelcomeMessageProvider welcomeMessage,
    IHubMessageProvider hubMessage,
    IRulesMessageProvider rulesMessage,
    IColorMessageProvider colorMessage) : IJsonMessageProvidersHub
{
    public IWelcomeMessageProvider Welcome => welcomeMessage;
    public IHubMessageProvider Hub => hubMessage;
    public IRulesMessageProvider Rules => rulesMessage;
    public IColorMessageProvider Color => colorMessage;
}
