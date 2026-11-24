# Spec: Player movement

## Goal

The player can run left and right and jump in a small test room, and it always does exactly what the keys say: instant start and stop, full air control, a jump you can cut short and no sticking to walls. This is the base of "tense but in control" from Docs/HLD.md. Every later fight depends on movement that feels precise and fair. It also gives the later specs (health, brush attack, enemies, door) a Room01 scene and a player to build on.

## Decisions

These are the values from the interview. All the numbers in the first table live in `PlayerConfig`. The room and player sizes are set in the scene.

| Config field (PlayerConfig) | Value | What it does |
|---|---|---|
| `moveSpeed` | 7 | Units per second, on the ground and in the air |
| `jumpHeight` | 3.5 | Units the feet rise with Space held. The code works out the take-off speed from this (about 14.3 units/s) |
| `gravityScale` | 3 | Applied to the player's Rigidbody2D. With Unity's default gravity of 9.81 this gives 29.43 units/s² |
| `jumpCutMultiplier` | 0.65 | Upward speed × 0.65 when Space is released while rising. A tap gives about 1.5 units |
| `fallResetY` | −10 | Below this y the player is put back at its start position |

| Scene layout (Room01) | Position and size, in world units |
|---|---|
| Camera | The template camera, unchanged: orthographic size 5, at (0, 0, −10), showing about x −8.9 to 8.9 and y −5 to 5 |
| Inside of the room | x from −8 to 8, y from −4 to 4 (16 wide, 8 tall) |
| Floor | Top at y = −4, 1 thick, x from −9 to 9 |
| Left and right walls | Inner faces at x = −8 and x = 8, 1 thick, y from −5 to 5 |
| Ceiling | Bottom at y = 4, 1 thick, x from −9 to 9 |
| Platform | x from 1 to 5 (4 wide), top at y = −1.5 (2.5 above the floor), 0.5 thick, so its underside is at y = −2 |
| Player | 1 wide × 1.5 tall. Starts standing on the floor at x = −6 (centre y = −3.25), facing right |
| Facing marker | A small darker square on the player's front side, near the top. It is on the right when facing right and on the left when facing left |
| Colors | Placeholder flat greys: dull grey background, darker grey floor, walls, ceiling and platform, light grey player, dark grey marker |

Useful results of these numbers:
- A full jump on flat ground takes about 1.0 s from take-off to landing. The highest point is about 0.5 s after take-off.
- A running jump carries the player about 6.8 units sideways.
- A full jump from the platform brings the head to y = 3.5, which is still 0.5 below the ceiling.
- From the start, the player needs 13.5 units to reach the right wall, which takes about 1.9 s at 7 units/s.

## Acceptance criteria

How to measure in Play Mode:
- **Speed:** select Player, open Rigidbody2D > Info and read Linear Velocity.
- **Positions:** read Transform > Position. Player positions are the centre of the player, and its feet are 0.75 below the centre.
- **The top of a jump:** press the Pause button during the jump, then use the Step button frame by frame to find the highest Position Y.

1. When I open File > Build Profiles > Scene List, Room01 (`Assets/_Game/Scenes/Room01.unity`) is the first scene and SampleScene is not in the list.
2. When I open Room01 and press Play, the player is standing on the floor at x = −6, y = −3.25. The darker marker is on its right side, and the whole room (floor, both walls, ceiling and platform) is visible in the Game view. The Console shows no errors or warnings.
3. From the start position, when I hold D, the player runs right at Linear Velocity x = 7 from the first frame. It reaches the right wall in about 2 s and the marker stays on the right.
4. From the start position, when I hold A, the marker flips to the left side on the first frame and the player runs left at Linear Velocity x = −7 until it touches the left wall.
5. While the player is running right at x = 7, when I release D, Linear Velocity x drops to 0 on the same frame. The player stops in place without sliding, and the marker stays on the right.
6. While I hold D and the player is running, when I also press A (both held), the player stops (Linear Velocity x = 0) and the marker stays on the right.
7. From the start position, when I hold → (right arrow) instead of D, the player runs right at x = 7, the same as with D. ← (left arrow) runs left at x = −7, the same as with A.
8. From the start position, when I press or hold W, S, ↑ or ↓, nothing happens: the player doesn't move, doesn't jump and doesn't flip.
9. Standing still on the floor, when I press and hold Space, the player jumps straight up. Its centre reaches y = 0.25 ± 0.1 (feet 3.5 above the floor) and it lands back on the floor about 1.0 s after take-off.
10. Standing still on the floor, when I tap Space (press and release at once), the player does a short hop. Its centre reaches about y = −1.75 ± 0.2 (feet about 1.5 above the floor), clearly lower than in criterion 9.
11. During a full jump, when I release Space after the highest point (while falling), the fall doesn't change. It lands about 1.0 s after take-off, the same as in criterion 9.
12. Standing on the floor, when I keep Space held through a full jump and the landing, the player doesn't jump again after it lands. It only jumps again after I release Space and press it again.
13. While the player is in the air (rising or falling), when I press Space again, nothing happens: there is no double jump.
14. From the start position, when I hold D and press and hold Space with the player at about x = −2, the player lands on the platform and stands on it, with its centre at y = −0.75 (feet at −1.5).
15. Standing on the platform, when I hold Space, the player jumps 3.5 units above the platform (centre about y = 2.75). Its head stays below the ceiling and it lands back on the platform.
16. Standing on the platform, when I hold D and walk off its right edge (x = 5), the player falls to the floor. Pressing Space while it falls does nothing.
17. During a full jump while holding D, when I switch to holding A, the player moves left at Linear Velocity x = −7 from that frame, still in the air, and the marker flips to the left.
18. From the start position, when I hold D until the player reaches the right wall and keep holding, the player stops with its right side against the wall (centre x = 7.5 ± 0.05). It stays inside the room and doesn't shake, and Linear Velocity x is 0.
19. Standing against the right wall while holding D, when I press and hold Space, the player rises and falls next to the wall without sticking to it. It lands on the floor about 1.0 s after take-off, the same as in criterion 9.
20. Standing on the floor under the platform (x = 3), when I press and hold Space, the player's head hits the underside of the platform (centre at y = −2.75). It stops rising at once, falls back to the floor and never ends up on top of or inside the platform.
21. While the game is running, when I select Player and set Transform > Position Y to −12 in the Inspector, the player appears back at x = −6, y = −3.25, standing still (Linear Velocity 0, 0) and facing right.
22. With Play Mode stopped, when I set `moveSpeed` to 3.5 on `Assets/_Game/Config/PlayerConfig.asset`, press Play and hold D, Linear Velocity x is 3.5. After the test I set it back to 7.
23. When I select Player in the Hierarchy and look at the PlayerController component, it has a field for PlayerConfig and fields for references only (for example the input actions), with no number fields for speed, jump or gravity.

## Edge cases

- **Both A and D held:** they cancel out to 0. The player stands still on the ground, or falls straight down in the air (no sideways speed). The facing stays on the last direction.
- **No keys held:** the facing stays on the direction the player last moved. At the start of the scene that's right.
- **Space while in the air:** ignored. That includes walking off the platform edge and pressing Space a moment after (no coyote time). It also includes pressing Space a moment before landing, which is simply lost (no jump buffer).
- **Space held while landing:** no new jump. Every jump needs a fresh press.
- **Space released while rising:** the upward speed is multiplied by 0.65 once, on that frame. Releasing it again or after the highest point does nothing.
- **Space pressed while touching a wall but not the ground:** ignored. Only a surface under the player's feet (the floor or the platform top) counts as ground, never the side of a wall or the platform.
- **Pushing into a wall in the air:** the player falls at normal speed, with no friction, sticking or slowing down.
- **Head hits something while rising:** the upward movement ends and the player falls normally.
- **Below y = −10:** this should never happen in the closed room. If it does, the player is put back at its start position with zero speed, facing right, and nothing else changes (no damage, no scene reload).
- **Fast falls:** the player can't pass through the 0.5-thick platform or the floor at any falling speed in this room.
- **W/S/↑/↓ together with A/D:** only the horizontal part matters. For example, holding W and D is the same as holding D.
- **Gamepad or joystick:** it stays bound but isn't tested in HA02. Any left or right push counts as full speed in that direction (no slow walking).

## Examples

- Before: the player stands at x = −6 on the floor, facing right. I do: hold D for 1 s, then release. After: the player is at about x = 1, stopped dead, facing right.
- Before: the player stands still at x = −6 on the floor. I do: tap Space. After: the feet rise about 1.5 units, and the player lands back at x = −6.
- Before: the player stands still at x = −6 on the floor. I do: hold Space. After: the feet rise about 3.5 units, and the player lands back at x = −6 about 1.0 s later.
- Before: the player is running right at x = −2. I do: hold Space while still holding D. After: the player lands on the platform (feet at y = −1.5) and keeps running right until it walks off the edge at x = 5 and falls to the floor.
- Before: the player stands on the floor at x = 3, under the platform. I do: hold Space. After: its head bumps the platform underside after rising 0.5 units, and it drops straight back down.
- Before: the player is in the air, moving right at 7. I do: hold A as well. After: Linear Velocity x is 0, and the player falls straight down, still facing right.
- Before: the player is running in Play Mode. I do: set Position Y to −12 in the Inspector. After: the player is back at x = −6, standing on the floor, facing right, not moving.

## Out of scope

- Health, damage, death and restarting the room (health-and-restart spec), including fall damage.
- The brush attack and the Attack action on J (brush-attack spec). The facing marker exists so the attack has a direction later.
- Enemies (enemy-type-a and enemy-type-b specs).
- Room clear and the door (room-clear-door spec).
- Camera follow or any camera change. The template camera stays as it is.
- Pause: Esc does nothing in this spec. There's no pause menu in HA02.
- Main menu, the painting reveal and the Ending scenes.
- Coyote time and a jump buffer: possible feel upgrades after playtesting.
- Wall slide, wall jump, double jump, dash, crouch, sprint and the dodge roll. The template's Crouch and Sprint actions stay unused.
- Slow walking with a gamepad stick, and any gamepad testing.
- Changing or removing any binding in InputSystem_Actions.
- Sprites, animations, particles, sound and art beyond the flat grey placeholders.
- Deleting SampleScene. It's only removed from the build list.

## Context for the agent

Read first: `CLAUDE.md`, `Docs/HLD.md`, `Assets/Settings/InputSystem_Actions.inputactions` (Player map: `Move` is a 2D Vector composite, `Jump` is a Button on Space).

What to create:
- `Assets/_Game/Scripts/Player/PlayerController.cs` and `Assets/_Game/Scripts/Player/PlayerConfig.cs`, in namespace `PainterJourney.Player`.
- `Assets/_Game/Config/PlayerConfig.asset`, filled with the values from the Decisions table.
- `Assets/_Game/Scenes/Room01.unity` with the camera, Global Light 2D, the floor, the walls, the ceiling, the platform and the Player.
- The build list change: Room01 in, SampleScene out.

How to build it:
- Make every scene and asset change through the Unity CLI against the open Editor (6000.3.29f1), never by editing .unity, .prefab or .asset files as text.
- Add the SpriteRenderer before the BoxCollider2D on every object, so the collider gets the sprite's size.
- Player Rigidbody2D: Dynamic, Z rotation frozen, Collision Detection set to Continuous, Interpolate on. Gravity scale comes from `PlayerConfig.gravityScale`.
- Give the player's collider a PhysicsMaterial2D with Friction 0, so it never sticks to walls.
- Set `rb.linearVelocity` directly for the horizontal speed, so the start and stop are instant. Use `linearVelocity`, not `velocity`.
- For the ground check, count only contacts whose normal points up (the floor and the platform top), so walls and the platform's sides and underside don't count as ground. Avoid adding new layers or tags if possible.
- Jump take-off speed = √(2 × 9.81 × gravityScale × jumpHeight). Read gravity from `Physics2D.gravity` instead of hard-coding 9.81.
- Read input only through the Input System and the `Move` and `Jump` actions of the InputSystem_Actions asset, referenced from the Inspector. No `Input.GetAxis`, no `Find*` calls and no singletons. Use only the x of `Move`.
- Flip a child visual (the marker or a visual root), not the root's collider or Rigidbody.
- The start position for the fall reset is wherever the player was placed in the scene when Play started, not a hard-coded number.
