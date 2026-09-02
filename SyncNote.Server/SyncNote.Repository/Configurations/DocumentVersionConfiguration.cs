using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class DocumentVersionConfiguration : IEntityTypeConfiguration<DocumentVersion>
{
    public void Configure(EntityTypeBuilder<DocumentVersion> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Label).HasMaxLength(200);

        builder.HasIndex(v => new { v.DocumentId, v.VersionNumber })
            .IsUnique()
            .HasDatabaseName("ux_versions_doc_number");

        builder.HasIndex(v => new { v.DocumentId, v.CreatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_versions_timeline");

        builder.HasOne(v => v.Document)
            .WithMany()
            .HasForeignKey(v => v.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.Snapshot)
            .WithMany()
            .HasForeignKey(v => v.SnapshotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.CreatedByUser)
            .WithMany()
            .HasForeignKey(v => v.CreatedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
