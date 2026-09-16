# RaceMania

A competitive top-down arcade racing game built with Unity and C#. RaceMania features local co-op multiplayer, allowing two players to race head-to-head on a single keyboard. 

## Features

* **Local Co-Op Multiplayer:** Engineered a shared-keyboard input system (`Driver.cs`, `Driver2.cs`) that processes independent inputs for two players racing simultaneously on a single machine.
* **Physics & Handling:** Custom 2D vehicle physics offering distinct acceleration, steering, and braking profiles.
* **Track Boundaries & Hazards:** Dynamic speed modifiers (`RoadSpeedModifier.cs`) that seamlessly alter vehicle physics when driving off-road or hitting track hazards.
* **Robust Lap Validation:** A rigorous checkpoint and lap-counting system (`LapController.cs`, `Checkpoints.cs`) that prevents players from cheating or skipping track segments.
* **Responsive UI:** Real-time race HUD tracking laps, times, and race states managed by a centralized game state controller (`GameManager.cs`).
* **WebGL Support:** Fully optimized for browser-based play using Unity's WebGL build target.

## Technical Architecture

* **Engine:** Unity (2D)
* **Language:** C#
* **Architecture:** Component-based design with specialized MonoBehaviours for vehicle physics, collision detection, and global game state management.
* **UI:** Unity UI (uGUI) & TextMeshPro

## Local Development

1. Ensure you have the [Unity Hub](https://unity.com/download) and the appropriate Unity Editor version installed.
2. Clone this repository.
3. Open Unity Hub, click **Add project from disk**, and select the `RaceMania` folder.
4. Open the main scene located in `Assets/Scenes/`.
5. Press **Play** in the editor to test the game locally.

## Controls

* **Player 1:** `W` `A` `S` `D` (Accelerate, Steer Left, Reverse/Brake, Steer Right)
* **Player 2:** `Up` `Left` `Down` `Right` Arrow Keys (Accelerate, Steer Left, Reverse/Brake, Steer Right)
