namespace UrlShortener.Api.Core;

public record Error(string Code, string Description)
{
    public static Error None => new Error(String.Empty, String.Empty);
}

public static class Errors
{
    public static Error CreatedBy_have_to_be_defined => new Error("Missing_Created_by", "Created by must de provided");

}