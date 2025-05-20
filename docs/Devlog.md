# WhileIterating RPG Devlog

**Day 1: Environment Setup and Basic Design – 18 May, 2025**

- Installed Godot 4.4.1 Mono
- Set up .NET 8 SDK
- Verified C# script runs inside Godot
- Configured Git, added .gitignore, made first commit
- Created base folders: /Scenes, /Scripts, /Assets, /UI
- Finalized core loop, races, classes, stat system, and world design
- Gained deeper understanding of Godot scenes, physics process, and script structure
- Ready to start player movement and time system tomorrow

**Day 2: Character Movement, Starting Area, and Early Dialogue – May 19, 2025**

## ✅ Progress Made
- Designed the **starting area** of the game, including terrain layout, environment objects, and player spawn placement.
- Added assets (sprites, terrain, etc.) to assets folder.
- Successfully implemented a **fully controllable player character**, complete with 4-directional movement and animation using `AnimatedSprite2D`.
- Set up the **Camera2D** to follow the player.
- Built the foundation of the **class selection system** using interactable items (e.g., a Short Sword).
- Implemented a `CanvasLayer`-based **dialogue UI system** using a `Panel` and `Label`.
- Wrote a reusable `DialogUI.cs` script to handle timed messages via `ShowMessage()`.
- Confirmed all internal values (text, visibility, position) were updating as expected.

## 🧱 Challenges
- Despite correct setup, the **dialogue UI wouldn't render in-game**.
- Troubleshot potential issues with:
  - Layout anchors and positioning
  - Text visibility and color overrides
  - Missing font assignments
  - Z-layering and rendering order
  - Viewport scaling/stretch modes
- After extensive debugging, decided to **scrap and rebuild** the dialogue UI from scratch to ensure clean behavior.

## 🔜 Next Steps
- Rebuild the dialogue UI from a clean scene using minimal structure.
- Confirm the panel anchors properly to the top of the screen and stays fixed regardless of camera movement.
- Add polish: fade-in/out, typewriter animation, or branching text.
- Fully connect **item pickup (sword/tome)** to **class assignment** logic.

## 💭 Reflection
> Even though the dialogue box didn’t work out today, I made solid foundational progress: the player moves, the world is built, and the systems are coming together. Sometimes starting over is faster than untangling a UI issue—and now I know exactly how to rebuild it cleaner. Tomorrow’s gonna be a win.
