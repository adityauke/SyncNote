using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Title).HasMaxLength(255);

        builder.HasQueryFilter(d => d.DeletedAt == null);

        builder.HasIndex(d => new { d.OwnerId, d.UpdatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_documents_owner_recent");

        builder.HasIndex(d => new { d.Status, d.UpdatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_documents_status_recent");

        builder.HasIndex(d => d.Title)
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops")
            .HasDatabaseName("ix_documents_title_search");

        builder.HasOne(d => d.Owner)
            .WithMany()
            .HasForeignKey(d => d.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
