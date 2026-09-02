using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;
using SyncNote.Model.Enums;

namespace SyncNote.Repository.Configurations;

public sealed class DocumentShareLinkConfiguration : IEntityTypeConfiguration<DocumentShareLink>
{
    public void Configure(EntityTypeBuilder<DocumentShareLink> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.TokenHash).HasMaxLength(64).IsFixedLength();

        builder.ToTable(t => t.HasCheckConstraint(
            "ck_document_share_links_role",
            $"role IN ({(short)DocumentRole.Editor}, {(short)DocumentRole.Viewer})"));

        builder.HasIndex(l => l.TokenHash)
            .IsUnique()
            .HasDatabaseName("ux_share_links_hash");

        builder.HasIndex(l => new { l.DocumentId, l.IsEnabled })
            .HasDatabaseName("ix_share_links_active");

        builder.HasOne(l => l.Document)
            .WithMany(d => d.ShareLinks)
            .HasForeignKey(l => l.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.CreatedByUser)
            .WithMany()
            .HasForeignKey(l => l.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.RevokedByUser)
            .WithMany()
            .HasForeignKey(l => l.RevokedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
