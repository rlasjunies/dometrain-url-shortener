namespace UrlShortener.Api.Core;

public interface IUrlDataStore
{
    Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken);
}