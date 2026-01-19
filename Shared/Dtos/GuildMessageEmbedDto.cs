using Discord;

namespace Amnyam.Shared.Dtos;

public class GuildMessageEmbedDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PicturesUrl { get; set; } = string.Empty;

    public bool WithTimestamp { get; set; } = false;

    public Color Color { get; set; } = new(50, 50, 53);
    public GuildMessageEmbedFooterDto? FooterDto { get; set; }
    public GuildMessageEmbedAuthorDto? AuthorDto { get; set; }

    public IReadOnlyCollection<EmbedFieldBuilder>? EmbedFields { get; set; } 
}
