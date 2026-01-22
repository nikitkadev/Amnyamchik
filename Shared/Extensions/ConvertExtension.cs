using Discord.WebSocket;

namespace Amnyam.Shared.Extensions;

public static class ConvertExtension
{
    public static T? GetOption<T>(
        this IReadOnlyCollection<SocketSlashCommandDataOption> options,
        string name)
    {
        var value = options.FirstOrDefault(x => x.Name == name)?.Value;

        if (value is null)
            return default;

        var targetType = typeof(T);

        var underlyingType = Nullable.GetUnderlyingType(targetType);
        if (underlyingType != null)
        {
            return (T)Convert.ChangeType(value, underlyingType);
        }

        return (T)Convert.ChangeType(value, targetType);
    }
}