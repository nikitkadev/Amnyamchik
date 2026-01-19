using Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Messages;

namespace Amnyam._3_Infrastructure.Providers.Models.Guild;

public class RootGuildConfigurationModel
{
    public GuildDetails GuildDetails { get; set; } = new();
    public Founder Founder { get; set; } = new();
    public DynamicMessages DynamicMessages { get; set; } = new();
}

public class GuildDetails
{
    public string Name { get; set; } = string.Empty;
    public ulong DiscordId { get; set; } = 0;
    public string InviteLink { get; set; } = string.Empty;
}

public class Founder
{
    public string Name { get; set;  } = string.Empty;
    public string DiscordUserName { get; set;  } = string.Empty;
    public ulong DiscordId { get; set; } = 0;
}

public class DynamicMessages
{
    public Hub Hub { get; set; } = new();
}

public class Hub
{
    public ulong DiscordId { get; set; } = 0;
}