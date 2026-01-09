namespace MlkAdmin._2_Application.Interfaces.Services;

public interface IGuildInitializationService
{
    Task InitializeAsync(ulong guildId, CancellationToken token);
}
