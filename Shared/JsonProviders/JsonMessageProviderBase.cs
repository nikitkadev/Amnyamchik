using Microsoft.Extensions.Logging;
using Amnyam._3_Infrastructure.Providers.Models.Messages;

namespace Amnyam.Shared.JsonProviders;

public abstract class JsonMessageProviderBase<T>(string path, ILogger<JsonMessageProviderBase<T>> logger) : JsonProviderBase<MessageBaseModel<T>>(path, logger), IJsonMessageContentProvider<T>
{
    public string Title => GetConfig().Title;
    public string Heading => GetConfig().Heading;
    public string Description => GetConfig().Description;
    public List<T> ContentList => GetConfig().ContentList;
    public string Footer => GetConfig().Footer;
}
