<div align="center">

# ⚖️ Guess The Weight

**A physics balance-scale puzzle game where you guess what a mystery food weighs, using only the food you can see.**

Made solo in 2 days for **[Kenney Jam 2026](https://itch.io/jam/kenney-jam-2026)** (theme: *Scale*) · Unity 6 · C# · URP

[**▶ Play in your browser on itch.io**](https://tsuki-0505.itch.io/guess-the-weight) · [**🎬 Watch the demo**](https://github.com/user-attachments/assets/9d8aa6ac-fc35-4ecd-92e5-2dda7f136a46)

![Gameplay](<img width="800" height="450" alt="gameplay" src="https://github.com/user-attachments/assets/db002d8c-29e1-47d7-aa25-e722bb9ba0ae" />
)

</div>

---

## About

A mystery food drops onto the left pan of a balance scale. You have a shelf of food with known weights and a scale that never lies. Load food onto the pans, watch which way the beam tips, narrow down the range, then type in your best guess in grams.

The closer you get, the more stars you earn. Earn at least two stars to unlock the next stage.

<p align="center">
  <img src="<img width="1600" height="900" alt="title" src="https://github.com/user-attachments/assets/183d21e2-af35-47b7-8d6a-3ad38169f000" />
" width="49%" alt="Title screen">
  <img src="**<img width="1600" height="900" alt="hover" src="https://github.com/user-attachments/assets/cd820dd9-9af1-4abc-b17f-fcd3e3de11a3" />
**" width="49%" alt="Hovering a food on the shelf shows its weight">
  <img src="<img width="1600" height="900" alt="twin" src="https://github.com/user-attachments/assets/97027c17-60f2-42bd-a3d1-46b994472e45" />
" width="49%" alt="Stage 5: two mystery croissants on the left pan">
  <img src="<img width="1600" height="900" alt="perfect" src="https://github.com/user-attachments/assets/e58f31b9-3c3c-4b62-9c5b-ef269431c875" />
" width="49%" alt="Perfect guess: blue stars">
</p>

## How to play

| Input | Action |
|---|---|
| **Mouse hover** | Select a food on the shelf and see its weight |
| **E** | Place the selected food on the **right** pan (or take it back off) |
| **F** | Place the selected food on the **left** pan (or take it back off) |
| **Type + Submit Guess** | Lock in your answer in grams |

**Tip:** bracket the answer. If 140 g on the right pan is too light and 160 g is too heavy, the mystery food is somewhere in between.

## Features

- **A scale that runs on physics.** The beam is a `HingeJoint` with a spring and angle limits. Each food's `Rigidbody.mass` comes from its weight in grams, so heavier loads tip the beam further.
- **15 handcrafted stages.** The mystery food goes from a 20 g slice of cheese to a 4 kg turkey, and the shelf changes as the weights grow.
- **A new answer every attempt.** The mystery weight re-rolls to a random whole number within ±10% of its base value each time you play a stage, so you can't memorize answers.
- **Twin-mystery stages.** Some stages put two copies of the mystery food on the pan, and you still guess the weight of one.
- **Star scoring.** You earn stars based on how far off your guess is. An exact guess turns the stars blue ("Wow, you got the perfect weight, are you cheating?").
- **Save and continue.** Progress and total stars are saved, and the title screen lets you start a new game or continue.
- **Juice.** Typewriter tutorial pop-ups, hover scaling, a mouse-driven camera sway, wood-creak and placement sounds, and a success chime.
- **Ready for the web.** Menus account for WebGL (the Quit button is hidden in the browser, since a web page can't close itself).

### Scoring

| Stars | Guess within |
|---|---|
| 🔵🔵🔵 | Exact weight |
| ⭐⭐⭐ | 10% of the actual weight |
| ⭐⭐ | 20% |
| ⭐ | 30% |
| — | more than 30% off ("Are you even trying?") |

All thresholds, along with the number of stars needed to advance (2), can be changed in the Inspector on `GameManager`.

### Stage progression

| Stage | Mystery food | Base weight | | Stage | Mystery food | Base weight |
|---|---|---|---|---|---|---|
| 1 | Cheese | 20 g | | 9 | Pineapple | 800 g |
| 2 | Strawberry | 70 g | | 10 | Corn | 500 g |
| 3 | Avocado | 150 g | | 11 | Loaf | 600 g |
| 4 | Banana | 120 g | | 12 | Coconut ×2 | 1.2 kg |
| 5 | Croissant ×2 | 225 g | | 13 | Pumpkin | 2.5 kg |
| 6 | Apple | 180 g | | 14 | Turkey | 4 kg |
| 7 | Burger | 300 g | | 15 | Fish ×2 | 3 kg |
| 8 | Eggplant | 450 g | | | | |

## Technical overview

The game is **data-driven**. Each stage is a `LevelData` ScriptableObject, so you can add or rebalance a stage in the editor without touching code.

```
LevelData (ScriptableObject)
 ├─ shelfItems[]      → ShelfItemOption { displayName, prefab, weightGrams, mysteryVariance }
 ├─ mysteryItem       → ShelfItemOption
 ├─ mysteryCount      → how many copies sit on the left pan
 └─ showInstruction / instructionText
```

| Script | Responsibility |
|---|---|
| `GameManager` | Stage flow, guess validation, star scoring, result pop-up, progression gating, saving |
| `ShelfSpawner` | Builds each stage from `LevelData`: fills shelf slots, writes the weight legend, spawns the randomized mystery item(s) |
| `ShelfSlot` | Spawns a shelf item and respawns it when it gets picked up |
| `ItemSelector` | Mouse raycast hover selection, the hover tooltip, and E/F placement and removal on the pans (New Input System) |
| `ScalePan` | Reserves and releases fixed item slots on each pan |
| `WeighableItem` | Turns grams into `Rigidbody.mass` (`mass = grams × gramsToMass`) |
| `ChainVisual` | `LineRenderer` chains that follow the swinging pans |
| `CameraLookController` | Subtle camera sway that follows the mouse |
| `TypewriterText` | Character-by-character tutorial text with per-character tick sounds |
| `SaveSystem` | Saves and loads stage index and total stars with `PlayerPrefs` |
| `MenuNavigator` / `EndingScreenNavigator` | Animated title and ending menus, with WebGL-specific handling |
| `CenterMeshPivot` | Editor utility (context menu) that re-centres imported mesh pivots |

### Scenes

`TitleScreen` → `GameplayScene` → `EndingScene`

## Getting started

1. Install **Unity 6000.0.78f1** (Unity 6) through Unity Hub.
2. Clone the repo:
   ```bash
   git clone https://github.com/tsuki9355/Guess-The-Weight-Kenney-Jam-2026-.git
   ```
3. Open the folder in Unity Hub (**Add → Add project from disk**).
4. Open `Assets/Scenes/TitleScreen.unity` and press **Play**.

### Adding a stage

1. Right-click in `Assets/Levels` and choose **Create → Guess The Weight → Level**.
2. Fill in the shelf items, the mystery item, and the copy count.
3. Add the asset to the `levels` list on the `GameManager` in `GameplayScene`.

## Project structure

```
Assets/
├─ Scenes/       TitleScreen, GameplayScene, EndingScene
├─ Scripts/      Gameplay, UI and save code (C#)
├─ Levels/       Level_01 … Level_15 (LevelData assets)
├─ Prefab/       16 food prefabs (Rigidbody + WeighableItem + DraggableItem)
├─ Physics/      HighFriction physic material
├─ Sounds/       Music and SFX
└─ GameAssets/   Kenney asset packs
```

## Credits

- **3D models, UI, fonts and cursors:** [Kenney](https://kenney.nl): Food Kit, Mini Market, UI Pack, UI Pack – Sci-Fi, Fantasy UI Borders, Cursor Pack, Kenney Fonts. All CC0.
- **Music:** "Puzzle Game – Bright Casual Video Game Music" by cyberwave-orchestra (Pixabay)
- **Sound effects:** audley_fergine, freesound_community, lesiakower, litupsubway, soundshelfstudio, yodguard (Pixabay)
- Made solo by **Angus Chou** ([@tsuki9355](https://github.com/tsuki9355) · [tsuki_0505 on itch.io](https://tsuki-0505.itch.io/guess-the-weight))
