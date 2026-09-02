using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class AiRequestConfiguration : IEntityTypeConfiguration<AiRequest>
{
    public void Configure(EntityTypeBuilder<AiRequest> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Provider).HasMaxLength(50);
        builder.Property(r => r.Model).HasMaxLength(100);
        builder.Property(r => r.ErrorCode).HasMaxLength(60);

        builder.HasIndex(r => new { r.UserId, r.CreatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_ai_requests_quota");

        builder.HasIndex(r => new { r.DocumentId, r.CreatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_ai_requests_by_doc");

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Document)
            .WithMany()
            .HasForeignKey(r => r.DocumentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
