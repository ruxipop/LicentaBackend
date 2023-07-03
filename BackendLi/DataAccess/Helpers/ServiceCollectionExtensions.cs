using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BackendLi.Helpers;

public static class ServiceCollectionExtensions
{
    public static TSettings AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration)
        where TSettings : class, new()
    {
        TSettings setting = configuration.Get<TSettings>();
        services.TryAddSingleton(setting);

        return setting;
    }
}