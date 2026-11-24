# Spec: Health and restart

## Goal

The player has 3 HP, sees them as 3 pips on screen, sees a clear red flash when hit, and restarts the room after a short pause at 0 HP. This is the "lose" rule from Docs/HLD.md ("Health reaches zero, and the player restarts the current room"). With only 3 HP every hit matters, which gives the tension, and a quick, clean restart keeps it fair. It also adds the shared `Health` component that the enemies use later.

## Decisions

| Config field (PlayerConfig) | Value | What it does |
|---|---|---|
| `maxHealth` | 3 | The player's HP at the start of the room |
| `hitFlashDuration` | 0.15 | Seconds the player stays red after a hit that doesn't kill it |
| `restartDelay` | 1.0 | Seconds between reaching 0 HP and Room01 reloading |

| Element | Decision |
|---|---|
| A hit | Costs 1 HP. Enemies later call `TakeDamage(1)` |
| Testing damage | Two test-only items in the ⋮ menu of the Health component: "Take 1 damage" and "Heal 1". No hazards or debug keys |
| Invulnerability after a hit | None (rejected in Session 1). Every hit counts, even during the flash |
| Knockback | None in this spec |
| HP display | 3 square pips in the top-left corner, 48 × 48 px, 16 px apart, 54 px from the top and left edges (at the 1920 × 1080 reference resolution). Full pip: warm orange (#E8A33D). Empty pip: dark grey (#3A3A3A). The pips go from left to right, and the right-most full pip empties first |
| Hit feedback | The player turns red (#D84A4A) for 0.15 s, then back to its normal grey. It can still move and be hit |
| Death (HP reaches 0) | No red flash. The player turns dark (#2A2A2A), stops in place (also in mid-air, no falling) and ignores all input. After 1.0 s Room01 reloads |
| After the reload | The player is back at x = −6, y = −3.25, facing right, 3 HP, 3 full pips, normal colour |
| Falling below y = −10 | Unchanged from the movement spec: back to the start, HP stays the same, no reload |

## Acceptance criteria

How to test damage: in Play Mode select Player, click the ⋮ menu on its Health component and choose "Take 1 damage" or "Heal 1". The Health component shows the current HP in the Inspector.

1. When I open Room01 and press Play, three full orange pips are in the top-left corner and the Health component shows current HP 3.
2. With the player at 3 HP, when I choose "Take 1 damage", the right-most pip turns dark grey, HP shows 2 and the player turns red for about 0.15 s, then back to grey.
3. With the player at 2 HP, when I choose "Take 1 damage", the middle pip turns dark grey too, HP shows 1 and the player flashes red again.
4. With the player at 1 HP, when I choose "Take 1 damage", all three pips are dark grey, HP shows 0 and the player turns dark (not red) and stops where it is.
5. During the 1.0 s after death, when I hold A, D or Space, the player doesn't move, jump or flip.
6. After the player dies, when I wait, Room01 reloads about 1.0 s later: the player stands at x = −6, y = −3.25 facing right in its normal colour, with three full pips and HP 3.
7. While the player is in the air during a full jump at 1 HP, when I choose "Take 1 damage", the player freezes in mid-air (it doesn't fall) and Room01 reloads about 1.0 s later.
8. With the player at 1 HP, when I choose "Heal 1", HP shows 2, the middle pip turns orange again and the player doesn't flash.
9. With the player at 3 HP, when I choose "Heal 1", nothing changes: HP stays 3, the pips stay full and nothing flashes.
10. While I hold D and the player runs right at 3 HP, when I choose "Take 1 damage", the player flashes red and keeps running at Linear Velocity x = 7, with no knockback and no slowdown.
11. With the player at 3 HP, when I choose "Take 1 damage" twice within 0.15 s, HP drops to 1 (both hits count) and the player stays red until about 0.15 s after the second hit.
12. During the 1.0 s after death, when I choose "Take 1 damage" or "Heal 1", nothing changes: HP stays 0 and Room01 still reloads once, about 1.0 s after the death.
13. With the player at 2 HP, when I set Transform > Position Y to −12 in the Inspector, the player is back at x = −6 on the floor, HP still shows 2 with one dark pip, and Room01 does not reload.
14. With Play Mode stopped, when I set `maxHealth` to 5 on `Assets/_Game/Config/PlayerConfig.asset` and press Play, five full pips appear and HP shows 5. After the test I set it back to 3.
15. With Play Mode stopped, when I set `restartDelay` to 3 and press Play, then take the player to 0 HP, Room01 reloads about 3 s after the death. After the test I set it back to 1.0.
16. When I select Player and look at its components, and at the HUD and restart objects, none of them has a number field for HP, flash time or restart delay. Those numbers only appear on PlayerConfig.
17. During all the checks above, the Console shows no errors or warnings, and all 23 checks in Docs/Specs/player-movement.md still pass.

## Edge cases

- **Damage bigger than the HP left:** HP stops at 0, never goes below 0, and the player dies.
- **Heal above the maximum:** HP stops at the maximum (3). Nothing changes and nothing is announced when HP was already full.
- **Damage or heal of 0 or less:** ignored, with no change and no event.
- **Anything after death:** further damage and heals are ignored, the death is announced only once and the reload happens only once.
- **A hit during the red flash:** it counts. The flash timer restarts from that hit, so the player stays red 0.15 s after the last hit.
- **The killing hit:** no red flash. The player goes straight to the dark death colour.
- **Death in mid-air:** the player freezes where it is, with no falling, then the room reloads.
- **Keys held during the death pause:** ignored. When the room reloads the player starts standing still, even if a key is still held, until the movement code reads the key again on the next frames.
- **Below y = −10:** back to the start with the same HP, as in the movement spec. It's not a death and doesn't reload the room.
- **Esc during the death pause:** nothing (Pause is only a binding in HA02).

## Examples

- Before: 3 HP, three full pips. I do: "Take 1 damage". After: 2 HP, pips full-full-empty, the player blinks red for 0.15 s.
- Before: 1 HP, running right. I do: "Take 1 damage". After: 0 HP, all pips empty, the player is dark and frozen. 1.0 s later the room reloads with the player at x = −6 and three full pips.
- Before: 1 HP. I do: "Heal 1" three times. After: 3 HP, three full pips. The third heal changes nothing.
- Before: 2 HP. I do: drop the player below y = −10. After: back at x = −6, still 2 HP.

## Out of scope

- Enemies and enemy damage, including how often an enemy can hurt you while touching you (enemy-type-a and enemy-type-b specs).
- Knockback when hit.
- Invulnerability or a cooldown after a hit (rejected in Session 1).
- Hazards, spikes, pits and fall damage.
- Health pickups or any way to heal in gameplay. "Heal 1" is only a test button.
- A "You died" screen, a game over screen or lives.
- Sound for hits and death (HA04).
- Enemy health bars.
- Restart animations, fades or screen shake.
- Any pause menu.

## Context for the agent

Read first: `CLAUDE.md`, `Docs/Specs/player-movement.md` and the code from the player movement build (`Assets/_Game/Scripts/Player/`, `Assets/_Game/Config/PlayerConfig.asset`, `Assets/_Game/Scenes/Room01.unity`). This spec builds on it, so match its names.

What to create or change:
- `Assets/_Game/Scripts/Core/Health.cs` (namespace `PainterJourney.Core`): the only place HP changes. It has `TakeDamage(int)` and `Heal(int)`, a `Changed` event (current and maximum HP) and a `Died` event that fires once. It shows the current HP in the Inspector, and has `[ContextMenu]` items "Take 1 damage" and "Heal 1". It gets its maximum from its owner's config (for the player, `PlayerConfig.maxHealth`) rather than a number field of its own, so the enemies can reuse it with their own configs.
- `PlayerConfig`: add `maxHealth`, `hitFlashDuration` and `restartDelay` with the values above.
- The player: listens to its Health for the red flash and the death (dark colour, freeze, ignore input). The flash and the death colour change the player's sprite colour, not the facing marker's.
- `Assets/_Game/Scripts/UI/` (namespace `PainterJourney.UI`): a HUD script that creates one pip per maximum HP and listens to `Health.Changed`. Use a uGUI Canvas (Screen Space Overlay, Canvas Scaler scaling with screen size, reference 1920 × 1080) in Room01. The game code never references the UI: the HUD gets the Health reference from the Inspector.
- `Assets/_Game/Scripts/Rooms/` (namespace `PainterJourney.Rooms`): a small component in Room01 that listens to the player's `Health.Died`, waits `restartDelay` and reloads the active scene, so later rooms can reuse it. It gets the Health reference from the Inspector.

Rules from CLAUDE.md that matter here:
- No `Find*` calls and no singletons: every reference comes from the Inspector.
- Scene and asset changes go only through the Unity CLI.
- Never edit .unity, .prefab or .asset files as text.
- Never commit.
- Make sure the HUD shows the right pips on the very first frame, whatever order `Awake`/`OnEnable`/`Start` run in.
