


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
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IUrlDataStore, InMemoryUrlDataStore>();
builder.Services.AddUrlFeature();

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

        var result = await handler.HandleAsync(request, cancellationToken);

        if (result.Failed) return Results.BadRequest(result.Error);

        return Results.Created($"/api/urls/{result.Value!.ShortUrl}", result.Value);
    });

app.Run();


internal class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
{
    public Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancel)
    {
        Add(shortenedUrl.ShortUrl, shortenedUrl);
        return Task.CompletedTask;
    }
}