using Microsoft.Extensions.Logging;
using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
using Amnyam._3_Infrastructure.Providers.Models.Guild;
using Amnyam.Shared.JsonProviders;

namespace Amnyam._3_Infrastructure.Providers.Implementations.Configuration.Guild;

public class JsonGuildChannelsProvider(string path, ILogger<JsonGuildChannelsProvider> logger) : JsonProviderBase<RootChannelsModel>(path, logger), IJsonGuildChannelsProvider
{
    public VoiceChannels VoiceChannels => GetConfig().VoiceChannels;

    public TextChannels TextChannels => GetConfig().TextChannels;
}
