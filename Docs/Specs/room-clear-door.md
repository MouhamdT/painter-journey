# Spec: Room clear and door

## Goal

When both enemies in Room01 are dead, the room is clear: a door in the right wall opens with a warm glow, and walking into it takes the player to the painting reveal. This closes the core loop from Docs/HLD.md ("Enter room → fight → clear → reveal part of the painting") and makes clearing the room feel like a moment of its own.

## Decisions

| Config field (RoomConfig, new) | Value | What it does |
|---|---|---|
| `doorOpenDelay` | 0.5 | Seconds from the last enemy dying to the door opening |

| Element | Decision |
|---|---|
| Door position | In the right wall, standing on the floor: x 8 to 9, y −4 to −2 (1 wide × 2 tall). The right wall above it now runs from y = −2 to 5. The player (1.5 tall) fits through |
| Closed door | A dark brown-grey block (#4A4038), solid like the wall. The player stops against it at x = 7.5 |
| Room clear | When every enemy in the room's list has died (the Chaser and the Shooter, in any order) |
| Opening | 0.5 s after the last enemy dies, the closed door disappears (its sprite and solid collider turn off) and the doorway shows a warm yellow glow (#F2D27A) of the same size |
| Leaving | The moment the player touches the glowing doorway, the PaintingReveal scene loads |
| Blobs | Blobs disappear at the doorway, closed or open (it counts as a room boundary), so they never fly out of the room |
| PaintingReveal scene | This spec creates `Assets/_Game/Scenes/PaintingReveal.unity` as a placeholder: the camera and a plain dark background, with nothing else. The painting-reveal spec fills it in |
| Build list | Room01 first, then PaintingReveal. MainMenu and Ending are added by their own specs |
| Restart | If the player dies at any time, the room reload brings back both enemies and a closed door |

## Acceptance criteria

To kill enemies quickly for a check, select the enemy in Play Mode and use "Take 1 damage" in the ⋮ menu of its Health component, or fight it normally.

1. When I open Room01 and press Play, a dark brown-grey door 1 wide × 2 tall sits at the bottom of the right wall (x 8 to 9, y −4 to −2), and the rest of the right wall is unchanged.
2. With both enemies alive, when I hold D and run to the right wall, the player stops against the door at x = 7.5 and nothing loads.
3. With both enemies alive, when I kill only the Chaser and wait 3 s, the door stays closed.
4. After a reload, with both enemies alive, when I kill only the Shooter and wait 3 s, the door stays closed.
5. With one enemy left, when I kill it, about 0.5 s later the door disappears and the doorway glows yellow.
6. With the door open, when I run right into the glowing doorway, the PaintingReveal scene loads at once (the Hierarchy shows PaintingReveal and the Game view shows its plain background).
7. With the player pressed against the closed door and one enemy left, when I kill that enemy (through the Inspector), PaintingReveal loads about 0.5 s later, as soon as the door opens, without me moving.
8. With the door open, when the player dies (with "Take 1 damage") and Room01 reloads, the door is closed again and both enemies are back.
9. With a blob flying toward the door (closed), when it reaches the door, the blob disappears.
10. When I open File > Build Profiles > Scene List, Room01 is first and PaintingReveal is second.
11. With Play Mode stopped, when I set `doorOpenDelay` to 2 on `Assets/_Game/Config/RoomConfig.asset`, press Play and kill both enemies, the door opens about 2 s after the last kill. After the test I set it back to 0.5.
12. When I look at the door and room-clear components in the Inspector, there are no number fields for the delay. It only appears on RoomConfig.
13. During all the checks above, the Console shows no errors or warnings, and all checks in the earlier specs still pass.

## Edge cases

- **Both enemies dying in the same frame:** the room is clear, and the door opens 0.5 s later, once.
- **The player dying after the last enemy but before the door opens:** death wins. The room reloads as usual, with both enemies back and the door closed.
- **The player already touching the doorway when it opens:** PaintingReveal loads at once.
- **The player dead (frozen) when the door opens:** nothing loads, and the room reloads as usual.
- **A blob in the air when the door opens:** it disappears if it reaches the doorway.
- **Walking back left after the door opens:** allowed. The door stays open.
- **The fall reset after the door opens:** the door stays open.

## Examples

- Before: both enemies dead, the door glowing. I do: run right. After: PaintingReveal loads as I touch the doorway.
- Before: only the Shooter alive. I do: hit it twice from the platform. After: 0.5 s later the door turns into a yellow glow.
- Before: the door open. I do: "Take 1 damage" three times. After: the room reloads with both enemies and a closed door.

## Out of scope

- The painting reveal itself (painting-reveal spec). PaintingReveal is only an empty placeholder here.
- More rooms, and going from PaintingReveal to the next room (HA03).
- Door animations, sounds, fades and screen transitions.
- A key, lever or button to open the door.
- Saving progress between scenes.

## Context for the agent

Read first: `CLAUDE.md`, all specs in `Docs/Specs/`, and the current code: `Assets/_Game/Scripts/Core/Health.cs` (the `Died` event), `Assets/_Game/Scripts/Enemies/` (Chaser, Shooter, PaintBlob), `Assets/_Game/Scripts/Rooms/` (RoomRestart and the RoomBounds marker), `Assets/_Game/Scenes/Room01.unity`. Match their names and style.

What to create or change:
- `Assets/_Game/Scripts/Rooms/RoomConfig.cs` and `Assets/_Game/Config/RoomConfig.asset` (doorOpenDelay 0.5).
- A room-clear component in `Assets/_Game/Scripts/Rooms/` with a list of enemy `Health` references set in the Inspector (the Chaser's and the Shooter's). It listens to each `Died` and opens the door `doorOpenDelay` seconds after the last one.
- A door component (Rooms): the closed block (SpriteRenderer, then a solid BoxCollider2D) and the glow (SpriteRenderer) with an exit trigger (BoxCollider2D, Is Trigger). When the player touches the exit trigger (recognised by `TryGetComponent<PlayerController>()`) and the door is open, load `PaintingReveal` with `SceneManager.LoadScene`. Ignore it while the player is dead (its Health is 0). Give the doorway the RoomBounds marker so blobs stop there.
- Room01: shorten the right wall to y −2 to 5 and add the door at x 8 to 9, y −4 to −2.
- `Assets/_Game/Scenes/PaintingReveal.unity`: the camera with a plain dark background, and nothing else. Add it to the build list after Room01.

Rules from CLAUDE.md that matter here:
- Scene and asset changes go only through the Unity CLI.
- No `Find*` calls and no singletons.
- The game code never references the UI.
- Never commit.

The student doesn't play-test. The build session tests every criterion above in Play Mode itself, fills the whole Dev Log entry (with no placeholders, and "How I verified it" describing its own checks truthfully), and ends with the files to commit and a one-line commit summary.
