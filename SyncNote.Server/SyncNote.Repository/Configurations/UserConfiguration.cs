using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email).HasMaxLength(320);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(320);
        builder.Property(u => u.DisplayName).HasMaxLength(100);
        builder.Property(u => u.PasswordHash).HasMaxLength(255);
        builder.Property(u => u.AvatarUrl).HasMaxLength(2048);

        builder.HasIndex(u => u.NormalizedEmail)
            .IsUnique()
            .HasDatabaseName("ux_users_normalized_email");
    }
}
