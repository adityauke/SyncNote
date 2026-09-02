using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SyncNote.Repository;

public sealed class SyncNoteDbContextFactory : IDesignTimeDbContextFactory<SyncNoteDbContext>
{
    public SyncNoteDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(FindHostProjectDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString(SyncNoteDbContextOptions.ConnectionStringName);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Missing ConnectionStrings:{SyncNoteDbContextOptions.ConnectionStringName}. " +
                "Set it in SyncNote.Server/appsettings.Development.json or as ConnectionStrings__Postgres.");
        }

        var options = new DbContextOptionsBuilder<SyncNoteDbContext>()
            .UseSyncNote(connectionString)
            .Options;

        return new SyncNoteDbContext(options);
    }

    private static string FindHostProjectDirectory()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "SyncNote.Server");
            if (File.Exists(Path.Combine(candidate, "appsettings.json")))
            {
                return candidate;
            }

            directory = directory.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}
