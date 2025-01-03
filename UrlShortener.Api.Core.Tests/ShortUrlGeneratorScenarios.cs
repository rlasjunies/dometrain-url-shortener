
namespace UrlShortener.Api.Core.Tests;

public class ShortUrlGeneratorScenarios
{

    [Fact]
    public void Should_return_short_url_for_0()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange( new TokenRange(0, 1000));
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

        shortUrl.Should().Be("0");
    }

    [Fact]
    public void Should_return_short_url_for_10001()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(new TokenRange(10001, 90000));
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

        shortUrl.Should().Be("2bJ");
    }

    [Fact]
    public void Should_return_error_when_start_is_gt_end_range()
    {
        var act = () => new TokenRange(2000, 1000);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("End must be greater than or eaqual to start range");
    }

    [Fact]
    public void Should_support_high_number_up_to_62_exposant_7()
    {
        var act = () => new TokenRange(3521614606207, 3521614606208);

        act.Should()
            .NotThrow<Exception>();
    }
}

