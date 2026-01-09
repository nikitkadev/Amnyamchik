namespace MlkAdmin.Shared.JsonProviders;

public interface IJsonProvider<T>
{
    T GetConfig();
}
