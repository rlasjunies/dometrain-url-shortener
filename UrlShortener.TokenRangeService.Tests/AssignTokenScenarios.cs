


using System.Collections.Concurrent;

namespace UtlShortener.TokenRangeService.Tests
{
    public class AssignTokenScenarios(WebAppFixture fixture) : IClassFixture<WebAppFixture>
    {
        private readonly HttpClient _clientHttp = fixture.CreateClient();

        [Fact]
        public async Task Should_return_range_when_requested()
        {
            var response = await _clientHttp.PostAsJsonAsync("/assign",
                new AssignTokenRangeRequest("tests"));

            response.Should().BeSuccessful();
            var responseContent = await response.Content.ReadFromJsonAsync<AssignTokenRangeResponse>();

            responseContent.Start.Should().BeGreaterThan(0);
            responseContent.End.Should().BeGreaterThan(responseContent.Start);

        }

        [Fact]
        public async Task Should_not_repeat_range_when_requested()
        {
            var requestResponse1 = await _clientHttp.PostAsJsonAsync("/assign",
                new AssignTokenRangeRequest("tests"));
            var requestResponse2 = await _clientHttp.PostAsJsonAsync("/assign",
                new AssignTokenRangeRequest("tests"));

            requestResponse1.Should().BeSuccessful();
            requestResponse2.Should().BeSuccessful();
            var tokenRange1 = await requestResponse1.Content
                .ReadFromJsonAsync<AssignTokenRangeResponse>();
            var tokenRange2 = await requestResponse2.Content
                .ReadFromJsonAsync<AssignTokenRangeResponse>();
            tokenRange2!.Start.Should().BeGreaterThan(tokenRange1!.End);
        }

        [Fact]
        public async Task Should_not_repeat_range_on_multiple_requests()
        {
            ConcurrentBag<AssignTokenRangeResponse> ranges = [];
            await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (number, cancellationToken) =>
            {
                var response = await _clientHttp
                    .PostAsJsonAsync("/assign",
                        new AssignTokenRangeRequest(number.ToString()),
                        cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var range = await response.Content
                        .ReadFromJsonAsync<AssignTokenRangeResponse>(cancellationToken: cancellationToken);
                    ranges.Add(range!);
                }
            });

            ranges.Should().OnlyHaveUniqueItems(x => x.Start);
            ranges.Should().OnlyHaveUniqueItems(x => x.End);
        }
    }
}

