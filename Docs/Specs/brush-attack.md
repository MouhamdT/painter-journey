# Spec: Brush attack

## Goal

The player can swing the brush with J to damage what's in front of it, and sees exactly where the brush reaches. It's the player's only way to fight, so it has to be quick and readable: short reach, so you have to get close (tense), and an exact visible hit box (fair). A test dummy in Room01 gives it something to hit before the enemies exist.

## Decisions

| Config field (PlayerConfig) | Value | What it does |
|---|---|---|
| `attackWidth` | 1.5 | Width of the hit box in units, measured from the player's front edge outward |
| `attackHeight` | 1.2 | Height of the hit box in units, centred on the player's centre |
| `attackDuration` | 0.1 | Seconds the hit box (and the blue stroke) stays active per swing |
| `attackDamage` | 1 | Damage to each target hit, through `Health.TakeDamage` |

| Config field (DummyConfig, new) | Value | What it does |
|---|---|---|
| `maxHealth` | 3 | The test dummy's HP |
| `hitFlashDuration` | 0.15 | Seconds the dummy stays red after a hit that doesn't kill it |

| Element | Decision |
|---|---|
| Swing | One swing per press of J. Holding J doesn't repeat. No cooldown: a new press right after a swing ends swings again |
| A press during a running swing | Ignored. A press only starts a swing when no swing is active, so the real limit is about 10 swings per second |
| Hit box position | In front of the player on the facing side. Its inner edge touches the player's front edge, so its centre is 1.25 units from the player's centre (0.5 + 1.5 / 2) |
| Facing during a swing | Locked at the moment J is pressed. Turning during the 0.1 s doesn't move the box to the other side |
| Moving and swinging | Allowed everywhere: standing, running, jumping and falling. Movement doesn't change. The box moves with the player during its 0.1 s |
| Hits per swing | Each target with a `Health` is hit at most once per swing, also if it stays inside the box for the whole 0.1 s. A target that enters the box during the 0.1 s is hit too |
| The player's own Health | Never hit by its own swing |
| Stroke visual | A flat blue rectangle (#4A90D9), exactly the hit box (1.5 × 1.2), shown for the 0.1 s, drawn in front of the player and the dummy |
| Keys | Keyboard: J only. The left mouse button and Enter bindings are removed from the `Attack` action. The gamepad, joystick, touch and XR bindings stay but aren't tested |
| While dead | J does nothing during the death pause |
| Test dummy | A grey block (#8A8A8A), 1 wide × 1.5 tall, standing on the floor at x = −2 (centre y = −3.25). It has `Health` with 3 HP, flashes red (#D84A4A) for 0.15 s when hit and disappears at 0 HP. The player walks through it (trigger collider, no Rigidbody, no gravity). It never hurts the player. The enemy-type-a spec removes it |

Useful positions, with the player 1 wide and the dummy spanning x −2.5 to −1.5:
- Player at x = −4 facing right: the box spans x −3.5 to −2.0, so it **hits** the dummy.
- Player at x = −5 facing right: the box spans x −4.5 to −3.0, so it **misses** (0.5 short).
- Player at x = 0 facing right: the box spans x 0.5 to 2.0, so it **misses**.
- Player at x = 0 facing left: the box spans x −0.5 to −2.0, so it **hits**.

## Acceptance criteria

How to place the player: in Play Mode select Player and type a Transform > Position X in the Inspector. The Y stays on the floor (−3.25). To read HP, select the dummy and look at its Health component.

1. When I open Room01 and press Play, the grey dummy stands on the floor at x = −2, its Health shows 3, and there is no blue stroke on screen.
2. With the player at its start (x = −6, facing right), when I press J, a blue rectangle 1.5 wide × 1.2 tall appears touching the player's right side (x −5.5 to −4.0) for about 0.1 s, then disappears. The dummy doesn't change.
3. With the player standing at x = −6 facing left (after a tap of A), when I press J, the blue rectangle appears on the player's left side (x −6.5 to −8.0, against the wall) and the dummy doesn't change.
4. With the player standing at x = −4 facing right, when I press J, the dummy flashes red for about 0.15 s and its Health shows 2.
5. With the dummy at 2 HP and the player at x = −4 facing right, when I press J twice (two separate presses), the dummy goes to 1 HP after the first one and disappears after the second.
6. With the dummy gone, when I press J at x = −4 facing right, the blue stroke still appears and nothing else happens. I can walk through the spot where the dummy stood.
7. With the player standing at x = −5 facing right, when I press J, the stroke appears and the dummy stays at 3 HP.
8. With the player standing at x = 0, when I press J facing right the dummy stays at 3 HP, and when I then tap A and press J facing left the dummy drops to 2 HP.
9. With the player at x = −4 facing right and the dummy at 3 HP, when I press and hold J for 2 s, there is exactly one stroke and the dummy drops to 2 HP only.
10. With the player at x = −4 facing right and the dummy at 3 HP, when I press J twice as fast as I can, there are two strokes and the dummy drops to 1 HP (no cooldown).
11. While I hold D and the player runs right at Linear Velocity x = 7, when I press J, the stroke appears and moves along with the player for its 0.1 s, and the speed stays 7.
12. During a full jump with the player at x = −4, when I press J at the top of the jump, the stroke appears in the air, the jump continues normally and the dummy below stays at 3 HP (the box is too high).
13. With the player at x = −4 facing right, when I click the left mouse button in the Game view or press Enter, no stroke appears and the dummy stays at 3 HP.
14. With the player at its start, when I hold D and run through the dummy, the player passes through it at full speed and neither the player's nor the dummy's HP changes.
15. With the player at 3 HP, when I swing 5 times in different places, the player's own Health still shows 3.
16. After I take the player to 0 HP with "Take 1 damage", when I press J during the 1.0 s death pause, no stroke appears.
17. With the dummy destroyed, when the player dies and Room01 reloads, the dummy is back at x = −2 with 3 HP.
18. With Play Mode stopped, when I set `attackWidth` to 3 on `Assets/_Game/Config/PlayerConfig.asset`, press Play and press J at x = −5 facing right, the stroke is 3 wide and the dummy drops to 2 HP. After the test I set it back to 1.5.
19. When I look at the player's attack component and the dummy in the Inspector, there are no number fields for reach, duration, damage, HP or flash time. Those numbers only appear on PlayerConfig and DummyConfig.
20. During all the checks above, the Console shows no errors or warnings, and all checks in Docs/Specs/player-movement.md and Docs/Specs/health-and-restart.md still pass.

## Edge cases

- **J pressed during a running swing:** ignored. The running swing finishes normally.
- **Turning during a swing:** the box stays on the side the player faced when J was pressed.
- **A and D both held:** the player faces its last direction, so the swing goes that way.
- **A target already inside the box when the swing starts:** hit on the first frame of the swing.
- **A target inside the box for the whole swing:** hit only once.
- **A target entering the box during the 0.1 s:** hit once when it enters.
- **Two targets in the box (later, with enemies):** both are hit once.
- **The player standing inside the dummy:** that doesn't count as a hit. Only the blue box hits.
- **A target at 0 HP:** further swings don't call it (the dummy is already gone).
- **Swinging against a wall:** the box goes into the wall. Nothing happens, and the player isn't pushed.
- **Swinging while the player is red from a hit:** allowed. The flash and the swing don't affect each other.
- **The left mouse button and Enter:** never swing.

## Examples

- Before: the player at x = −4 facing right, the dummy at 3 HP. I do: press J three times. After: 2 HP, then 1 HP, then the dummy disappears.
- Before: the player at x = −5 facing right. I do: press J. After: a blue stroke from x −4.5 to −3.0, and the dummy still has 3 HP.
- Before: the player running right at 7. I do: press J. After: a blue stroke rides along in front of the player for 0.1 s, and the player keeps running at 7.
- Before: the dummy is gone. I do: die and wait for the reload. After: the dummy is back with 3 HP.

## Out of scope

- Enemies, enemy HP and enemy reactions to hits (enemy-type-a and enemy-type-b specs).
- Knockback on targets or the player.
- Combos, a charged or heavy attack, an up or down attack, and brush powers (Won't in the Backlog).
- A cooldown (the student chose none).
- Attack sound and a real brush sprite or animation (HA04).
- Screen shake, hit-stop and particles.
- Keeping the dummy in the final game. It's a test object and enemy-type-a removes it.
- Changing the gamepad, joystick, touch or XR bindings.

## Context for the agent

Read first: `CLAUDE.md`, the three earlier specs in `Docs/Specs/`, and the current code: `Assets/_Game/Scripts/Player/` (`PlayerController` keeps an int `facing`, 1 = right and −1 = left, and mirrors `facingMarker`), `Assets/_Game/Scripts/Core/Health.cs`, `Assets/_Game/Config/PlayerConfig.asset`, `Assets/_Game/Scenes/Room01.unity`. Match their names and style.

What to create or change:
- A player attack component in `Assets/_Game/Scripts/Player/` (for example `PlayerAttack.cs`), reading `Attack` through an `InputActionReference` from the Inspector. It needs the facing from `PlayerController`, so expose it as a read-only property.
- `PlayerConfig`: add `attackWidth`, `attackHeight`, `attackDuration` and `attackDamage` under an "Attack" header, with the values above.
- The stroke: a child object with a SpriteRenderer using the placeholder square, scaled to the hit box, shown only while a swing is active, with a sorting order above the player and the dummy.
- Hit detection: an `OverlapBox` each physics step while the swing is active, with a ContactFilter2D that includes triggers. Find targets with `TryGetComponent<Health>`, skip the player's own Health and hit each Health once per swing.
- The test dummy in `Assets/_Game/Scripts/Enemies/` (namespace `PainterJourney.Enemies`), for example `TestDummy.cs`, with its own `DummyConfig` ScriptableObject in `Assets/_Game/Config/DummyConfig.asset`. It listens to its Health for the red flash and for disappearing at 0 HP (deactivate the GameObject). Add the SpriteRenderer before the BoxCollider2D, and set Is Trigger on.
- `Assets/Settings/InputSystem_Actions.inputactions`: remove the `<Mouse>/leftButton` and `<Keyboard>/enter` bindings from `Player/Attack`, through Unity (the Input System API or the Editor, then save and reimport). Read the bindings back afterwards to confirm J is still there and the other two are gone.

Rules from CLAUDE.md that matter here:
- Scene and asset changes go only through the Unity CLI.
- No `Find*` calls and no singletons.
- Recognise the player by `TryGetComponent<PlayerController>()` if needed, never by tag or name.
- Never commit.
