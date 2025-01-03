using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Api.Tests;

public static class ServiceCollectionExtensions
{
    public static void Remove<T>(this IServiceCollection services)
    {
        var service = services.SingleOrDefault(s =>
            s.ServiceType == typeof(T));
        if (service != null) services.Remove(service);
    }
}

