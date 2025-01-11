using UrlShortener.Api.Core;

namespace UrlShortener.Api.Extension;


public static class ServiceCollectionExtensions {
    public static IServiceCollection AddUrlFeature(this IServiceCollection services)
    {
        services.AddScoped<AddUrlHandler>();
        services.AddSingleton<TokenProvider>();
        services.AddScoped<ShortUrlGenerator>();
        return services;
    }
}

