namespace UrlShortener.Api.Core.Tests.TestDoubles;


internal class InMemoryUrlDataStore : Dictionary<string,ShortenedUrl>, IUrlDataStore
{
    public Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancel)
    {
        Add(shortenedUrl.ShortUrl, shortenedUrl);
        return Task.CompletedTask;
    }
}

