# Spec: Enemy type B, the Shooter

## Goal

The Shooter is the ranged threat in Room01. It stands on the platform and fires slow paint blobs straight at where the player is. Standing still gets you hit, and moving, jumping or swatting a blob with the brush keeps you safe. Together with the Chaser it makes the player use the whole move set: run, jump and attack. It fires on a steady, learnable rhythm and blobs are slow and bright, so it stays fair.

## Decisions

| Config field (ShooterConfig, new) | Value | What it does |
|---|---|---|
| `maxHealth` | 2 | HP, so 2 brush hits |
| `firstShotDelay` | 2.0 | Seconds from the room starting to the first blob |
| `fireInterval` | 2.0 | Seconds between blobs after the first |
| `blobSpeed` | 5 | Units per second |
| `blobSize` | 0.4 | Width and height of a blob in units |
| `blobDamage` | 1 | Damage to the player per blob |
| `hitFlashDuration` | 0.15 | Seconds the Shooter stays red after a brush hit that doesn't kill it |

| Element | Decision |
|---|---|
| Look | A 1 × 1 square in dark teal-grey (#3F5A5A), red (#D84A4A) during its hit flash. Blobs are 0.4 × 0.4 squares in bright orange-red (#E0603A) |
| Position | Standing on the middle of the platform: x = 3, centre y = −1.0 (its bottom on the platform top at −1.5). It never moves |
| Firing | The first blob comes 2.0 s after the room starts, then one every 2.0 s, wherever the player is, until the Shooter dies |
| Aim | Each blob starts at the Shooter's centre and flies in a straight line toward the player's centre as it was at the moment of firing, at 5 units/s. It doesn't follow the player afterwards |
| A blob hits the player | The player loses 1 HP (through `Health.TakeDamage`) and the blob disappears |
| A blob hits a wall, the floor or the ceiling | The blob disappears |
| The platform | Blobs fly through it. Otherwise almost every shot would hit the platform under the Shooter |
| Other enemies | Blobs fly through the Chaser and the Shooter, without effect |
| Swatting | If the brush's blue stroke touches a blob, the blob disappears and nobody takes damage |
| Touching the Shooter | Harmless. The player passes through it (a trigger) |
| Hit by the brush | It flashes red for 0.15 s and loses 1 HP. Only reachable from the platform: from the floor the brush's box is too low |
| At 0 HP | It disappears at once and fires no more blobs. Blobs already in the air keep flying until they hit something |
| Restart | The room reload brings the Shooter back with 2 HP, removes all blobs and starts the 2.0 s first-shot timer again |
| Count | One Shooter, together with the one Chaser |

## Acceptance criteria

How to measure:
- **Placing the player:** select Player in Play Mode and type Transform > Position X (or run there).
- **Blob speed:** pause Play Mode while a blob is flying, select it in the Hierarchy and read Rigidbody2D > Info > Linear Velocity.
- **HP:** read the Health component.

1. When I open Room01 and press Play, a dark teal 1 × 1 square stands on the middle of the platform at x = 3, its Health shows 2, and no blob appears during the first 2.0 s.
2. With the player standing still at its start (x = −6), when about 2.0 s have passed after Play, one orange-red 0.4 × 0.4 blob leaves the Shooter's centre and flies in a straight line toward the player at 5 units/s (Linear Velocity length 5).
3. With the player standing still at its start, when I watch for 7 s, blobs leave at about 2, 4 and 6 s, one every 2.0 s.
4. With the player standing still at 3 HP, when a blob reaches it, the player's HP drops to 2 (a pip goes dark, the player flashes red) and the blob disappears.
5. With a blob flying at the player, when I run away to the side before it arrives, the blob keeps its line, misses the player and disappears when it hits a wall or the floor.
6. With the player standing on the floor right under the platform (x = 3), when the Shooter fires, the blob flies straight down through the platform and hits the player.
7. With a blob flying at the player, when I press J so the blue stroke touches the blob, the blob disappears and the player's HP doesn't change.
8. With the Chaser between the Shooter and the player, when a blob flies through the Chaser, the blob keeps flying and the Chaser's HP doesn't change.
9. With the player on the platform, when I walk through the Shooter, the player's HP doesn't change.
10. With the player standing on the platform at x = 1.5 facing right, when I press J, the Shooter flashes red and its Health shows 1.
11. With the Shooter at 1 HP, when I hit it once more, it disappears, and no new blobs appear afterwards (a blob already in the air keeps flying until it hits something).
12. After the Shooter is gone, when the player dies (with "Take 1 damage") and Room01 reloads, the Shooter is back at x = 3 with 2 HP, there are no blobs, and the first new blob comes about 2.0 s after the reload.
13. With Play Mode stopped, when I set `fireInterval` to 1 on `Assets/_Game/Config/ShooterConfig.asset` and press Play, blobs leave at about 2, 3, 4 and 5 s. After the test I set it back to 2.
14. With Play Mode stopped, when I set `blobSpeed` to 2.5 and press Play, the blobs fly at Linear Velocity length 2.5. After the test I set it back to 5.
15. When I select the Shooter or a blob and look at their components, there are no number fields for HP, timing, speed, size, damage or flash time. Those numbers only appear on ShooterConfig.
16. During all the checks above, the Console shows no errors or warnings, and all checks in the earlier specs (movement, health, brush, Chaser) still pass.

## Edge cases

- **The player dead (during the death pause):** the Shooter may still fire and blobs may still reach the player, but the player's Health ignores all damage at 0 HP.
- **The player standing inside the Shooter at the moment it fires:** the blob is fired straight left at 5 units/s, because there's no direction to aim in.
- **Two blobs in the air at once:** allowed. Each one works on its own.
- **A blob touching the player and the stroke in the same frame:** the swat wins. The blob disappears with no damage.
- **One swing touching two blobs:** both disappear.
- **One swing touching a blob and the Shooter:** the blob disappears and the Shooter takes 1 damage.
- **A brush hit during the Shooter's red flash:** it counts, and the flash timer restarts.
- **The Shooter dying while a blob is in the air:** that blob keeps flying and can still hurt or be swatted.
- **The fall reset (player below y = −10):** the Shooter keeps its rhythm and aims at the player's new position on the next shot.
- **A blob hitting the ceiling:** it disappears (only possible with the player very high, but still handled).

## Examples

- Before: Play just pressed, the player at x = −6. I do: nothing. After: at 2 s a blob flies at me, hits me at 3 → 2 HP and disappears. At 4 s the next one comes.
- Before: a blob 2 units away, flying at me. I do: press J. After: the stroke swats it, and I stay at 3 HP.
- Before: a blob flying at me. I do: run right. After: it misses, keeps going and disappears against the floor.
- Before: I'm on the platform at x = 1.5, facing the Shooter. I do: press J twice. After: it flashes after the first hit and disappears after the second, and the firing stops.

## Out of scope

- More than one Shooter, or a moving or jumping Shooter.
- Lobbed arcs, spreads, homing blobs and reflecting blobs back.
- Touch damage from the Shooter.
- Blobs that stick, splash or leave paint behind.
- The door and "room clear" (room-clear-door spec).
- Knockback, stagger and player invulnerability (rejected in Session 1).
- Shooter sounds and animations (HA04).

## Context for the agent

Read first: `CLAUDE.md`, all specs in `Docs/Specs/`, and the current code: `Assets/_Game/Scripts/Core/Health.cs`, `Assets/_Game/Scripts/Player/` (PlayerController, PlayerConfig, PlayerAttack), `Assets/_Game/Scripts/Enemies/` (the Chaser and ChaserConfig, which this follows as a model), `Assets/_Game/Scripts/Rooms/`, `Assets/_Game/Scenes/Room01.unity`. Match their names and style.

What to create or change:
- `Assets/_Game/Scripts/Enemies/Shooter.cs`, `ShooterConfig.cs` and `PaintBlob.cs` (namespace `PainterJourney.Enemies`), with `Assets/_Game/Config/ShooterConfig.asset` holding the values above. The Shooter passes `maxHealth` to its `Health` the same way the player and the Chaser do.
- A blob prefab in `Assets/_Game/Prefabs/PaintBlob.prefab`, created through the Unity CLI: a SpriteRenderer (the placeholder square), then a trigger BoxCollider2D, a Kinematic Rigidbody2D moved by `linearVelocity`, and PaintBlob. The Shooter references the prefab from the Inspector and creates one with `Instantiate` per shot (no pooling needed).
- What stops a blob: add a small marker component (for example `PainterJourney.Rooms.RoomBounds`) to the floor, the two walls and the ceiling, but not to the platform. A blob disappears on a trigger contact with a RoomBounds or with the player (recognised by `TryGetComponent<PlayerController>()`, then its `Health`). It ignores everything else. Don't use tags, layers or names.
- Swatting: the brush must be able to remove blobs without the Player code depending on the Enemies code. Add a tiny interface in Core (for example `PainterJourney.Core.ISwattable` with `Swat()`), implement it on PaintBlob, and let PlayerAttack call it on anything in its box that has one, as well as damaging Health targets.
- Aim: the Shooter needs the player's Transform from the Inspector.
- Room01: one Shooter at x = 3, y = −1.0. Mark the floor, walls and ceiling with RoomBounds.

Rules from CLAUDE.md that matter here:
- Scene and asset changes go only through the Unity CLI.
- No `Find*` calls and no singletons.
- The game code never references the UI.
- Never commit.

The student doesn't play-test. The build session tests every criterion above in Play Mode itself, fills the whole Dev Log entry (with no placeholders, and "How I verified it" describing its own checks truthfully), and ends with the files to commit and a one-line commit summary.
