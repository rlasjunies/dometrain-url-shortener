


namespace UrlShortener.Api.Tests
{
    public class AddUrlFeature(ApiFixture fixture) : IClassFixture<ApiFixture>
    {
        private readonly HttpClient _client = fixture.CreateClient();

        [Fact]
        public async Task Given_long_url_will_return_short_url()
        {
            var response = await _client.PostAsJsonAsync<AddUrlRequest>("/api/urls",
                new AddUrlRequest(new Uri("https://dometrain.com"), "rlasjunies@gmail.com"));

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var addUrlResponse = await response.Content.ReadFromJsonAsync<AddUrlResponse>();
            addUrlResponse!.ShortUrl.Should().NotBeNull();


        }
    }
}