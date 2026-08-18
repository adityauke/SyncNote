# PRD — Real-Time Collaborative Document Editor

**Project Type:** Personal / Portfolio Project
**Backend:** ASP.NET Core Web API
**Frontend:** React + TypeScript
**Database:** SQL Server
**Real-Time:** SignalR
**Collaboration:** Yjs CRDT
**Distributed State:** Redis
**AI:** Ollama + Local LLM
**Deployment:** Docker
**Goal:** Build a self-hosted, Google Docs–style collaborative document platform.

---

# 1. Product Overview

A web-based document editor where multiple users can simultaneously edit the same document in real time.

The system will support:

* Real-time collaborative editing
* Multiple concurrent users
* Cursor and selection sharing
* Presence indicators
* Conflict-free editing
* Offline editing
* Autosave
* Version history
* Revision restoration
* Comments
* Document sharing
* Permissions
* Document locking
* Undo/redo
* AI-assisted writing
* AI-generated collaborative edits
* Audit history

The application should be designed as a **distributed real-time system**, not simply as a CRUD document application.

---

# 2. Product Goals

### Primary Goals

1. Allow multiple users to edit one document simultaneously.
2. Prevent users from losing changes due to concurrent editing.
3. Synchronize changes in real time.
4. Support offline editing and automatic synchronization.
5. Maintain document history and recoverable versions.
6. Provide secure document sharing and permissions.
7. Integrate a locally hosted AI writing assistant.
8. Demonstrate scalable distributed-system architecture.

### Non-Goals — Initial Version

Do not initially build:

* Video conferencing
* Voice calls
* Full Google Drive replacement
* Complex spreadsheet editor
* Presentation editor
* Enterprise SSO
* Paid cloud AI integrations

---

# 3. Target Users

### Owner

Creates and manages documents.

### Editor

Can modify document content.

### Commenter

Can comment but cannot modify document content.

### Viewer

Can only view the document.

---

# 4. Recommended Technology Stack

## Frontend

```text
React
TypeScript
Vite
Tiptap
ProseMirror
Yjs
SignalR Client
Zustand
Tailwind CSS
shadcn/ui
React Router
```

## Backend

```text
ASP.NET Core Web API
ASP.NET Core SignalR
Entity Framework Core
SQL Server
Redis
BackgroundService
JWT Authentication
```

## AI

```text
Ollama
Qwen / Llama / Gemma
Optional Qdrant
```

## Development

```text
Visual Studio / VS Code
Git
GitHub
Docker
Docker Compose
```

---

# 5. High-Level Architecture

```text
                     React Application
                           │
             ┌─────────────┴─────────────┐
             │                           │
       REST API Client              SignalR Client
             │                           │
             └─────────────┬─────────────┘
                           │
                    ASP.NET Core
                           │
       ┌───────────────────┼───────────────────┐
       │                   │                   │
   Web API              SignalR            AI Service
       │                   │                   │
       │                Redis              Ollama
       │                   │
       └───────────┬───────┘
                   │
              SQL Server
```

---

# 6. Application Modules

The application should contain these major modules:

```text
Authentication
User Management
Dashboard
Document Management
Document Editor
Real-Time Collaboration
Presence
Comments
Sharing & Permissions
Versioning
Revision History
Offline Synchronization
Document Locking
AI Assistant
Notifications
Audit Logs
Administration
```

---

# 7. Authentication

## Requirements

Users should be able to:

* Register
* Login
* Logout
* Refresh session
* Update profile
* Change password

## Authentication

Use:

```text
JWT Access Token
+
Refresh Token
```

## Security

* Password hashing
* Token expiration
* Refresh-token rotation
* Authentication middleware
* Authorization policies

---

# 8. Dashboard

After login:

```text
┌─────────────────────────────────────────────┐
│ My Documents                    [+ New]     │
├─────────────────────────────────────────────┤
│ Search documents...                         │
├─────────────────────────────────────────────┤
│                                             │
│ Project Proposal       Edited 2 min ago     │
│ Architecture Design    Edited yesterday     │
│ Meeting Notes           Edited Aug 15       │
│                                             │
└─────────────────────────────────────────────┘
```

## Features

* Create document
* Open document
* Rename
* Delete
* Duplicate
* Archive
* Search
* Sort
* Recent documents
* Shared documents

---

# 9. Document Management

## Create Document

Default:

```text
Untitled Document
```

Properties:

```text
DocumentId
Title
OwnerId
Status
CreatedAt
UpdatedAt
CurrentVersion
```

## Actions

* Create
* Rename
* Duplicate
* Archive
* Delete
* Restore
* Share

---

# 10. Document Editor

Use:

**Tiptap / ProseMirror**

## Formatting

Support:

* Bold
* Italic
* Underline
* Strike
* Headings
* Paragraphs
* Bullet lists
* Numbered lists
* Checklists
* Links
* Code blocks
* Blockquotes
* Tables
* Images
* Horizontal rules

---

# 11. Collaborative Editing

This is the core system.

Use:

```text
Tiptap
   ↓
Yjs
   ↓
SignalR
   ↓
ASP.NET Core
   ↓
Redis
```

Multiple users should be able to edit the same document simultaneously.

Example:

```text
User A → Hello World
User B → Hello Aditya
User C → Hello Developer
```

The system must reconcile concurrent changes without simply overwriting another user's changes.

---

# 12. CRDT Requirement

Use **Yjs CRDT** instead of implementing the complete CRDT algorithm yourself.

Yjs is responsible for:

* Document state
* Concurrent modifications
* Conflict resolution
* Synchronization state
* Offline changes
* Merging changes

ASP.NET Core is responsible for:

* Authentication
* Authorization
* Transport
* Persistence
* Collaboration sessions
* Document lifecycle

---

# 13. SignalR

Create:

```text
DocumentHub
```

## Hub operations

```text
JoinDocument
LeaveDocument
SendDocumentUpdate
SendAwarenessUpdate
SendCursorUpdate
SendSelectionUpdate
```

## Events

```text
DocumentChanged
UserJoined
UserLeft
PresenceChanged
CursorChanged
SelectionChanged
DocumentLocked
DocumentUnlocked
```

---

# 14. Presence

Display active users:

```text
● Aditya     Editing
● Rahul      Editing
● Priya      Viewing
```

Presence should include:

* User name
* Avatar
* Online/offline
* Current activity
* Cursor
* Selection
* Last active time

Presence data should primarily be stored in **Redis/in-memory**, not SQL Server.

---

# 15. Concurrent User Capacity

Initial target:

**20–50 simultaneous users per document.**

Architecture target:

**100+ concurrent users per document after optimization and horizontal scaling.**

The system should not have a hard architectural dependency on a specific user count.

---

# 16. Autosave

The editor should continuously synchronize changes.

However, SQL Server must **not receive a database write for every keystroke**.

Use:

```text
User Edit
   ↓
Yjs Update
   ↓
SignalR
   ↓
Realtime Clients
   ↓
Debounced Persistence
   ↓
SQL Server
```

Persist changes in batches.

---

# 17. Offline Editing

When internet connectivity is lost:

```text
User continues editing
        ↓
Yjs local state
        ↓
Internet reconnects
        ↓
Pending changes synchronized
        ↓
CRDT merge
        ↓
Final consistent state
```

Requirements:

* Local document persistence
* Detect connection loss
* Continue editing
* Queue updates
* Reconnect automatically
* Synchronize pending updates
* Resolve concurrent changes

---

# 18. Versioning

Create document versions at meaningful points.

Example:

```text
Version 1
Document created

Version 2
Major content update

Version 3
AI rewrite

Version 4
User changes

Version 5
Restored revision
```

Version information:

```text
VersionId
DocumentId
VersionNumber
CreatedBy
CreatedAt
ChangeType
SnapshotId
```

---

# 19. Revision History

UI:

```text
Revision History

Today
 ├── Aditya edited Introduction
 ├── Rahul modified Architecture
 └── AI rewrote Conclusion

Yesterday
 └── Aditya created document
```

Actions:

* View
* Compare
* Restore

---

# 20. Document Snapshots

Periodically create complete document snapshots.

Example:

```text
Snapshot 1
   ↓
Changes
   ↓
Changes
   ↓
Snapshot 2
```

This prevents the system from having to reconstruct a document from thousands of individual changes.

---

# 21. Collaborative Undo/Redo

Support:

* Undo
* Redo
* Local edits
* Collaborative edits
* AI-generated edits

AI operations should be inserted into the same collaborative document state.

Therefore:

```text
AI generated content
       ↓
User accepts
       ↓
Yjs transaction
       ↓
SignalR
       ↓
Other collaborators
```

---

# 22. Comments

Users can select text and create a comment.

Example:

```text
"This architecture should use Redis here."

        ↓

Aditya:
"This architecture should use Redis here."
                  └── 💬 Rahul:
                      "Agreed."
```

Features:

* Add comment
* Reply
* Edit
* Delete
* Resolve
* Reopen
* Mention users

---

# 23. Comment Mentions

Support:

```text
@Aditya
@Rahul
@Priya
```

Mentioned users should receive a notification.

---

# 24. Document Sharing

Add a **Share** button.

```text
[ Share ]
```

Dialog:

```text
Share Document

Add people:
[ rahul@gmail.com ]

Permission:
[ Editor ▼ ]

[ Send Invitation ]
```

---

# 25. Invitation System

### Existing User

```text
Owner
 ↓
Enter email
 ↓
Find user
 ↓
Create DocumentMember
 ↓
Send notification
```

### New User

```text
Owner
 ↓
Enter email
 ↓
Create invitation
 ↓
Generate secure token
 ↓
Invitation link
 ↓
Register/Login
 ↓
Accept invitation
 ↓
DocumentMember created
```

---

# 26. Invitation Table

Create:

```text
DocumentInvitations
```

Fields:

```text
Id
DocumentId
InvitedBy
Email
Role
TokenHash
ExpiresAt
AcceptedAt
CreatedAt
```

Invitation tokens must be:

* Cryptographically random
* Hashed before storage
* Expirable
* Single-use after acceptance

---

# 27. Permissions

Roles:

```text
Owner
Editor
Commenter
Viewer
```

### Owner

Everything.

### Editor

* Edit
* Comment
* Use AI

### Commenter

* View
* Comment

### Viewer

* View only

Backend must enforce all permissions.

---

# 28. Share Links

Support:

```text
Anyone with link → Viewer
Anyone with link → Editor
```

Owner can disable the link.

Use secure random tokens.

Never use predictable document IDs as access tokens.

---

# 29. Document Locking

Support:

### Soft Lock

Informational:

```text
Rahul is currently editing this section.
```

### Hard Lock

```text
Document Locked

Only Aditya can edit this document.
```

Lock information:

```text
LockedBy
LockedAt
ExpiresAt
LockType
```

---

# 30. AI Assistant

AI should be available inside the editor.

Toolbar:

```text
✨ AI

Generate
Rewrite
Improve
Summarize
Expand
Shorten
Fix Grammar
Change Tone
Continue
```

---

# 31. AI Write Workflow

User selects:

```text
We need better customer support.
```

Then:

```text
AI → Improve
```

AI generates:

```text
We need to improve our customer support
experience to provide faster and more effective
assistance to customers.
```

Actions:

```text
[ Accept ]
[ Reject ]
[ Regenerate ]
```

---

# 32. AI Architecture

Use local Ollama:

```text
React
 ↓
ASP.NET Core
 ↓
AI Service
 ↓
Ollama
 ↓
Local LLM
 ↓
Response
 ↓
User Accepts
 ↓
Yjs Transaction
 ↓
SignalR
```

The AI should **not directly modify SQL Server**.

---

# 33. AI Features — Phase 1

Implement:

* Generate
* Rewrite
* Improve
* Summarize
* Expand
* Shorten
* Grammar correction
* Continue writing
* Tone conversion

---

# 34. AI Features — Phase 2

Implement:

* Ask about document
* Document summarization
* Generate outline
* Generate action items
* Generate meeting notes

---

# 35. AI RAG

Optional advanced feature:

```text
Document
   ↓
Chunking
   ↓
Embeddings
   ↓
Qdrant
   ↓
Relevant chunks
   ↓
Ollama
   ↓
Answer
```

This allows:

> "What are the main risks mentioned in this document?"

---

# 36. Database

Core tables:

```text
Users
Documents
DocumentMembers
DocumentInvitations

DocumentVersions
DocumentSnapshots
DocumentChanges

Comments
CommentReplies
CommentMentions

AIRequests

RefreshTokens
AuditLogs
```

Optional:

```text
DocumentLocks
Notifications
UserSessions
```

---

# 37. Database Relationships

```text
Users
 │
 ├── Documents
 │      │
 │      ├── DocumentMembers
 │      ├── DocumentInvitations
 │      ├── DocumentVersions
 │      ├── DocumentSnapshots
 │      ├── DocumentChanges
 │      ├── Comments
 │      ├── AIRequests
 │      └── AuditLogs
 │
 └── RefreshTokens
```

---

# 38. Redis Responsibilities

Redis should handle temporary/distributed state:

```text
Presence
Active Connections
SignalR Backplane
Distributed Locks
Temporary Collaboration State
```

SQL Server remains the durable source of truth.

---

# 39. REST API

### Authentication

```text
POST /api/auth/register
POST /api/auth/login
POST /api/auth/refresh
POST /api/auth/logout
```

### Documents

```text
GET    /api/documents
POST   /api/documents
GET    /api/documents/{id}
PUT    /api/documents/{id}
DELETE /api/documents/{id}
POST   /api/documents/{id}/duplicate
```

### Members

```text
GET    /api/documents/{id}/members
POST   /api/documents/{id}/members
PUT    /api/documents/{id}/members/{userId}
DELETE /api/documents/{id}/members/{userId}
```

### Invitations

```text
POST /api/documents/{id}/invitations
POST /api/invitations/{token}/accept
POST /api/invitations/{token}/reject
```

### Versions

```text
GET  /api/documents/{id}/versions
GET  /api/documents/{id}/versions/{versionId}
POST /api/documents/{id}/versions/{versionId}/restore
```

### Comments

```text
GET    /api/documents/{id}/comments
POST   /api/documents/{id}/comments
PUT    /api/comments/{id}
DELETE /api/comments/{id}
POST   /api/comments/{id}/resolve
```

### AI

```text
POST /api/ai/generate
POST /api/ai/rewrite
POST /api/ai/summarize
POST /api/ai/improve
POST /api/ai/continue
```

---

# 40. SignalR API

Hub:

```text
/hubs/document
```

Client → Server:

```text
JoinDocument
LeaveDocument
SendDocumentUpdate
SendAwarenessUpdate
SendCursorUpdate
```

Server → Client:

```text
DocumentChanged
AwarenessChanged
CursorChanged
UserJoined
UserLeft
DocumentLocked
DocumentUnlocked
```

---

# 41. Security

Implement:

* JWT authentication
* Authorization policies
* Document-level authorization
* Permission validation
* Secure invitation tokens
* Password hashing
* Refresh-token rotation
* Rate limiting
* CORS
* Input validation
* Audit logging
* SignalR authorization
* AI endpoint authorization

Never trust frontend permissions.

---

# 42. Error Handling

Standard API response:

```json
{
  "success": false,
  "message": "You do not have permission to edit this document.",
  "code": "DOCUMENT_ACCESS_DENIED"
}
```

Use global ASP.NET Core exception handling.

---

# 43. Logging

Use structured logging.

Log:

```text
User login
Document opened
User joined document
User left document
Document changed
Version created
Permission changed
Invitation created
AI request
Document restored
Lock acquired
Lock released
```

Do not log:

* Passwords
* JWT tokens
* Invitation tokens
* Sensitive document content unnecessarily

---

# 44. Testing

## Backend

Use:

* xUnit
* Moq
* ASP.NET Core integration tests

Test:

* Authentication
* Authorization
* Document CRUD
* Permissions
* Invitations
* Versioning
* Comments
* AI service

## Frontend

Use:

* Vitest
* React Testing Library

Test:

* Editor
* Collaboration state
* Comments
* Sharing
* AI UI

---

# 45. Critical Collaboration Tests

These are extremely important for the project.

### Test 1

Two users edit the same paragraph simultaneously.

### Test 2

Five users edit different parts simultaneously.

### Test 3

Two users modify the same text while disconnected.

### Test 4

One user disconnects during editing.

### Test 5

User reconnects with pending offline changes.

### Test 6

AI generates content while another user edits the document.

### Test 7

Document is locked while users are editing.

### Test 8

User loses permission while connected.

### Test 9

Version is restored while multiple users are connected.

---

# 46. Performance Requirements

Initial targets:

```text
20–50 concurrent editors/document
<100–200ms perceived collaboration latency
Automatic reconnection
No lost CRDT updates
```

Later target:

```text
100+ concurrent users/document
Horizontal SignalR scaling
Redis backplane
```

---

# 47. Docker Architecture

Development environment:

```text
docker-compose.yml

services:

frontend
backend
sqlserver
redis
ollama
qdrant
```

Initially you can run React and ASP.NET Core directly and use Docker only for infrastructure.

---

# 48. Development Phases

## Phase 1 — Foundation

```text
React
ASP.NET Core
SQL Server
Authentication
Document CRUD
```

## Phase 2 — Editor

```text
Tiptap
Formatting
Autosave
Undo/Redo
```

## Phase 3 — Collaboration

```text
Yjs
SignalR
Real-time editing
Presence
Cursor sharing
```

## Phase 4 — Distributed Collaboration

```text
Redis
Reconnection
Offline editing
Conflict resolution
Distributed state
```

## Phase 5 — Document Management

```text
Comments
Mentions
Sharing
Invitations
Permissions
Locking
```

## Phase 6 — Versioning

```text
Snapshots
Versions
Revision history
Compare
Restore
```

## Phase 7 — AI

```text
Ollama
AI writing
Rewrite
Summarize
Generate
Accept/Reject
```

## Phase 8 — RAG

```text
Embeddings
Qdrant
Document Q&A
```

## Phase 9 — Production Engineering

```text
Load testing
Horizontal scaling
Redis SignalR backplane
Monitoring
Docker
Security hardening
```

---

# 49. Recommended React Project Structure

```text
src/
├── app/
├── components/
│   ├── editor/
│   ├── collaboration/
│   ├── comments/
│   ├── sharing/
│   ├── presence/
│   └── ai/
│
├── pages/
│   ├── auth/
│   ├── dashboard/
│   └── document/
│
├── hooks/
│   ├── useDocument.ts
│   ├── useCollaboration.ts
│   ├── usePresence.ts
│   └── useAI.ts
│
├── services/
│   ├── api.ts
│   ├── auth.service.ts
│   ├── document.service.ts
│   ├── collaboration.service.ts
│   └── ai.service.ts
│
├── store/
│   ├── auth.store.ts
│   ├── document.store.ts
│   └── collaboration.store.ts
│
├── types/
└── utils/
```

---

# 50. Recommended ASP.NET Core Structure

```text
src/
├── Api/
│   ├── Controllers/
│   ├── Hubs/
│   ├── Middleware/
│   └── Extensions/
│
├── Application/
│   ├── Services/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Validators/
│
├── Domain/
│   ├── Entities/
│   ├── Enums/
│   └── ValueObjects/
│
├── Infrastructure/
│   ├── Persistence/
│   ├── Redis/
│   ├── AI/
│   └── Authentication/
│
└── Tests/
```

---

# 51. Important Architectural Rule

Keep these responsibilities separate:

```text
                    ┌─────────────────┐
                    │     React       │
                    │       UI        │
                    └────────┬────────┘
                             │
                       Tiptap + Yjs
                             │
                             ▼
                    ┌─────────────────┐
                    │    SignalR      │
                    │ Real-time layer │
                    └────────┬────────┘
                             │
                    ┌────────┴────────┐
                    │ ASP.NET Core API │
                    └───────┬─┬───────┘
                            │ │
                       Redis  │ SQL
                            │ │
                            ▼ ▼
                    Temporary Durable
                       State   State
```

**Yjs = collaboration/conflict resolution**

**SignalR = real-time transport**

**Redis = distributed temporary state**

**SQL Server = durable application/document state**

**ASP.NET Core = business logic/security/API**

**Ollama = local AI**

This separation is the key architectural design of the entire project.

---

# 52. MVP Definition

Your first usable version should contain only:

```text
✓ Registration/Login
✓ Document CRUD
✓ Rich text editor
✓ Real-time multi-user editing
✓ Yjs CRDT
✓ SignalR
✓ Cursor/presence
✓ Autosave
✓ Sharing
✓ Permissions
✓ Comments
✓ Version history
✓ Offline editing
✓ AI Write
✓ AI Rewrite
✓ Undo/Redo
```

Then add (Not Considered For Now):

```text
→ Redis scaling
→ Document locking
→ Revision comparison
→ AI RAG
→ Qdrant
→ Load testing
→ Horizontal scaling
→ Advanced audit system
```

This scope gives you a **serious distributed .NET portfolio project**, rather than another standard CRUD application.
