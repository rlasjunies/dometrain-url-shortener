﻿using System.Collections.Concurrent;

namespace UrlShortener.Api.Core.Tests;

public class TokenProviderScenarios
{

    [Fact]
    public void Should_get_the_token_from_the_start()
    {
        var provider = new TokenProvider();
        
        provider.AssignRange(new TokenRange(5,10));

        provider.GetToken().Should().Be(5);
    }

    [Fact]
    public void Should_increment_token_on_get()
    {
        var provider = new TokenProvider();

        provider.AssignRange(new TokenRange(5, 10));
        provider.GetToken();

        provider.GetToken().Should().Be(6);
    }


    [Fact]
    public void Should_not_return_same_token_twice()
    {
        var provider = new TokenProvider();
        ConcurrentBag<long> tokens = [];

        const int start = 1;
        const int end = 10000;
        provider.AssignRange(new TokenRange(start, end));

        Parallel.ForEach(Enumerable.Range(start, end),
            _ => tokens.Add(provider.GetToken()));

        tokens.Should().OnlyHaveUniqueItems();
    }

}

