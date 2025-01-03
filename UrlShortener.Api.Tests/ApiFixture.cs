namespace UrlShortener.Api.Tests;

public class ApiFixture : WebApplicationFactory<IApiAssemblyMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureTestServices(
            services =>
            {
                services.Remove<IUrlDataStore>();
                services.AddSingleton<IUrlDataStore>(new InMemoryUrlDataStore());
            });
        base.ConfigureWebHost(builder);
    }
}

internal class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
{
    public Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancel)
    {
        Add(shortenedUrl.ShortUrl, shortenedUrl);
        return Task.CompletedTask;
    }
}