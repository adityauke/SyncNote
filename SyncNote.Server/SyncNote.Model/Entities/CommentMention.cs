namespace SyncNote.Model.Entities;

public class CommentMention
{
    public Guid CommentId { get; set; }
    public Guid MentionedUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? NotifiedAt { get; set; }

    public Comment? Comment { get; set; }
    public User? MentionedUser { get; set; }
}
