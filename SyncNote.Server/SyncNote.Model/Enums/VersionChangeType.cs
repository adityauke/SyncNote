namespace SyncNote.Model.Enums;

public enum VersionChangeType : short
{
    Created = 0,
    ContentUpdate = 1,
    AiRewrite = 2,
    Restore = 3,
    ManualCheckpoint = 4
}
