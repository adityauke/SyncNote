using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class DocumentLockConfiguration : IEntityTypeConfiguration<DocumentLock>
{
    public void Configure(EntityTypeBuilder<DocumentLock> builder)
    {
        builder.HasKey(l => l.Id);

        builder.HasIndex(l => l.DocumentId)
            .IsUnique()
            .HasFilter("released_at IS NULL")
            .HasDatabaseName("ux_locks_one_active_per_doc");

        builder.HasOne(l => l.Document)
            .WithMany()
            .HasForeignKey(l => l.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.LockedByUser)
            .WithMany()
            .HasForeignKey(l => l.LockedBy)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
