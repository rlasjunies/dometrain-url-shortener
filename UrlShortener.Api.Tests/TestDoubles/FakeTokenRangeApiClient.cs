using UrlShortener.Api_;


namespace UrlShortener.Tests.TestDoubles;

public class FakeTokenRangeApiClient : ITokenRangeApiClient
{
    public Task<TokenRange?> AssignRangeAsync(string machineKey, CancellationToken cancellationToken)
    {
        return Task.FromResult<TokenRange?>(new TokenRange(1, 10));
    }
}