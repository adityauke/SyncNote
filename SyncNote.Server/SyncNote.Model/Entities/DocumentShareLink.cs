using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class DocumentShareLink
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid DocumentId { get; set; }
    public Guid CreatedBy { get; set; }
    public required string TokenHash { get; set; }
    public DocumentRole Role { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset? ExpiresAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public Guid? RevokedBy { get; set; }

    public Document? Document { get; set; }
    public User? CreatedByUser { get; set; }
    public User? RevokedByUser { get; set; }
}
