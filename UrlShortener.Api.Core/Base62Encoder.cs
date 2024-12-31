﻿namespace UrlShortener.Api.Core;

public static class Base62EncodingExtensions
{
    private const string Alphanumeric = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    public static string EncodeToBase62(this int number)
    {
        if (number == 0) return Alphanumeric[number].ToString();

        var result = new Stack<char>();
        while (number > 0)
        {
            result.Push(Alphanumeric[number % 62]);
            number /= 62;
        }
        return new string(result.ToArray());
    }
}