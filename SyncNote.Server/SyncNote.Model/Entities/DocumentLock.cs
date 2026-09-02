using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class DocumentLock
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid DocumentId { get; set; }
    public Guid LockedBy { get; set; }
    public LockType LockType { get; set; } = LockType.Hard;
    public DateTimeOffset LockedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? ReleasedAt { get; set; }

    public Document? Document { get; set; }
    public User? LockedByUser { get; set; }
}
