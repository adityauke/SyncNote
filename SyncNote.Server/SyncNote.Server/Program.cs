using SyncNote.Repository;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// --- CORS (the only setting we actually use in the pipeline right now) ---
var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "Missing Cors:AllowedOrigins. Add at least one frontend origin in appsettings.");
}

// --- AI provider (switch by changing Ai:Provider; each provider keeps its own model + creds) ---
var aiProviderName = configuration["Ai:Provider"];
if (string.IsNullOrWhiteSpace(aiProviderName))
{
    throw new InvalidOperationException(
        "Missing Ai:Provider. Set it to a key under Ai:Providers (e.g. Gemini, OpenAI, Ollama).");
}

var aiProvider = configuration.GetSection($"Ai:Providers:{aiProviderName}");
if (!aiProvider.Exists() || string.IsNullOrWhiteSpace(aiProvider["Model"]))
{
    throw new InvalidOperationException(
        $"Ai:Provider '{aiProviderName}' was not found under Ai:Providers, or its Model is missing.");
}

builder.Services.AddSyncNotePersistence(configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.Logger.LogInformation(
        "Loaded config — Postgres: {HasPostgres}, AI: {Provider}/{Model} ({BaseUrl}, key set: {HasApiKey}), CORS: {Origins}",
        !string.IsNullOrWhiteSpace(configuration.GetConnectionString("Postgres")),
        aiProviderName,
        aiProvider["Model"],
        aiProvider["BaseUrl"],
        !string.IsNullOrWhiteSpace(aiProvider["ApiKey"]),
        string.Join(", ", allowedOrigins));
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
