# Spec: Enemy type A, the Chaser

## Goal

The Chaser is the close-range threat in Room01. It waits until you come near, then walks after you and hurts you when it touches you. You beat it with the brush or dodge it by jumping over it. It's slower than you and pauses after every hit, so you're always in control of when and how you fight: tense, but fair. It replaces the brush spec's test dummy as the thing to hit.

## Decisions

| Config field (ChaserConfig, new) | Value | What it does |
|---|---|---|
| `maxHealth` | 3 | HP, so 3 brush hits |
| `moveSpeed` | 4 | Units per second while chasing (the player runs at 7) |
| `noticeRange` | 6 | It starts chasing when the player's centre is this close sideways (any height) |
| `contactDamage` | 1 | Damage to the player per touch |
| `hitPause` | 1.0 | Seconds it stands still and can't hurt after it hits the player |
| `hitFlashDuration` | 0.15 | Seconds it stays red after a brush hit that doesn't kill it |

| Element | Decision |
|---|---|
| Look | A 1 × 1 square in muted dark purple (#5A4A6A). Red (#D84A4A) during its hit flash |
| Start | Standing on the floor at x = 6 (centre y = −3.5), still. The player starts 12 units away, so it notices the player once the player reaches x = 0 |
| Waiting | Before it notices the player, it doesn't move at all |
| Chasing | Once noticed, it chases until it dies. It never goes back to waiting, even if the player runs far away. It walks along the floor toward the player's x at 4 units/s. It never jumps |
| Player above it | When the player is on the platform (or in the air) right above it, it stands still under the player, within 0.1 units of the player's x, with no shaking |
| Touching the player | It hurts the player for 1 HP, through the player's `Health.TakeDamage`. Then it stops for 1.0 s and can't hurt. After the pause, if it still touches the player, it hurts again at once. The player is never made invulnerable |
| Hit by the brush | It flashes red for 0.15 s, loses 1 HP and keeps doing what it was doing (chasing or pausing). No knockback and no stagger |
| At 0 HP | It disappears at once (the GameObject is deactivated) and never hurts again |
| Passing through | The player can run through it. It's a trigger, so it doesn't block the player and the player can't stand on it |
| Walls | It never needs to pass a wall: it only walks toward the player's x, and the player can't leave the room |
| Count | One Chaser in Room01 |
| Test dummy | Removed: the dummy object leaves Room01, and `TestDummy.cs` and `DummyConfig.asset` are deleted |
| Restart | The room reload brings the Chaser back at x = 6 with 3 HP, waiting |

## Acceptance criteria

How to measure:
- **Positions:** to place the player, select Player in Play Mode and type Transform > Position X (or run there).
- **Speed:** select the Chaser and open Rigidbody2D > Info > Linear Velocity.
- **HP:** read the Health component on the player or on the Chaser.

1. When I open Room01 and press Play, a dark purple 1 × 1 square stands on the floor at x = 6, its Health shows 3, and there is no test dummy in the Hierarchy or in the room.
2. With the player at its start (x = −6), when I wait 5 s without pressing anything, the Chaser doesn't move.
3. With the player standing at x = −1 (7 units away), when I wait 3 s, the Chaser still doesn't move.
4. With the player standing at x = −1, when I walk right to x = 0 or further, the Chaser starts walking left toward the player, at Linear Velocity x = −4.
5. After the Chaser has noticed the player, when I run back to the far left wall, the Chaser keeps walking after the player and doesn't stop until it reaches the player's x.
6. After the Chaser has noticed the player, when I stand on the platform at x = 3, the Chaser walks under the player and stands still there (Linear Velocity 0). It doesn't shake, and the player's HP doesn't change.
7. With the player standing still at 3 HP and the Chaser walking toward it, when the Chaser touches the player, the player's HP drops to 2 (one pip goes dark, the player flashes red) and the Chaser stops for about 1.0 s, then moves again.
8. With the player at 3 HP standing still, when I let the Chaser reach the player and keep standing still inside it, HP goes 3 → 2 on the first touch, 2 → 1 about 1.0 s later and 1 → 0 about 1.0 s after that. The player dies and Room01 reloads.
9. Right after the Chaser hurts the player, when I run through it and away during its 1.0 s pause, the player takes no more damage from that touch.
10. With the Chaser chasing and the player facing it within brush range, when I press J, the Chaser flashes red, its Health shows 2 and it keeps walking at 4 units/s (no knockback).
11. With the Chaser at 2 HP and in brush range, when I hit it twice more, its Health shows 1 after the first hit and it disappears after the second. After that nothing hurts the player, and I can walk where it was.
12. With the Chaser chasing toward the player, when I run toward it and do a full running jump over it from about 2 units away, the player lands on its other side and the player's HP doesn't change. (A jump straight up doesn't work: the Chaser waits under the player and touches it on landing.)
13. After the Chaser is gone, when the player dies (with "Take 1 damage") and Room01 reloads, the Chaser is back at x = 6 with 3 HP, standing still.
14. With Play Mode stopped, when I set `moveSpeed` to 2 on `Assets/_Game/Config/ChaserConfig.asset`, press Play and walk to x = 0, the Chaser chases at Linear Velocity x = −2. After the test I set it back to 4.
15. With Play Mode stopped, when I set `noticeRange` to 10 and press Play, the Chaser stays still while the player is at x = −6 and starts chasing when the player reaches x = −4. After the test I set it back to 6.
16. When I select the Chaser and look at its components, there are no number fields for HP, speed, range, damage, pause or flash time. Those numbers only appear on ChaserConfig.
17. When I look in the Project window, `TestDummy.cs` and `DummyConfig.asset` are gone and the project compiles.
18. During all the checks above, the Console shows no errors or warnings, and all checks in the movement and health specs still pass. The brush spec's checks that used the dummy are now covered by checks 10 and 11 here, and its other checks (stroke, keys, J only, hold J, mouse and Enter) still pass.

## Edge cases

- **Player dead (during the death pause):** the Chaser may keep moving or touch the player, but the player's Health ignores all damage once at 0 HP.
- **The player above the Chaser:** the Chaser waits underneath, within 0.1 units of the player's x, without shaking from side to side.
- **The player standing exactly at noticeRange (6.0 units):** this counts as noticed.
- **A brush hit during the Chaser's 1.0 s pause:** the Chaser flashes and loses HP, and the pause continues as before.
- **Death of the Chaser during its pause:** it disappears at once.
- **The player jumping and landing on the Chaser:** the player passes through it to the floor and takes damage if it overlaps the Chaser (and the Chaser isn't pausing).
- **Being hit by the Chaser and the brush in the same frame:** both happen. Neither cancels the other.
- **The player overlapping the Chaser when it first notices:** it hurts the player at once, then pauses.
- **The fall reset (player below y = −10):** the Chaser keeps chasing toward the player's new x.

## Examples

- Before: the Chaser waiting at x = 6, the player at x = −6. I do: walk right to x = 0. After: the Chaser starts walking left at 4 units/s.
- Before: 3 HP, the Chaser touching the player. I do: nothing. After: HP 2 at once, 1 after about 1 s, 0 after about 2 s, and the room reloads.
- Before: the Chaser at 3 HP walking at me. I do: press J three times as it comes into range. After: it flashes twice and disappears on the third hit.
- Before: the Chaser 2 units away and coming at me. I do: run at it and do a full running jump. After: I land on its other side with no damage, and it turns around and follows at 4 while I run away at 7.
- Before: the Chaser coming at me. I do: a jump straight up. After: it walks under me and waits, and I take 1 damage when I land on it.

## Out of scope

- Enemy B, the Shooter (enemy-type-b spec).
- The door and "room clear" (room-clear-door spec).
- More than one Chaser.
- Jumping, flying, climbing onto the platform or turning at edges.
- Knockback and stagger, on the Chaser or the player.
- Player invulnerability after a hit (rejected in Session 1).
- A Chaser health bar, or showing its facing.
- Chaser sounds and animations (HA04).
- A boss version of the Chaser (HA03, Backlog plan B).

## Context for the agent

Read first: `CLAUDE.md`, all specs in `Docs/Specs/`, and the current code: `Assets/_Game/Scripts/Core/Health.cs`, `Assets/_Game/Scripts/Player/` (PlayerController, PlayerConfig, the attack component), `Assets/_Game/Scripts/Enemies/` (the test dummy, to remove), `Assets/_Game/Scripts/UI/`, `Assets/_Game/Scripts/Rooms/`, `Assets/_Game/Scenes/Room01.unity`. Match their names and style.

What to create or change:
- `Assets/_Game/Scripts/Enemies/Chaser.cs` and `ChaserConfig.cs` (namespace `PainterJourney.Enemies`), with `Assets/_Game/Config/ChaserConfig.asset` holding the values above. The Chaser passes `maxHealth` to its `Health` the same way the player does.
- Physics: a Kinematic Rigidbody2D, so it doesn't fall, push or get pushed. Move it by setting `linearVelocity` (only x), so the Inspector shows the speed. Use a BoxCollider2D set to Is Trigger, so the player passes through and the brush's OverlapBox (which includes triggers) finds it. Add the SpriteRenderer before the BoxCollider2D.
- Noticing: compare the x of the player's Transform with the Chaser's x. The player reference comes from the Inspector.
- Contact damage: in `OnTriggerStay2D` (the player has a dynamic Rigidbody2D), recognise the player with `TryGetComponent<PlayerController>()`, get its `Health` from the same object and call `TakeDamage(contactDamage)` when the Chaser isn't pausing. Then start the pause.
- Room01: add one Chaser at x = 6, y = −3.5, and delete the test dummy object. Delete `TestDummy.cs`, its `.meta` and `DummyConfig.asset` through Unity (for example `AssetDatabase.DeleteAsset`), and the `DummyConfig` script if it's separate.

Rules from CLAUDE.md that matter here:
- Scene and asset changes go only through the Unity CLI.
- No `Find*` calls and no singletons.
- The game code never references the UI.
- Never commit.

The student doesn't play-test. The build session tests every criterion above in Play Mode itself, fills the whole Dev Log entry (with no placeholders, and "How I verified it" describing its own checks truthfully), and ends with the files to commit and a one-line commit summary.
