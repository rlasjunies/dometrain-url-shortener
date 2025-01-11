namespace UrlShortener.TokenRangeService;

internal class TokenRangeManager(string connectionString)
{
    private const int DefaultRangeSize = 1000;

    private readonly string _sqlQuery =
        $$"""
               INSERT INTO "TokenRanges" ("MachineIdentifier", "Start", "End")
               VALUES (
                   @MachineIdentifier,
                   COALESCE((SELECT MAX("End") FROM "TokenRanges") + 1, {{DefaultRangeSize}}),
                   COALESCE((SELECT MAX("End") FROM "TokenRanges") + {{DefaultRangeSize}}, 2000)
               )
               RETURNING "Id", "MachineIdentifier", "Start", "End";
          """;

    public async Task<AssignTokenRangeResponse> AssignRangeAsync(string machineIdentifier)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(_sqlQuery, connection);
        command.Parameters.AddWithValue("@MachineIdentifier", machineIdentifier);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new AssignTokenRangeResponse
            (
                reader.GetInt64(2),
                reader.GetInt64(3)
            );
        }
        throw new FailedToAssignRangeException("Failed to assign range.");
    }
}