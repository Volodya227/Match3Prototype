Match-3 Core Engine (Unity)
Custom Match-3 implementation focused on deterministic state management, layered architecture, and extensible grouping logic.
This project was built without gameplay frameworks. All core mechanics (grouping, gravity, state updates) are implemented manually.

Architecture
The project follows a layered design:
Model — pure game logic (no MonoBehaviour)
View — rendering and animation layer
Controller — orchestrates update pipeline
Model and View are decoupled via events.
Controller
→ Model (state & logic)
→ View (animation & rendering)
The Model layer is fully independent of Unity lifecycle.

Game State Design
The board state consists of:
Item objects (position, type, color, state)
2D grid index for fast lookup

Invariant:
grid[x, y] always matches Item.X/Y.
All item mutations go through centralized state methods to maintain consistency.
The system avoids hidden magic and relies on explicit state transitions.

Update Pipeline
Game state evolves in discrete ticks.
Each tick performs:
Resolve matches (group detection)
Delete matched items
Apply gravity
Spawn new items
Unlock grouping
This ensures deterministic and predictable board evolution.
Grouping System
Grouping is implemented using a flood-fill algorithm.

Features:
Configurable direction modes:
Horizontal
Vertical
Diagonal

Minimum group size configurable
Local visited tracking during grouping pass
Grouping logic is isolated from movement logic.
Item Movement
Item switching is validated via allowed direction modes.
State mutation includes:
Grid index update
Item position update
Event dispatch for animation layer
Movement logic is separated from grouping logic.
View Layer

The View layer:
Subscribes to Model events
Animates movement via DOTween
Does not mutate game state
Visual behavior reacts to Model changes only.
Extensibility

The architecture supports:
Alternative grouping rules
Custom item behaviors (e.g., bomb logic)
Different movement constraints
Additional rule systems
The system is designed to allow expansion without rewriting core state logic.

Tech Stack
Unity
C#
DOTween
Custom state engine (no third-party gameplay framework)

Design Focus
This project emphasizes:
Deterministic state control
Separation of concerns
Explicit mutation handling
Clean update pipeline
The goal was to implement a transparent Match-3 core system rather than rely on built-in mechanics.
