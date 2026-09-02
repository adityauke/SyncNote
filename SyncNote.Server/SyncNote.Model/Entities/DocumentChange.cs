using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class DocumentChange
{
    public long Id { get; set; }
    public Guid DocumentId { get; set; }
    public long Seq { get; set; }
    public Guid? UserId { get; set; }
    public ChangeOrigin Origin { get; set; } = ChangeOrigin.User;
    public required byte[] UpdateBlob { get; set; }
    public int SizeBytes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Document? Document { get; set; }
    public User? User { get; set; }
}
