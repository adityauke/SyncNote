namespace SyncNote.Model.Enums;

public enum NotificationType : short
{
    CommentMention = 0,
    CommentReply = 1,
    InvitationReceived = 2,
    InvitationAccepted = 3,
    MembershipChanged = 4,
    DocumentShared = 5,
    VersionRestored = 6
}
