using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class DocumentSnapshotConfiguration : IEntityTypeConfiguration<DocumentSnapshot>
{
    public void Configure(EntityTypeBuilder<DocumentSnapshot> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasIndex(s => new { s.DocumentId, s.UpToSeq })
            .IsUnique()
            .HasDatabaseName("ux_snapshots_doc_seq");

        builder.HasOne(s => s.Document)
            .WithMany()
            .HasForeignKey(s => s.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
