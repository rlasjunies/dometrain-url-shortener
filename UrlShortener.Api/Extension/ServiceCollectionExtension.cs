using UrlShortener.Api.Core;

namespace UrlShortener.Api.Extension;


public static class ServiceCollectionExtensions {
    public static IServiceCollection AddUrlFeature(this IServiceCollection services)
    {
        services.AddScoped<AddUrlHandler>();
        services.AddSingleton<TokenProvider>(_ =>
        {
            var tokenProvider = new TokenProvider();
            tokenProvider.AssignRange(new TokenRange(1, 1000));
            return new TokenProvider();
        });
        services.AddScoped<ShortUrlGenerator>();
        return services;
    }
}

