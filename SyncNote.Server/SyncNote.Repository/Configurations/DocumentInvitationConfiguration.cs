using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class DocumentInvitationConfiguration : IEntityTypeConfiguration<DocumentInvitation>
{
    public void Configure(EntityTypeBuilder<DocumentInvitation> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Email).HasMaxLength(320);
        builder.Property(i => i.TokenHash).HasMaxLength(64).IsFixedLength();

        builder.HasIndex(i => i.TokenHash)
            .IsUnique()
            .HasDatabaseName("ux_invitations_hash");

        builder.HasIndex(i => new { i.DocumentId, i.Email })
            .HasDatabaseName("ix_invitations_doc_email");

        builder.HasIndex(i => new { i.Status, i.ExpiresAt })
            .HasDatabaseName("ix_invitations_expiry_sweep");

        builder.HasOne(i => i.Document)
            .WithMany(d => d.Invitations)
            .HasForeignKey(i => i.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.InvitedByUser)
            .WithMany()
            .HasForeignKey(i => i.InvitedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.AcceptedByUser)
            .WithMany()
            .HasForeignKey(i => i.AcceptedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
