using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).UseIdentityAlwaysColumn();
        builder.Property(l => l.EventType).HasMaxLength(60);
        builder.Property(l => l.EntityType).HasMaxLength(60);
        builder.Property(l => l.IpAddress).HasMaxLength(45);
        builder.Property(l => l.UserAgent).HasMaxLength(512);
        builder.Property(l => l.Metadata).HasColumnType("jsonb");

        builder.HasIndex(l => new { l.DocumentId, l.CreatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_audit_by_document");

        builder.HasIndex(l => new { l.ActorUserId, l.CreatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_audit_by_actor");

        builder.HasIndex(l => new { l.EventType, l.CreatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_audit_by_event");

        builder.HasOne(l => l.ActorUser)
            .WithMany()
            .HasForeignKey(l => l.ActorUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(l => l.Document)
            .WithMany()
            .HasForeignKey(l => l.DocumentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
