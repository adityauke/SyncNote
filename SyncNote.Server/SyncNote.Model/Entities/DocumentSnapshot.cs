namespace SyncNote.Model.Entities;

public class DocumentSnapshot
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid DocumentId { get; set; }
    public long UpToSeq { get; set; }
    public required byte[] StateVector { get; set; }
    public required byte[] YdocState { get; set; }
    public int SizeBytes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Document? Document { get; set; }
}
