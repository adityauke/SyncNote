using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SyncNote.Repository;

public static class SyncNoteDbContextOptions
{
    public const string ConnectionStringName = "Postgres";
    public const string MigrationsHistoryTable = "__ef_migrations_history";

    public static DbContextOptionsBuilder<TContext> UseSyncNote<TContext>(
        this DbContextOptionsBuilder<TContext> builder,
        string connectionString)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)builder).UseSyncNote(connectionString);

    public static DbContextOptionsBuilder UseSyncNote(this DbContextOptionsBuilder builder, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return builder
            .UseNpgsql(connectionString, npgsql => npgsql
                .MigrationsAssembly(typeof(SyncNoteDbContext).Assembly.FullName)
                .MigrationsHistoryTable(MigrationsHistoryTable))
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(warnings => warnings
                .Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning));
    }
}
