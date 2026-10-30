# VR Virtual Library for Interactive Learning

**CIAP (Community / Industry Action Project) — College Submission**  
**Primary SDG:** SDG 4 — Quality Education  
**Secondary SDG:** SDG 10 — Reduced Inequalities  

---

## 📖 Project Overview

The **VR Virtual Library for Interactive Learning** is an immersive, accessible 3D educational environment developed in Unity. Designed to democratize access to quality educational resources, it enables learners to explore a realistic, serene library space, discover curated books across five major disciplines (Programming, Natural Sciences, Mathematics, World Literature, and General Knowledge), interact with learning kiosks, and follow directional waypoints directly to bookshelf coordinates.

The system features dual-mode input architecture:
1. **Desktop Mode:** Full first-person keyboard and mouse exploration (`WASD`, Mouse Look, `E` to interact, `Tab` for catalog).
2. **VR Mode:** Native OpenXR / XR Interaction Toolkit integration with spatial teleportation, snap turning, and controller ray interaction for headsets such as Meta Quest, HTC Vive, and Windows Mixed Reality.

---

## 🏛️ Virtual Environment Features

- **Architectural Room Construction:** Fully enclosed, proportioned library hall (16m × 24m × 4.5m) featuring parquet wood flooring, acoustic ceiling tiles, navy central carpet runner, and classical pillars.
- **Physical Bookshelves:** Four dedicated double-sided bookshelves organizing educational volumes by domain:
  - *Bookshelf 1 (West - North):* Computing & Software Architecture
  - *Bookshelf 2 (West - South):* Natural Sciences & Cosmology
  - *Bookshelf 3 (East - North):* Advanced Mathematics & Problem Solving
  - *Bookshelf 4 (East - South):* World Literature & Social Civics
- **Reception & Search Kiosk:** Central counter housing a digital computer catalog terminal and display racks for General Knowledge and memoirs.
- **Study Lounge:** Central wooden study table with 4 study chairs, interactive reading lamp with toggleable warm illumination, and quiet reading corners.
- **Dynamic Wayfinding:** Selecting "Locate on Shelf" from any book or search result places a persistent 3D destination indicator with real-time distance calculations, guiding learners directly to the book's physical shelf.
- **Acoustic Environment:** Procedural ambient sound synthesis reproducing warm library room tone, subtle ventilation hum, and realistic paper rustle cues without external media dependencies.

---

## 🎮 Controls & Interaction Guide

### Desktop Mode
| Key / Input | Action |
| :--- | :--- |
| **`W`, `A`, `S`, `D`** | Move Forward / Backward / Strafe |
| **`Mouse Move`** | Look around (Pitch & Yaw) |
| **`Left Shift`** | Sprint / Accelerate Locomotion |
| **`E`** or **`Left Mouse Click`** | Inspect Book / Use Catalog Kiosk / Toggle Desk Lamp |
| **`Tab`** or **`M`** | Open / Close Search & Category Filter Catalog |
| **`Escape`** | Back / Close Active Dialog / Return to Menu |

### Virtual Reality Mode (OpenXR / XR Interaction Toolkit)
| Controller Input | Action |
| :--- | :--- |
| **Left Thumbstick** | Smooth Locomotion / Teleportation |
| **Right Thumbstick (Horizontal)** | Snap Turn (45° increments for comfort) |
| **Right Hand Controller Ray + Trigger** | Select Book / Press UI Buttons / Toggle Lights |
| **Controller Menu Button** | Open Catalog Modal |

---

## 📁 Repository & Project Layout

```
arvr_ciap/
├── Assets/
│   ├── Materials/                       # 16 PBR Materials (Mahogany, Oak, Parquet, Metals, etc.)
│   ├── Prefabs/                         # Standard Unity prefabs for Books, Shelves, Terminals
│   │   ├── Books/
│   │   ├── Furniture/
│   │   └── UI/
│   ├── Scenes/
│   │   └── VirtualLibrary.unity         # Complete 3D Library Scene
│   └── Scripts/
│       ├── Audio/
│       │   └── ProceduralLibraryAmbience.cs
│       ├── Core/
│       │   ├── AudioManager.cs          # Procedural sound effect synthesizer
│       │   ├── GameConstants.cs
│       │   ├── LibraryEnums.cs          # BookCategory, ReadingDifficulty, InteractionState
│       │   └── LibraryManager.cs        # Central state, event bus, and wayfinding coordinator
│       ├── Data/
│       │   ├── BookData.cs              # Educational metadata schema with SDG tags
│       │   └── BookDatabase.cs          # Master catalog of 15 curated educational books
│       ├── Interaction/
│       │   ├── BookInteractable.cs      # 3D book hover highlight, scale pulse, and inspect trigger
│       │   ├── CatalogTerminalInteractable.cs # Interactive digital kiosk
│       │   ├── DoorController.cs        # Interactive swinging door
│       │   ├── IInteractable.cs         # Common interaction contract
│       │   ├── RaycastInteractor.cs     # Center-reticle & controller raycaster
│       │   └── ReadingLampInteractable.cs # Interactive reading lamp with light toggle
│       ├── Player/
│       │   ├── FirstPersonController.cs # CharacterController physics, WASD, mouse-look
│       │   ├── HeadBob.cs               # Sinusoidal motion synchronization
│       │   └── VRRigAdapter.cs          # Auto-detection between Desktop and VR rigs
│       └── UI/
│           ├── BookDetailPanelUI.cs     # Comprehensive book modal (takeaways, SDG impact, locator)
│           ├── BookSearchResultItemUI.cs # Catalog list card component
│           ├── InteractionHUD.cs        # Reticle, interaction prompts, and waypoint HUD
│           ├── LibraryCatalogUI.cs      # Real-time search and 5-category filter modal
│           ├── UIController.cs          # View stack transitions and modal manager
│           └── WelcomeScreenUI.cs       # Portal screen with title, SDG goals, and controls
├── Packages/
│   └── manifest.json                    # Unity package dependencies (URP, XR Toolkit, OpenXR)
├── ProjectSettings/                     # ProjectVersion.txt, TagManager.asset, PlayerSettings.asset
├── Tests/
│   └── StandaloneVerification/          # C# test harness and mocks for automated testing
│       ├── UnityEngineMocks.cs
│       └── VirtualLibraryTestHarness.cs
├── Assembly-CSharp.dll                  # Pre-compiled assembly validating 100% code syntax
├── VirtualLibraryTests.exe              # Automated test runner (15/15 tests passing)
├── CIAP_PROJECT_SUMMARY.md              # Academic SDG alignment, design analysis, and impact
├── TESTING_REPORT.md                    # Formal test execution logs and verification summary
└── README.md                            # Main project documentation
```

---

## 🚀 How to Launch in Unity

1. Open **Unity Hub** (version 3.0 or higher recommended).
2. Click **Add project from disk** and select this directory: `c:\Users\Meet Katarmal\OneDrive\Desktop\arvr_ciap`.
3. Select **Unity 2022.3 LTS** (or compatible 2021.3+ / 2023.x version).
4. Once Unity opens the project:
   - Navigate to `Assets/Scenes/` in the Project window.
   - Double-click `VirtualLibrary.unity` to open the scene.
5. Press the **Play** button at the top of the Unity Editor.
6. The Welcome Screen will appear. Click **"Start Exploring"** to begin walking through the virtual library.

---

## 🧪 Automated Testing

A standalone test suite is included in the root directory. To run automated verification without launching Unity:
```powershell
.\VirtualLibraryTests.exe
```
This tests master catalog schema completeness, category filters, substring query algorithms, waypoint arrival thresholds, raycast math, and acoustic synthesis stability.

