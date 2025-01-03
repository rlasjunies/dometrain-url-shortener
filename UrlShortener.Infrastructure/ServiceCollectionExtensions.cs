using Microsoft.Extensions.Configuration;

namespace UrlShortener.Infrastructure;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosUrlDataStore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<CosmosClient>(s =>
                new CosmosClient(configuration["CosmosDB:ConnectionString"]!));

            services.AddSingleton<IUrlDataStore>(s =>
            {
                var cosmosClient = s.GetRequiredService<CosmosClient>();
                var container = cosmosClient.GetContainer(
                    databaseId: configuration["DatabaseName"]!,
                    containerId: configuration["ContainerName"]!);
                return new CosmosDbUrlDataStore(container);
            });

            return services;
        }
    }

