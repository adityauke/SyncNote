using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class DocumentMember
{
    public Guid DocumentId { get; set; }
    public Guid UserId { get; set; }
    public DocumentRole Role { get; set; }
    public Guid AddedBy { get; set; }
    public DateTimeOffset AddedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Document? Document { get; set; }
    public User? User { get; set; }
    public User? AddedByUser { get; set; }
}
