namespace UrlShortener.Api.Core;
public class TokenProvider
{
    private long _start;
    private long _end;

    public void AssignRange(TokenRange tr)
    {
        _start = tr.Start;
        _end = tr.End;
    }

    public long GetToken() {
        return _start;
    }


}

