namespace Amnyam.Shared.Extensions;

public static class TimeExtension
{
    public static string ConvertTimeFromSecond(long seconds)
    {
        var ts = TimeSpan.FromSeconds(seconds);
        return $"{ts.Days}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
    }
}
