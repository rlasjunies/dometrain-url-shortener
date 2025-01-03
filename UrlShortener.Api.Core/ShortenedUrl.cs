namespace UrlShortener.Api.Core;
public record ShortenedUrl(
    Uri LongUrl, 
    string ShortUrl, 
    string CreatedBy,
    DateTimeOffset CreatedOn
    );
