namespace UrlShortener.Api.Core;
public class TokenProvider
{
    private long _start;
    private long _end;
    private long _token;
    private readonly object _tokenLock = new();

    public void AssignRange(TokenRange tr)
    {
        _start = tr.Start;
        _token = tr.Start;
        _end = tr.End;
    }

    public long GetToken()
    {
        lock (_tokenLock)
        {
            return _token++;
        }
    }


}

