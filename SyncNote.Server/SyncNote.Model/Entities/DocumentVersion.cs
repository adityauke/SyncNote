using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class DocumentVersion
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid DocumentId { get; set; }
    public int VersionNumber { get; set; }
    public Guid SnapshotId { get; set; }
    public VersionChangeType ChangeType { get; set; }
    public string? Label { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Document? Document { get; set; }
    public DocumentSnapshot? Snapshot { get; set; }
    public User? CreatedByUser { get; set; }
}
