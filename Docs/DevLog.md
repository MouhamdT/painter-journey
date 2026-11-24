# Dev Log

## 2026-11-11 · Session 1 · Feature backlog with MoSCoW

**Goal:** Get a list of features for my game and sort them with MoSCoW for Backlog.md.

**Model:** Claude Opus 5.5 (claude.ai chat)

**Key prompts:**
> You're a game designer who helps students scope their first game.
>
> I'm making a solo 2D action game in Unity 6.3 LTS this semester. The player is a painter on a journey to complete a painting. The game has 5 rooms: 4 rooms plus a final boss room. Each room has its own obstacles and enemies that the player has to fight. Clearing a room reveals one more part of the painting and opens the next room. I want the player to feel tense but in control, so every fight feels hard but fair. That tension should push the player to keep going, while the painting slowly coming together makes them feel they are building something beautiful.
>
> List 12 candidate features and sort them with MoSCoW (Must, Should, Could, Won't this semester).
>
> Together, the Musts must make a complete game from main menu to ending and nothing more. Keep it to 5 rooms, because I'm one person with one semester.
>
> Reply as a table: Feature | MoSCoW | Why | Effort (S/M/L).
>
> Example row: Clearing a room reveals part of the painting | Must | It is the core reward and progression | M

**What the AI produced:** A table of 12 features: 8 Musts, 2 Shoulds (attack telegraphs and hit feedback, dodge roll), 1 Could (sound and music) and 1 Won't (brush powers).

**Accepted:**
* Two enemy types reused across rooms, to keep the work small
* The boss room and the painting reveal as Musts
* Dodge roll as Should
* Brush powers as Won't

**Changed (and why):**
* Sound and music moved from Could to Must, because HA04 requires AI made music and SFX in the game.
* Main menu and ending screen joined into one row, to keep the table short.
* Shortened all the Why texts so the table is easy to read.

**Rejected (and why):**
* Attack telegraphs and hit feedback, because I don't think it would be cool to have a short cooldown where I can't get hit after an enemy hits me.

**How I verified it:**
I read every row and checked if this is the game I want to work on.

**Time spent:** 45 minutes

## 2026-11-24 · Session 2 · Unity project setup and CLAUDE.md

**Goal:** Set up the Unity 6.3 project inside my HA01 repo and write CLAUDE.md, so the agent knows the rules of my project before it builds anything.

**Model:** Claude Opus 5.5 (claude.ai chat)

**Key prompts:**
> Let's start Home Assignment 02. Here are the assignment PDF and the lecture slides.

> Before we start, give me step by step what I need to install and what you need from me.

> Before you change anything, explain what we are doing now and why.

> Explain in more detail why the UI should listen to events instead of the game code changing the UI directly.

For CLAUDE.md, the AI drafted it from the context-kit template and marked every guess with [?]. Then it asked me about each [?] one at a time and I answered.

**What the AI produced:**
* A setup checklist: Unity 6.3 LTS, Windows Build Support (Mono), the Unity CLI, the Unity plugin for Claude and Git.
* A way to add the Unity project to my existing repo: create it as PainterTemp, then move Assets, Packages and ProjectSettings into painter-journey, because Unity won't create a project in a folder that already exists.
* A first draft of CLAUDE.md with the Unity version, template and input setting checked in the project files, plus [?] for every decision it couldn't know.

**Accepted:**
* Scripts in Assets/_Game/Scripts/<Area>/ with the namespace PainterJourney.<Area>
* All gameplay numbers in ScriptableObject configs in Assets/_Game/Config/
* Health.cs is the only place HP changes, for the player and the enemies
* Game code sends events and the UI listens, game code never touches the UI. I asked for more explanation first: Health.cs announces "HP changed" and the health bar listens, so the UI can change without breaking gameplay and enemies can use Health.cs without having a health bar.
* One scene per screen: MainMenu, Room01, PaintingReveal and Ending
* Controls go through the template's InputSystem_Actions file
* Don't build on the template's SampleScene
* Add the SpriteRenderer before the Collider2D. In the lecturer's Lesson 3 test the collider got size zero and the player fell through the floor. The AI said I could also wait until the agent makes this mistake in my project, but I chose to add it now to prevent the bug.

**Changed (and why):**
* .gitignore: GitHub Desktop only ignored one .vscode file, so it was changed to ignore the whole .vscode folder and .DS_Store. Those are editor and macOS files, not part of the game.
* In Unity Hub I created a local project instead of a Unity Cloud project and picked no source control, because GitHub is already my version control.

**Rejected (and why):**
* The AI first told me to use Unity 6.6.6, mixing up a version from my other class. I rejected it because the HA02 assignment says Unity 6.3 LTS (its mentioned in the docs), I also told it to only use the information I gave it in this chat.

**How I verified it:**
I checked the file list in github desktop, in unity console, after each prompt i check claude.md and verify that it adds the settings right

**Time spent:** 1 hour 25 minutes

## 2026-11-24 · Session 3 · Moving to Claude Code, setup fixes and HA02 plan

**Goal:** Move from the claude.ai chat to Claude Code, so the agent can read the repo and drive the Unity Editor itself. Then get a plan for the HA02 prototype before anything is built.

**Model:** Claude Opus 5.5 (Claude Code desktop app, Plan mode first)

**Key prompts:**
> First task: read the repo, tell me how it's structured and what's in it now, then suggest a plan for the HA02 prototype. Don't change anything until I approve.

> should we do some edits on the task since all the time i ran it on regular chat instead of claude code? then we continue ?

**What the AI produced:**
* A read-only look at the repo, the HA02 PDF and the Lesson 3, 4 and 6 slides. `unity status` showed my project connected and "ready" on 6000.3.29f1.
* Two gaps between CLAUDE.md and the project: Attack wasn't bound to J and there was no Pause action.
* A plan in three parts: setup fixes for working in Claude Code, one spec per feature in build order (player movement, health and restart, brush attack, enemy A, enemy B, room clear and door, painting reveal, main menu, ending), then the Windows build and the GitHub Release.
* Three questions, which I answered: both enemy types in Room01 already, the Ending shows the painting so far (1 of 5), and Pause is only a key binding with no menu.
* The setup fixes, after I approved the plan:
  * .claude/settings.json with the Lesson 3 permission rules: force push, reset --hard and .env are blocked, and git commit and git push always ask me.
  * J added to Attack and a new Pause action on Esc in InputSystem_Actions. Unity reimported it and read the bindings back.
  * Docs/Specs/SPEC-template.md copied from the context-kit.
  * Two CLAUDE.md lines: always use 6000.3.29f1 because 6000.6.6f1 is also installed, and where the spec template is. Plus the HA02 scope and Pause decisions.
  * The Model line of sessions 1 and 2 now says they ran in the claude.ai chat.
* An answer to "how many features im making in the HLD?": 8 Musts in the HLD. HA02 splits its part of them into 9 smaller specs, so each one fits in one build session.

**Accepted:**
* The plan: one spec per feature, in build order, with an interview session that writes the spec and a fresh session that builds it (the Lesson 4 loop)
* The Ending shows the painting so far (1 of 5 parts) in HA02, and the full painting only after the boss later
* Pause as a key binding only, so CLAUDE.md is true, with no pause menu yet
* All the setup fixes listed above
* Adding the permission rules now, so the agent can't commit or push for me

**Changed (and why):**
* Enemies: the AI suggested only one enemy type in Room01 for HA02 and the second one in HA03. I chose both types now to see if it will be too much.

**Rejected (and why):**
* Nothing this session.

**How I verified it:**
I read every changed file in GitHub Desktop: CLAUDE.md, the Dev Log, the new settings.json and the spec template. I opened InputSystem_Actions in Unity and saw J [Keyboard] under Attack and Escape [Keyboard] under Pause. They don't do anything in Play Mode yet because there is no player script.


**Time spent:** About 45 minutes

## 2026-11-24 · Session 4 · Player movement spec (interview)

**Goal:** Write the spec for the first HA02 feature, player movement, in an interview before anything is built, so a fresh session can build it from the spec alone.

**Model:** Claude Opus 5.5 (Claude Code desktop app, started from the session 3 plan)

**Key prompts:**
> Interview me and write the spec for the first HA02 feature: player movement. Don't write any code or change any scene yet. A later fresh session builds from the spec.

> Ask me one question at a time, with lettered options (a/b/c) where that helps, and your recommendation. Wait for my answer before the next question.

> Every acceptance criterion must be numbered and testable in Play Mode, written as: a starting state, one action, what I see, with numbers.

**What the AI produced:**
* A read of CLAUDE.md, the HLD, the Backlog, the spec template and the input file. It found that the Move action also has W/S, arrow, gamepad and joystick bindings, and that the template camera shows about 17.8 × 10 units, so the room has to fit in that.
* 11 interview questions, each with options and a recommendation: the room layout, player size and start, move speed, jump height, holding vs tapping Space, how short a tap is, air control, A and D together, W/S and the arrows, walls and head bumps, falling out of the room, and coyote time / jump buffer.
* Docs/Specs/player-movement.md from the template: a decisions table with the PlayerConfig values and the Room01 layout, 23 numbered acceptance criteria with numbers and how to measure them in Play Mode, edge cases, examples, out of scope and notes for the build session.

**Accepted:**
* Room01 is a closed 16 × 8 box with one platform 4 wide, 2.5 above the floor, that fits the template camera
* The player is 1 × 1.5, starts on the floor at x = −6 facing right, with a darker marker that shows the facing
* 7 units/s with an instant start and stop, and the same full control in the air
* Jump 3.5 units high with gravity scale 3, about 1 s in the air
* A and D together cancel out, the arrows move too, W/S do nothing and the input file stays the same
* The player stops flat at walls without sticking, and a head bump ends the jump
* A safety net: below y = −10 the player goes back to the start
* No coyote time or jump buffer for now, and both are in Out of scope

**Changed (and why):**
* Jump: the AI recommended a fixed jump height, but I picked a variable height where releasing Space early cuts the jump short, because I want more control in fights. A tap is about 1.5 units and a hold is 3.5 (jumpCutMultiplier 0.65).

**Rejected (and why):**
* The AI first said halving the speed on release gives a 1.5-unit tap. Then it calculated it and corrected itself: halving gives only about 0.9 units, less than the player's own height. So we used 0.65 instead of 0.5.

**How I verified it:**
I read the spec and checked the numbers in the decisions table match my answer.

**Time spent:** 1 hour 30 minutes

## 2026-11-24 · Session 5 · Player movement build

**Goal:** Build the player movement feature from Docs/Specs/player-movement.md in a fresh session, with every scene and asset change made through the Unity CLI against the open Editor.

**Model:** Claude Opus 5.5 (Claude Code desktop app, a side session started from the session 4 plan)

**Key prompts:**
> Build the player movement feature exactly as written in Docs/Specs/player-movement.md. The spec is the source of truth. Don't add anything that isn't in it, and list ideas as suggestions at the end instead.

> Make every scene and asset change through the Unity CLI, never by editing .unity, .prefab or .asset files as text.

> Report the files changed, the scene objects and assets created, how each of the 23 acceptance criteria is met, and what I should test in Play Mode.

**What the AI produced:**
* PlayerConfig.cs (a ScriptableObject with the 5 movement numbers) and PlayerController.cs (run, jump, jump cut, ground and wall checks from contact normals, facing marker, fall reset) in Assets/_Game/Scripts/Player/.
* PlayerConfig.asset with the spec values, a 1-unit white placeholder sprite, and a PhysicsMaterial2D with friction 0 for the player.
* Room01.unity from the project's 2D scene template (camera and Global Light 2D), with the floor, walls, ceiling, platform and Player at the spec's positions and sizes. Room01 is the only scene in the build list, and SampleScene is still in the project.
* A Play Mode check with simulated key presses: rest height, full jump, tap, A and D, the wall, the head bump, landing on the platform and the fall reset. The Console stayed at 0 errors and 0 warnings.

**Accepted:**
* The movement feels right: instant run, variable jump, no wall sticking, and all checks pass in Play Mode
* The half-step jump fix, so the jump is really 3.5 units

**Changed (and why):**
* Jump take-off speed: the spec's formula √(2 × g × jumpHeight) only gave a 3.36-unit jump in Unity, because physics runs in 0.02 s steps. That is outside criterion 9's tolerance. The AI added a half-step correction, so the jump is now 3.50 units. jumpHeight still means "how high the feet rise".

**Rejected (and why):**
* Nothing this session.

**How I verified it:**
I played Room01 and went through the spec's checklist: moving, jumping, walls, the fall reset and changing Move Speed in PlayerConfig. Everything worked.

**Time spent:** About 15 minutes (build and play-test)
