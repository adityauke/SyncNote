using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class Notification
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public required string Title { get; set; }
    public string? Body { get; set; }
    public Guid? DocumentId { get; set; }
    public Guid? RefId { get; set; }
    public bool IsRead { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public User? User { get; set; }
    public Document? Document { get; set; }
}
