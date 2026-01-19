namespace Amnyam._3_Infrastructure.Providers.Models.Guild;

public class RootCategoriesModel
{
    public ConfigCategory Admin { get; set; } = new();
    public ConfigCategory Server { get; set; } = new();
    public ConfigCategory Base { get; set; } = new();
    public ConfigCategory Game { get; set; } = new();
    public ConfigCategory Lobby { get; set; } = new();
    public ConfigCategory Rest { get; set; } = new();
}

public class ConfigCategory
{
    public ulong DiscordId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}