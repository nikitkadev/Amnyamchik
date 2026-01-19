using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Amnyam.Shared.JsonProviders;

public static class JsonProviderExtensions
{
    public static IServiceCollection AddJsonProvider<TInterface, TImplementation>(
         this IServiceCollection services,
         string filePath)
         where TInterface : class
         where TImplementation : class, TInterface
    {
        services.AddSingleton<TInterface>(sp =>
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, filePath));
            var logger = sp.GetRequiredService<ILogger<TImplementation>>();

            return (TInterface)Activator.CreateInstance(
                typeof(TImplementation),
                path,
                logger)!;
        });

        return services;
    }
}
