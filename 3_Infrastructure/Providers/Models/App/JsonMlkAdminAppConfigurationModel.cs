namespace Amnyam._3_Infrastructure.Providers.Models.App;

public class RootMlkAdminAppConfigurationModel
{
    public InfrastructureConfig InfrastructureConfig { get; set; } = new();
    public BotDetails BotDetails { get; set; } = new();
}

public class InfrastructureConfig
{
    public string DiscordApiKey { get; set; } = string.Empty;
    public string OpenAIApiKey { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}

public class BotDetails
{
    public Developer Developer { get; set; } = new();
    public string Version { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class Developer
{
    public string Nickname { get; set; } = string.Empty;
    public string IconLink { get; set; } = string.Empty;
    public string GitHub { get; set; } = string.Empty;
    public string Discord { get; set; } = string.Empty;
}