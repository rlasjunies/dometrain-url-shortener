﻿namespace UrlShortener.Api_;
public interface ITokenRangeApiClient
{
    Task<TokenRange?> AssignRangeAsync(string machineKey, CancellationToken cancellationToken);
}