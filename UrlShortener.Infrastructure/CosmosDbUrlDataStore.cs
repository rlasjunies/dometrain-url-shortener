
namespace UrlShortener.Infrastructure;

public class CosmosDbUrlDataStore(Container container) : IUrlDataStore
{
    private readonly Container _container = container;

    public async Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken)
    {

        var document = (ShortenedUrl4Cosmos)shortenedUrl;
        await _container.CreateItemAsync(document,
            new PartitionKey(document.PartitionKey),
                cancellationToken: cancellationToken); 
    }
    internal class ShortenedUrl4Cosmos
    {
        public string LongUrl { get; }

        [JsonProperty(PropertyName = "id")] // Cosmos DB Unique Identifier
        public string ShortUrl { get; }

        public DateTimeOffset CreatedOn { get; }
        public string CreatedBy { get; }

        public string PartitionKey => ShortUrl[..1]; // Cosmos DB Partition Key

        public ShortenedUrl4Cosmos(string longUrl, string shortUrl, string createdBy, DateTimeOffset createdOn)
        {
            LongUrl = longUrl;
            ShortUrl = shortUrl;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }

        public static implicit operator ShortenedUrl(ShortenedUrl4Cosmos url4) =>
            new(new Uri(url4.LongUrl), url4.ShortUrl, url4.CreatedBy, url4.CreatedOn);

        public static explicit operator ShortenedUrl4Cosmos(ShortenedUrl url) =>
            new(url.LongUrl.ToString(), url.ShortUrl, url.CreatedBy, url.CreatedOn);
    }
}
