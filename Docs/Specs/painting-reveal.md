# Spec: Painting reveal

## Goal

After clearing Room01, the player sees their painting with one more part painted in. The colour is brushed down the panel from top to bottom, so the reward feels made by the painter. This is the core reward from Docs/HLD.md: "every room you clear adds a new part to your painting", and the painting is the most colourful thing in the game. In HA02 one room is cleared, so part 1 of 5 is revealed and then the Ending follows.

## Decisions

| Config field (PaintingConfig, new) | Value | What it does |
|---|---|---|
| `revealedPart` | 1 | Which panel this screen reveals (1 = leftmost). Always 1 in HA02 |
| `totalParts` | 5 | Number of panels (4 rooms plus the boss) |
| `wipeDelay` | 0.5 | Seconds from the scene starting to the brush wipe starting |
| `wipeDuration` | 1.5 | Seconds the wipe takes to cover the panel from top to bottom |

| Element | Decision |
|---|---|
| Background | Plain dark (#1E1E1E), from the placeholder PaintingReveal scene the door spec made |
| Painting | Centred at (0, 0.5): a painting area 10 wide × 6 tall split into 5 vertical panels, each 2 wide × 6 tall (panel 1 at x −5 to −3, panel 5 at x 3 to 5), with a dark brown frame (#5A3E2B) 0.3 thick around the whole painting |
| Unrevealed panel | Flat grey (#6A6A6A) |
| Panel 1 in colour | A simple placeholder landscape: the top 4 units sky blue (#7EC8E3), the bottom 2 units grass green (#6DBE5A). HA04 replaces it with a slice of the real AI painting |
| Brush wipe | After 0.5 s, the coloured panel 1 is uncovered from its top edge downward over 1.5 s, at an even speed. A mask grows over the coloured panel, so the art itself is never stretched. At the end, panel 1 is fully coloured |
| Text | When the wipe ends, "Part 1 of 5" appears centred under the painting (white, y ≈ −3.3), and "Press Space to continue" appears below it (light grey #BBBBBB, smaller, y ≈ −4.2) |
| Continuing | After the prompt appears, a new press of Space (the `Jump` action) loads the Ending scene. Space during the wipe does nothing, and Space held since before the prompt doesn't count until it's pressed again |
| Ending scene | This spec creates `Assets/_Game/Scenes/Ending.unity` as a placeholder (camera and plain dark background only). The ending spec fills it in |
| Build list | Room01, PaintingReveal, Ending, in that order. MainMenu is added at the top by its own spec |

## Acceptance criteria

To start here quickly, open `Assets/_Game/Scenes/PaintingReveal.unity` directly and press Play, or play Room01 and go through the door.

1. When I open PaintingReveal and press Play, a framed painting of 5 grey panels sits in the middle of the screen, with no text.
2. With PaintingReveal just started, when I wait about 0.5 s, the colour starts appearing at the top of panel 1 (the leftmost) and moves down.
3. While the wipe runs, when I watch, it reaches the bottom of panel 1 about 1.5 s after it started (about 2.0 s after Play), and panel 1 then shows sky blue on top and green at the bottom. Panels 2 to 5 stay grey.
4. When the wipe ends, "Part 1 of 5" and "Press Space to continue" appear under the painting at the same moment.
5. While the wipe is still running, when I press Space, nothing happens: the wipe continues and the scene stays.
6. After the prompt appears, when I press Space, the Ending scene loads (the Hierarchy shows Ending and the Game view shows its plain background).
7. With Space held down from the start of the scene, when the prompt appears, nothing loads. Only releasing and pressing Space again loads the Ending.
8. With all of Room01 played (both enemies killed and the door touched), when PaintingReveal loads, the same reveal plays from the start.
9. When I open File > Build Profiles > Scene List, the order is Room01, PaintingReveal, Ending.
10. With Play Mode stopped, when I set `wipeDuration` to 4 on `Assets/_Game/Config/PaintingConfig.asset` and press Play in PaintingReveal, the wipe takes about 4 s. After the test I set it back to 1.5.
11. When I look at the painting components in the Inspector, there are no number fields for the part, the count or the timings. They only appear on PaintingConfig.
12. During all the checks above, the Console shows no errors or warnings, and all checks in the earlier specs still pass.

## Edge cases

- **Space pressed during the wipe:** ignored.
- **Space held into the scene (for example from jumping through the door):** it only counts once it's released and pressed again after the prompt.
- **A, D, J or Esc on this screen:** nothing.
- **Pressing Space twice quickly after the prompt:** the Ending loads once.
- **A different screen size:** the painting and text stay centred. The frame always fits on screen at 16:9.

## Examples

- Before: PaintingReveal just loaded. I do: wait. After: at 0.5 s colour starts at the top of panel 1, at 2.0 s the panel is fully coloured, and the two lines of text appear.
- Before: the prompt is showing. I do: press Space. After: the Ending loads.
- Before: the wipe is halfway. I do: press Space. After: nothing, and the wipe finishes.

## Out of scope

- The real painting art (HA04, AI art).
- Revealing parts 2 to 5, remembering progress between rooms, and going to Room02 (HA03).
- Music and sound on this screen (HA04).
- Fades between scenes, particles and camera moves.
- Clicking with the mouse to continue.
- The Ending screen's content (ending spec). Ending is only an empty placeholder here.

## Context for the agent

Read first: `CLAUDE.md`, all specs in `Docs/Specs/`, and the current code and scenes, especially `Assets/_Game/Scenes/PaintingReveal.unity` (the placeholder from the door spec), `Assets/_Game/Scripts/Rooms/` and how `PlayerController` reads input through `InputActionReference`. Match their names and style.

What to create or change:
- `Assets/_Game/Scripts/UI/` (namespace `PainterJourney.UI`), for example `PaintingReveal.cs`, and `PaintingConfig.cs` with `Assets/_Game/Config/PaintingConfig.asset` holding the values above. The painting is UI-only, so it belongs in UI. It doesn't touch gameplay code.
- The painting in PaintingReveal: world-space SpriteRenderers with the placeholder square for the frame, the grey panels and the coloured panel 1. Panel 1's colour is uncovered by a SpriteMask whose height grows from the top down (pivot at the top), with the coloured sprites set to "Visible Inside Mask", so a real image can replace them later without stretching. Alternatively use a uGUI Image with Fill Method Vertical, Origin Top: either way, the art isn't scaled.
- Text: a uGUI Canvas with TextMeshPro (import the TMP Essential Resources if they aren't in the project yet).
- Input: read the `Jump` action through an `InputActionReference`, and only react to a press that starts after the prompt is visible (`WasPressedThisFrame` after that moment). Then load `Ending` with `SceneManager.LoadScene`.
- `Assets/_Game/Scenes/Ending.unity`: a placeholder with the camera and a plain dark background. Add it to the build list after PaintingReveal.

Rules from CLAUDE.md that matter here:
- Scene and asset changes go only through the Unity CLI.
- No `Find*` calls and no singletons.
- Never commit.

The student doesn't play-test. The build session tests every criterion above in Play Mode itself, fills the whole Dev Log entry (with no placeholders, and "How I verified it" describing its own checks truthfully), and ends with the files to commit and a one-line commit summary.
