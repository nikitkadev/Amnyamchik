namespace Amnyam.Shared.Dtos;

public class GuildMessageDto
{
    public string Message { get; set; } = string.Empty;
    public GuildMessageEmbedDto? Embed { get; set; } 
}
