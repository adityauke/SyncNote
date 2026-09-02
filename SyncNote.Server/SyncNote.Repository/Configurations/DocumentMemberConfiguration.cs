using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class DocumentMemberConfiguration : IEntityTypeConfiguration<DocumentMember>
{
    public void Configure(EntityTypeBuilder<DocumentMember> builder)
    {
        builder.HasKey(m => new { m.DocumentId, m.UserId })
            .HasName("pk_document_members");

        builder.HasIndex(m => new { m.UserId, m.DocumentId })
            .HasDatabaseName("ix_document_members_by_user");

        builder.HasOne(m => m.Document)
            .WithMany(d => d.Members)
            .HasForeignKey(m => m.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.AddedByUser)
            .WithMany()
            .HasForeignKey(m => m.AddedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
