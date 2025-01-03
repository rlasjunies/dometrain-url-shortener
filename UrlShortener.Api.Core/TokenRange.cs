namespace UrlShortener.Api.Core;

public record TokenRange
{
    public long Start { get; }
    public long End { get; }

    public TokenRange(long start, long end)
    {
        if (end < start)
            throw new ArgumentException("End must be greater than or eaqual to start range");

        Start = start;
        End = end;
    }
};