using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class Comment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid DocumentId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public Guid AuthorId { get; set; }
    public string? AnchorId { get; set; }
    public string? QuotedText { get; set; }
    public required string Body { get; set; }
    public CommentStatus Status { get; set; } = CommentStatus.Open;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? EditedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? ResolvedBy { get; set; }
    public DateTimeOffset? ResolvedAt { get; set; }

    public Document? Document { get; set; }
    public Comment? ParentComment { get; set; }
    public User? Author { get; set; }
    public User? ResolvedByUser { get; set; }
    public ICollection<Comment> Replies { get; set; } = [];
    public ICollection<CommentMention> Mentions { get; set; } = [];
}
