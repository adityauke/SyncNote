using SyncNote.Model.Enums;

namespace SyncNote.Model.Entities;

public class AiRequest
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid UserId { get; set; }
    public Guid? DocumentId { get; set; }
    public AiOperation Operation { get; set; }
    public required string Provider { get; set; }
    public required string Model { get; set; }
    public int? PromptTokens { get; set; }
    public int? CompletionTokens { get; set; }
    public int? DurationMs { get; set; }
    public AiRequestStatus Status { get; set; }
    public string? ErrorCode { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public User? User { get; set; }
    public Document? Document { get; set; }
}
