using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SyncNote.Repository;

public static class DependencyInjection
{
    public static IServiceCollection AddSyncNotePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(SyncNoteDbContextOptions.ConnectionStringName);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Missing ConnectionStrings:{SyncNoteDbContextOptions.ConnectionStringName}. " +
                "Add the PostgreSQL connection string in appsettings.");
        }

        services.AddDbContextPool<SyncNoteDbContext>(options => options.UseSyncNote(connectionString));

        return services;
    }
}
