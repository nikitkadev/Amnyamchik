namespace Amnyam.Shared.JsonProviders;

public interface IJsonProvider<T>
{
    T GetConfig();
}
