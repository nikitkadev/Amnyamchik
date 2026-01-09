using MlkAdmin._3_Infrastructure.Providers.Models.Guild;

namespace MlkAdmin._3_Infrastructure.Providers.Interfaces.Configuration.Guild;
public interface IJsonGuildChannelsProvider 
{
    VoiceChannels VoiceChannels { get; }
    TextChannels TextChannels { get; }
}