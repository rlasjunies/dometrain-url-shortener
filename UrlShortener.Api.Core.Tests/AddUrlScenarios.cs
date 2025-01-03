using Microsoft.Extensions.Time.Testing;
using Newtonsoft.Json.Linq;
using UrlShortener.Api.Core.Tests.TestDoubles;

namespace UrlShortener.Api.Core.Tests;

public class AddUrlScenarios
{
    private readonly AddUrlHandler _handler;
    private readonly InMemoryUrlDataStore _urlDataStore;
    private readonly FakeTimeProvider _timeProvider;

    public AddUrlScenarios()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(new TokenRange(1, 5));
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);

        _timeProvider = new FakeTimeProvider();
        _urlDataStore = new InMemoryUrlDataStore();
        _handler = new AddUrlHandler(shortUrlGenerator, _urlDataStore, _timeProvider);
    }

    [Fact]
    public async void Should_return_shortened_url()
    {

        var request = CreateAddUrlRequestMessage();
        var response = await _handler.HandleAsync(request, default);
        response.Succeeded.Should().BeTrue();
        response.Value.ShortUrl.Should().NotBeEmpty();
        response.Value.ShortUrl.Should().Be("1");
    }

    [Fact]
    public async Task Should_save_short_url()
    {
        var request = CreateAddUrlRequestMessage();
        var response = await _handler.HandleAsync(request, default);

        response.Succeeded.Should().BeTrue();
        _urlDataStore.Should().ContainKey(response.Value.ShortUrl);
    }


    [Fact]
    public async Task Should_return_error_if_creator_is_emptY()
    {
        var request = CreateAddUrlRequestMessage(createdBy:"");
        var response = await _handler.HandleAsync(request, default);

        response.Succeeded.Should().BeFalse();

        var expectedError = response.Error.Should().BeEquivalentTo(Errors.CreatedBy_have_to_be_defined);

    }

    [Fact]
    public async Task Should_save_short_url_with_created_by_and_created_on()
    {
        var request = CreateAddUrlRequestMessage();
        var response = await _handler.HandleAsync(request, default);

        response.Succeeded.Should().BeTrue();
        _urlDataStore.Should().ContainKey(response.Value.ShortUrl);
        _urlDataStore[response.Value.ShortUrl].CreatedBy.Should().Be(request.CreatedBy);
        _urlDataStore[response.Value.ShortUrl].CreatedOn.Should().Be(_timeProvider.GetUtcNow());
    }

    private static AddUrlRequest CreateAddUrlRequestMessage(string createdBy = "rlasjunies@gmail.com")
    {
        var request = new AddUrlRequest(
            new Uri("https://dometrain.com"),
            createdBy
        );
        return request;
    }
}



