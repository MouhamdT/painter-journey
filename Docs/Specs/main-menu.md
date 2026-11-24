# Spec: Main menu

## Goal

The game opens on a main menu with the title, an empty grey painting as a teaser, a Start button that begins Room01, a Quit button and a one-line reminder of the controls. It's the "start" half of "Main menu and ending" in the Backlog. From the very first screen it shows what the player is working toward: filling the painting.

## Decisions

| Element | Decision |
|---|---|
| Scene | `Assets/_Game/Scenes/MainMenu.unity`, first in the build list: MainMenu, Room01, PaintingReveal, Ending |
| Background | Plain dark (#1E1E1E), like the painting screen |
| Title | "The Painter's Journey", white, large (about 72 px at 1920 × 1080), centred near the top (about 110 px from the top edge) |
| Painting teaser | The framed painting from the painting spec, smaller: 5 wide × 3 tall, centred at world (0, 1.2), with 5 grey panels (#6A6A6A) each 1 × 3, and the dark brown frame (#5A3E2B) 0.15 thick. All panels grey and nothing animates |
| Buttons | Two buttons under the painting, centred: "Start" (about 320 × 80 px, centre 200 px below the screen centre) and "Quit" (same size, 310 px below the centre). Grey (#6A6A6A) with white text. The selected or hovered button is warm yellow (#F2D27A) with dark text |
| Controls line | "A/D move · Space jump · J brush" in light grey (#BBBBBB), small (about 28 px), centred about 40 px above the bottom edge |
| Keyboard | Start is selected when the menu opens. W/S and ↑/↓ move the selection between Start and Quit. Enter presses the selected button |
| Mouse | Hovering a button selects it. Clicking presses it |
| Start | Loads Room01, which starts fresh: player at x = −6, 3 HP, both enemies and a closed door |
| Quit | In the Windows build, closes the game. In the Editor, stops Play Mode, so it can be tested |
| No numbers in configs | This screen has no gameplay numbers, so it has no config |

## Acceptance criteria

1. When I open File > Build Profiles > Scene List, the order is MainMenu, Room01, PaintingReveal, Ending.
2. When I open MainMenu and press Play, I see on a dark background: the title "The Painter's Journey" at the top, a small framed painting with 5 grey panels under it, the Start and Quit buttons, and "A/D move · Space jump · J brush" at the bottom.
3. When the menu has just opened, the Start button is yellow (selected) and Quit is grey.
4. With Start selected, when I press S (or ↓), Quit turns yellow and Start turns grey. When I press W (or ↑), Start is selected again.
5. With Start selected, when I press Enter, Room01 loads with the player at x = −6, 3 full pips, both enemies in place and the door closed.
6. With the menu open, when I click Start with the mouse, Room01 loads the same way.
7. With the menu open, when I move the mouse over Quit, Quit turns yellow.
8. In the Editor, when I select Quit and press Enter (or click it), Play Mode stops.
9. With the menu open, when I press A, D, Space, J or Esc, nothing happens: no scene loads and the selection doesn't change. (Space, J and Esc have no meaning on the menu.)
10. When I resize the Game view to a different 16:9 size (for example 1280 × 720), the title, buttons and controls line stay centred and readable, and the painting stays between the title and the buttons.
11. During all the checks above, the Console shows no errors or warnings, and all checks in the earlier specs still pass when I start them from the menu.

## Edge cases

- **Pressing Enter twice quickly on Start:** Room01 loads once.
- **Hovering one button with the mouse while the other is selected by keyboard:** the hovered one becomes selected. Only one button is yellow at a time.
- **Clicking on the background (not a button):** nothing happens, and the keyboard selection stays.
- **Dying in Room01:** the room restarts as before. It doesn't go back to the menu.
- **Esc in Room01:** still nothing (Pause is only a binding in HA02).

## Examples

- Before: the game just started. I do: press Enter. After: Room01 starts.
- Before: the menu with Start selected. I do: press ↓ then Enter. After: in the Editor, Play Mode stops (in the build, the game closes).
- Before: the menu. I do: click Start. After: Room01 starts.

## Out of scope

- A settings, options, controls or credits screen.
- Music and sound on the menu (HA04).
- Animations, fades and a moving background.
- Continue or save slots.
- A pause menu, or going back to the menu from Room01 (Pause is only a binding in HA02).
- Gamepad navigation testing.

## Context for the agent

Read first: `CLAUDE.md`, all specs in `Docs/Specs/`, especially `Docs/Specs/painting-reveal.md` (the frame and panel look to reuse), and the current scenes and UI code (`Assets/_Game/Scenes/`, `Assets/_Game/Scripts/UI/`). Match their names and style.

What to create or change:
- `Assets/_Game/Scenes/MainMenu.unity` with the camera (dark background), the small painting as SpriteRenderers with the placeholder square, and a uGUI Canvas (Screen Space Overlay, Canvas Scaler with screen size, reference 1920 × 1080) with TextMeshPro text and two uGUI Buttons.
- An EventSystem with the Input System UI Input Module (not the old Standalone Input Module, because the old Input Manager is off). Set Start as the first selected object. Use the Button's colour tint (or a small script) so the selected and hovered states are yellow #F2D27A.
- `Assets/_Game/Scripts/UI/MainMenu.cs` (namespace `PainterJourney.UI`) with methods for Start (`SceneManager.LoadScene("Room01")`) and Quit (`Application.Quit()`, plus `EditorApplication.isPlaying = false` inside `#if UNITY_EDITOR`), hooked to the buttons' OnClick in the Inspector.
- The build list: MainMenu at the top, then Room01, PaintingReveal, Ending.

Rules from CLAUDE.md that matter here:
- Scene and asset changes go only through the Unity CLI.
- No `Find*` calls and no singletons.
- Never commit.

The student doesn't play-test. The build session tests every criterion above in Play Mode itself, fills the whole Dev Log entry (with no placeholders, and "How I verified it" describing its own checks truthfully), and ends with the files to commit and a one-line commit summary.
