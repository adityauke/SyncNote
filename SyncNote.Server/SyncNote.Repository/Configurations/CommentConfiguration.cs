using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncNote.Model.Entities;

namespace SyncNote.Repository.Configurations;

public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.AnchorId).HasMaxLength(200);
        builder.Property(c => c.QuotedText).HasMaxLength(1000);

        builder.HasQueryFilter(c => c.DeletedAt == null);

        builder.ToTable(t => t.HasCheckConstraint(
            "ck_comments_reply_has_no_anchor",
            "parent_comment_id IS NULL OR anchor_id IS NULL"));

        builder.HasIndex(c => new { c.DocumentId, c.Status, c.CreatedAt })
            .HasDatabaseName("ix_comments_doc_open");

        builder.HasIndex(c => new { c.ParentCommentId, c.CreatedAt })
            .HasDatabaseName("ix_comments_thread");

        builder.HasOne(c => c.Document)
            .WithMany()
            .HasForeignKey(c => c.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ResolvedByUser)
            .WithMany()
            .HasForeignKey(c => c.ResolvedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
