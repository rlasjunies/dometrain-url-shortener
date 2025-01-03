
namespace UrlShortener.Api.Core;

public record AddUrlRequest(Uri LongUrl, string CreatedBy);
public record AddUrlResponse(Uri LongUrl, string ShortUrl);
public class AddUrlHandler
{
    private readonly IUrlDataStore _urlDataStore;
    private readonly TimeProvider _timeProvider;
    private readonly ShortUrlGenerator _shortUrlGenerator;

    public AddUrlHandler(ShortUrlGenerator shortUrlGenerator, 
        IUrlDataStore urlDataStore,
        TimeProvider timeProvider)
    {
        _urlDataStore = urlDataStore;
        _timeProvider = timeProvider;
        _shortUrlGenerator = shortUrlGenerator;

    }

    public async Task<Result<AddUrlResponse>> HandleAsync(AddUrlRequest request, CancellationToken cancel)
    {
        if (string.IsNullOrEmpty(request.CreatedBy))
            return Errors.CreatedByHaveToBeDefined;

        var shortenedUrl = new ShortenedUrl(
            request.LongUrl, 
            _shortUrlGenerator.GenerateUniqueUrl(),
            request.CreatedBy,
            _timeProvider.GetUtcNow());
        await _urlDataStore.AddAsync(shortenedUrl,cancel);
        return new AddUrlResponse(shortenedUrl.LongUrl, shortenedUrl.ShortUrl);
    }
}