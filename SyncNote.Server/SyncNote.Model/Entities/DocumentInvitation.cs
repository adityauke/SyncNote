using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class DocumentInvitation
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid DocumentId { get; set; }
    public Guid InvitedBy { get; set; }
    public required string Email { get; set; }
    public DocumentRole Role { get; set; }
    public required string TokenHash { get; set; }
    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? AcceptedAt { get; set; }
    public Guid? AcceptedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Document? Document { get; set; }
    public User? InvitedByUser { get; set; }
    public User? AcceptedByUser { get; set; }
}
