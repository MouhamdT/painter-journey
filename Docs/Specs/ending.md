# Spec: Ending

## Goal

The Ending closes the HA02 prototype. It shows the painting as far as the player has painted it (part 1 of 5), says the journey continues, and lets the player go back to the main menu or quit. It's the "end" half of "Main menu and ending" in the Backlog. In the full game, the Ending after the boss shows the finished painting (HLD win condition). In HA02 it shows the progress so far, as decided in Session 3.

## Decisions

| Element | Decision |
|---|---|
| Scene | `Assets/_Game/Scenes/Ending.unity`, the placeholder from the painting spec, filled in. Last in the build list |
| Background | Plain dark (#1E1E1E) |
| Title | "The journey continues…", white, about 64 px at 1920 × 1080, centred about 90 px from the top edge |
| Painting | The framed painting, a little smaller than on the reveal screen so everything fits: 8 wide × 4.8 tall, centred at world (0, 0.8), 5 panels each 1.6 × 4.8, with the dark brown frame (#5A3E2B) 0.25 thick. Panel 1 shows the placeholder landscape (sky blue #7EC8E3 on top, grass green #6DBE5A on the bottom third). Panels 2 to 5 are grey (#6A6A6A). It doesn't animate |
| Progress text | "1 of 5 parts painted", light grey (#BBBBBB), about 36 px, centred just under the painting (about 230 px below the screen centre). The numbers come from `PaintingConfig` (`revealedPart` and `totalParts`) |
| Buttons | "Main Menu" (centre about 300 px below the screen centre) and "Quit" (about 390 px below), each about 300 × 64 px, centred, styled exactly like the main menu's: grey with white text, yellow (#F2D27A) with dark text when selected or hovered |
| Keyboard and mouse | Main Menu is selected when the screen opens. W/S and ↑/↓ move the selection, and Enter presses it. Hovering selects and clicking presses |
| Main Menu | Loads the MainMenu scene |
| Quit | Same as the main menu: closes the game in the build, stops Play Mode in the Editor |
| Config | No new config. It reuses `PaintingConfig` for which panel is coloured and for the "1 of 5" numbers |

## Acceptance criteria

To start here quickly, open `Assets/_Game/Scenes/Ending.unity` and press Play, or play the whole game from the menu.

1. When I open Ending and press Play, I see on a dark background: "The journey continues…" at the top, the framed painting with only the leftmost panel in colour (blue sky, green grass) and the other 4 grey, "1 of 5 parts painted" under it, and the Main Menu and Quit buttons.
2. When the Ending has just opened, Main Menu is yellow (selected) and Quit is grey.
3. With Main Menu selected, when I press S (or ↓), Quit turns yellow. When I press W (or ↑), Main Menu is selected again.
4. With Main Menu selected, when I press Enter, the MainMenu scene loads.
5. With the Ending open, when I click Main Menu with the mouse, the MainMenu scene loads.
6. In the Editor, when I select Quit and press Enter (or click it), Play Mode stops.
7. With the Ending open, when I press A, D, Space, J or Esc, nothing happens.
8. When I play the whole game from MainMenu (Start, kill both enemies, go through the door, watch the reveal, press Space), I arrive at the Ending, and Main Menu brings me back to the start, where Start begins a fresh Room01.
9. With Play Mode stopped, when I set `revealedPart` to 2 on `Assets/_Game/Config/PaintingConfig.asset` and press Play in Ending, the text reads "2 of 5 parts painted". After the test I set it back to 1. (This only checks that the text comes from the config. Which panels are coloured doesn't need to follow it in HA02.)
10. When I resize the Game view to another 16:9 size (for example 1280 × 720), the title, painting, text and buttons stay centred and don't overlap.
11. During all the checks above, the Console shows no errors or warnings, and all checks in the earlier specs still pass.

## Edge cases

- **Pressing Enter twice quickly on Main Menu:** the menu loads once.
- **Space or Enter still held from the painting screen when the Ending loads:** a held key doesn't press a button. Only a new press does.
- **Hovering one button while the other is selected by keyboard:** the hovered one becomes selected, and only one is yellow.
- **Playing again after the Ending:** the new run starts fresh (3 HP, both enemies, a closed door), and the reveal and Ending show part 1 again.

## Examples

- Before: the reveal screen showing its prompt. I do: press Space. After: the Ending, with panel 1 coloured and "1 of 5 parts painted".
- Before: the Ending. I do: press Enter. After: back at the main menu.
- Before: the Ending. I do: press ↓ then Enter. After: in the Editor, Play Mode stops.

## Out of scope

- The finished painting and the real ending after the boss (HA03 and later).
- Credits, a score, a time or statistics.
- Music and sound (HA04).
- Animations, fades and a reveal on this screen.
- Remembering progress between sessions.

## Context for the agent

Read first: `CLAUDE.md`, all specs in `Docs/Specs/`, especially `Docs/Specs/painting-reveal.md` and `Docs/Specs/main-menu.md`, and the current scenes and UI code (`Assets/_Game/Scenes/`, `Assets/_Game/Scripts/UI/`, including `MainMenu.cs` and `PaintingConfig`). Reuse their look, components and button handling instead of writing new versions.

What to create or change:
- Fill in `Assets/_Game/Scenes/Ending.unity`: the painting as SpriteRenderers (the placeholder square), a uGUI Canvas (Screen Space Overlay, reference 1920 × 1080) with TextMeshPro text and two Buttons, and an EventSystem with the Input System UI Input Module, with Main Menu as the first selected object.
- `Assets/_Game/Scripts/UI/EndingScreen.cs` (namespace `PainterJourney.UI`), or extend the menu script if that's cleaner. It sets the progress text from `PaintingConfig` and has Main Menu (`SceneManager.LoadScene("MainMenu")`) and Quit (the same as the menu's) for the buttons' OnClick.

Rules from CLAUDE.md that matter here:
- Scene and asset changes go only through the Unity CLI.
- No `Find*` calls and no singletons.
- Never commit.

The student doesn't play-test. The build session tests every criterion above in Play Mode itself, fills the whole Dev Log entry (with no placeholders, and "How I verified it" describing its own checks truthfully), and ends with the files to commit and a one-line commit summary.
