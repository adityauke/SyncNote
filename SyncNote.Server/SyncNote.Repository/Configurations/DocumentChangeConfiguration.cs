using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class DocumentChangeConfiguration : IEntityTypeConfiguration<DocumentChange>
{
    public void Configure(EntityTypeBuilder<DocumentChange> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).UseIdentityAlwaysColumn();

        builder.HasIndex(c => new { c.DocumentId, c.Seq })
            .IsUnique()
            .HasDatabaseName("ux_document_changes_doc_seq");

        builder.HasOne(c => c.Document)
            .WithMany()
            .HasForeignKey(c => c.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
