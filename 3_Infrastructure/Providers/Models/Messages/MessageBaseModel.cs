namespace Amnyam._3_Infrastructure.Providers.Models.Messages;

public class MessageBaseModel<T>
{
    public string Title { get; set; } = string.Empty;
    public string Heading { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<T> ContentList { get; set; } = [];
    public string Footer { get; set; } = string.Empty;
}
