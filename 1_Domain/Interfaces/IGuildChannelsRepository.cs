using Amnyam._1_Domain.Entities;

namespace Amnyam._1_Domain.Interfaces;

public interface IGuildChannelsRepository
{
    Task SyncDbVoiceChannelsWithGuildAsync(CancellationToken token = default);

    Task UpsertDbTextChannelAsync(GuildTextChannel channel);
    Task UpsertDbVoiceChannelAsync(GuildVoiceChannel channel);

    Task RemoveDbTextChannelAsync(ulong id);
    Task RemoveDbVoiceChannelAsync(ulong id);

    Task<bool> IsTemporaryVoiceChannel(ulong id, CancellationToken token = default);
    Task<bool> IsGeneratingVoiceChannel(ulong id, CancellationToken token = default);
}
