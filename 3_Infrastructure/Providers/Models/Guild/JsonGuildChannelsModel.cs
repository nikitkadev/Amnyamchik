namespace Amnyam._3_Infrastructure.Providers.Models.Guild;

public class RootChannelsModel
{
    public VoiceChannels VoiceChannels { get; set; } = new();
    public TextChannels TextChannels { get; set; } = new();
}

public class VoiceChannels
{
    public ConfigVoiceChannels AdminVoice { get; set; } = new();
    public ConfigVoiceChannels GeneralVoice { get; set; } = new();
    public ConfigVoiceChannels GeneratingVoice { get; set; } = new();
    public ConfigVoiceChannels RestVoice { get; set; } = new();
}
public class TextChannels
{
    public ConfigTextChannels TestingText { get; set; } = new();
    public ConfigTextChannels AdminText { get; set; } = new();
    public ConfigTextChannels LogsText { get; set; } = new();
    public ConfigTextChannels WelcomeText { get; set; } = new();
    public ConfigTextChannels HubText { get; set; } = new();
    public ConfigTextChannels NewsText { get; set; } = new();
    public ConfigTextChannels BotCommandsText { get; set; } = new();
    public ConfigTextChannels GeneralText { get; set; } = new();
    public ConfigTextChannels HighlightText { get; set; } = new();
    public ConfigTextChannels OwesSilverText { get; set; } = new();
    public ConfigTextChannels GamingText { get; set; } = new();
    public ConfigTextChannels Arts { get; set; } = new();
}

public class ConfigVoiceChannels
{
    public string Name { get; set; } = string.Empty;
    public ulong DiscordId { get; set; } = 0;
    public string Https { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsGenerating { get; set; } = false;
}
public class ConfigTextChannels
{
    public string Name { get; set; } = string.Empty;
    public ulong DiscordId { get; set; } = 0;
    public string Https { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}