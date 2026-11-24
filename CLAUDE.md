# The Painter's Journey: notes for the agent

## Project
- Unity 6.3 LTS (6000.3.29f1), Universal 2D template, input: Input System only (activeInputHandler 1, so the old Input Manager is off).
- Solo student project. The agent writes all the code; I review it, test it in Play Mode and commit it.
- Design: Docs/HLD.md. Backlog: Docs/Backlog.md. Feature specs: Docs/Specs/. Dev Log: Docs/DevLog.md.
- HA02 scope (from the Backlog): main menu, one room, the painting screen and the ending. Nothing from later deadlines yet.

## Architecture
- Game code lives in Assets/_Game/Scripts/<Area>/ with the namespace PainterJourney.<Area> (Player, Enemies, Rooms, UI, Core).
- Every gameplay number (speed, jump force, damage, HP, enemy stats) lives in a ScriptableObject config in Assets/_Game/Config/. No tuning numbers in [SerializeField] fields on MonoBehaviours.
- Health.cs is the only place HP changes: TakeDamage(int) and Heal(int). The player and the enemies both use it.
- Game code never references UI. UI listens to events such as Health.Changed.
- One scene per screen: MainMenu, Room01, PaintingReveal, Ending, all in Assets/_Game/Scenes/ and in the build list in that order.
- Input goes through Assets/Settings/InputSystem_Actions.inputactions, with actions for Move (A/D), Jump (Space), Attack (J) and Pause (Esc).

## Conventions
- Recognise the player by its component: TryGetComponent<PlayerController>(). Never by tag or name.
- No singletons and no FindFirstObjectByType or other Find* calls at runtime. Get references from the Inspector or from the collision.
- Add the SpriteRenderer before the Collider2D, so the collider gets the sprite's size.
- Assets/Scenes/SampleScene.unity is from the template. Don't build on it.

## Verified API notes (Unity 6.3)
- Rigidbody2D.linearVelocity, not velocity. linearDamping, not drag.
- Read input with the Input System (UnityEngine.InputSystem). Input.GetAxis compiles but throws at runtime in this project.
- OnTriggerEnter2D needs a Rigidbody2D on one of the two objects.

## Never do
- Never edit .unity, .prefab or .asset files as text. Scene and asset changes go through Unity CLI commands. No Editor connection? List the steps for me instead.
- Never add features that aren't in the spec or my request. Suggest them at the end instead.
- Never commit or push. I do that.

## When you finish a task
Report: the files you changed, the scene objects and assets you created, how each acceptance criterion is met, and what I should test in Play Mode.
