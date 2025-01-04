﻿

namespace UtlShortener.TokenRangeService.Tests;

    public  class WebAppFixture : WebApplicationFactory<ITokenRangeAssemblyMarker>, IAsyncLifetime
    {

        private readonly PostgreSqlContainer _postgresContainer;
        public string ConnectionString => _postgresContainer.GetConnectionString();

        public WebAppFixture()
        {
            _postgresContainer = new PostgreSqlBuilder().Build();
        }

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();
            Environment.SetEnvironmentVariable("Postgres__ConnectionString",ConnectionString);
            await InitializeSqlTable();
        }

        private async Task InitializeSqlTable()
        {
            var tableSql = await File.ReadAllTextAsync(
                "Table.sql");

            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(tableSql, connection);
            await command.ExecuteNonQueryAsync();
        }

    public async Task DisposeAsync()
        {
            await _postgresContainer.StopAsync();
        }

}

