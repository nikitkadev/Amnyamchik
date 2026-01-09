using Microsoft.Extensions.Logging;
using MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
using MlkAdmin._3_Infrastructure.Providers.Models.Guild;
using MlkAdmin.Shared.JsonProviders;

namespace MlkAdmin._3_Infrastructure.Providers.Implementations.Configuration.Guild;

public class JsonGuildChannelsProvider(string path, ILogger<JsonGuildChannelsProvider> logger) : JsonProviderBase<RootChannelsModel>(path, logger), IJsonGuildChannelsProvider
{
    public VoiceChannels VoiceChannels => GetConfig().VoiceChannels;

    public TextChannels TextChannels => GetConfig().TextChannels;
}
