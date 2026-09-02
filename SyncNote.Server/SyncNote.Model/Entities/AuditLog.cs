namespace SyncNote.Model.Entities;

public class AuditLog
{
    public long Id { get; set; }
    public Guid? ActorUserId { get; set; }
    public required string EventType { get; set; }
    public Guid? DocumentId { get; set; }
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Metadata { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public User? ActorUser { get; set; }
    public Document? Document { get; set; }
}
