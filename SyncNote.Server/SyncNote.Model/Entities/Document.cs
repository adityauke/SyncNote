using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class Document
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = "Untitled Document";
    public Guid OwnerId { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Active;
    public int CurrentVersion { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public User? Owner { get; set; }
    public ICollection<DocumentMember> Members { get; set; } = [];
    public ICollection<DocumentInvitation> Invitations { get; set; } = [];
    public ICollection<DocumentShareLink> ShareLinks { get; set; } = [];
}
