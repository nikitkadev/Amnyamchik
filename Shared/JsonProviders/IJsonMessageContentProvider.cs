namespace Amnyam.Shared.JsonProviders;

public interface IJsonMessageContentProvider<T>
{
    public string Title { get; }
    public string Heading { get; }
    public string Description { get; }
    public List<T> ContentList { get; }
    public string Footer { get; }
}
