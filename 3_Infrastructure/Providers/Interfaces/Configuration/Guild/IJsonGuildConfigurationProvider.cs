using Amnyam._3_Infrastructure.Providers.Models.Guild;


namespace Amnyam._3_Infrastructure.Providers.Interfaces.Configuration.Guild;

public interface IJsonGuildConfigurationProvider 
{
    public GuildDetails GuildDetails { get; }
    public Founder Founder { get; }
    public DynamicMessages DynamicMessages { get; }
}
 