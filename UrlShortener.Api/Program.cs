
using UrlShortener.Api;
using UrlShortener.Api_;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton(TimeProvider.System)
    .AddSingleton<IEnvironmentManager, EnvironmentManager>();
//builder.Services.AddSingleton<IUrlDataStore, InMemoryUrlDataStore>();
builder.Services
    .AddUrlFeature()
    .AddCosmosUrlDataStore(builder.Configuration);

builder.Services.AddHttpClient("TokenRangeService",
    client =>
    {
        client.BaseAddress =
            new Uri(builder.Configuration["TokenRangeService:Endpoint"]!); // TODO: Add to bicep
    });

builder.Services.AddSingleton<ITokenRangeApiClient, TokenRangeApiClient>();
builder.Services.AddHostedService<TokenManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/urls",
    async (AddUrlHandler handler, AddUrlRequest request, CancellationToken cancellationToken) =>
    {
        var requestWithUser = request with
        {
            CreatedBy = "not empty"
        };

        var result = await handler.HandleAsync(requestWithUser, cancellationToken);

        if (result.Failed) return Results.BadRequest(result.Error);

        return Results.Created($"/api/urls/{result.Value!.ShortUrl}", result.Value);
    });

app.MapGet("/", () => "UrlShortener - Hello World!");

app.Run();