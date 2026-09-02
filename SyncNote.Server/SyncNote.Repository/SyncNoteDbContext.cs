using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SyncNote.Model.Entities;

namespace SyncNote.Repository;

public class SyncNoteDbContext(DbContextOptions<SyncNoteDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentMember> DocumentMembers => Set<DocumentMember>();
    public DbSet<DocumentInvitation> DocumentInvitations => Set<DocumentInvitation>();
    public DbSet<DocumentShareLink> DocumentShareLinks => Set<DocumentShareLink>();

    public DbSet<DocumentChange> DocumentChanges => Set<DocumentChange>();
    public DbSet<DocumentSnapshot> DocumentSnapshots => Set<DocumentSnapshot>();
    public DbSet<DocumentVersion> DocumentVersions => Set<DocumentVersion>();
    public DbSet<DocumentLock> DocumentLocks => Set<DocumentLock>();

    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentMention> CommentMentions => Set<CommentMention>();

    public DbSet<AiRequest> AiRequests => Set<AiRequest>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SyncNoteDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var clrType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
                if (clrType.IsEnum)
                {
                    property.SetProviderClrType(typeof(short));
                }
            }

            foreach (var keyProperty in entityType.FindPrimaryKey()?.Properties ?? [])
            {
                if (keyProperty.ClrType == typeof(Guid))
                {
                    keyProperty.ValueGenerated = ValueGenerated.Never;
                }
            }
        }
    }
}
