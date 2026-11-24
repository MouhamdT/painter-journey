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

## 2026-11-24 · Session 6 · Health and brush attack specs (interview)

**Goal:** While the player movement build ran, write the next two specs, health-and-restart and brush-attack, in the same interview session, so each build session can start as soon as the feature before it is tested and committed.

**Model:** Claude Opus 5.5 (Claude Code desktop app, the session 4 interview session)

**Key prompts:**
> whle the other chat is working, wanna continue interviewing me for the rest and start a subagent for each afrer we finish the interview

> lets interview for the feature 3 and the rest

**What the AI produced:**
* A change to my plan: the builds run one at a time, not all at once, because each feature builds on the one before and they all edit Room01 in the same Unity Editor.
* 6 questions for health-and-restart and Docs/Specs/health-and-restart.md with 17 acceptance criteria.
* 6 questions for the brush attack and Docs/Specs/brush-attack.md with 20 acceptance criteria.
* The health build session (Session 7), started after I approved its spec.

**Accepted:**
* Health: 3 HP and 1 damage per hit, "Take 1 damage" / "Heal 1" test buttons in the Inspector instead of hazards, 3 orange pips top-left, a 0.15 s red flash, and at 0 HP a dark freeze and a room reload after 1.0 s. Falling out of the room still only resets the player, with no HP lost.
* Brush: a 1.5 × 1.2 hit box for 0.1 s, swinging anywhere with no slowdown, a walk-through test dummy with 3 HP at x = −2, only J attacks (the mouse click and Enter come off Attack), and a blue stroke the size of the hit box.
* Builds one at a time, each after I test and commit the one before.

**Changed (and why):**
* Health: the AI recommended 5 HP, but I picked 3 HP, so every hit matters more.
* Brush: the AI recommended a 0.35 s cooldown, but I picked no cooldown, so every press of J swings. The AI added one rule so the spec has no gap: a press during a running 0.1 s swing is ignored.

**Rejected (and why):**
* Building all the features in parallel, because they would overwrite each other's changes in Room01.

**How I verified it:**
I read both specs and checked that the Decisions tables match my answers before approving them.

**Time spent:** About 35 minutes

## 2026-11-24 · Session 7 · Health and restart build

**Goal:** Build the health-and-restart feature from Docs/Specs/health-and-restart.md in a fresh session: 3 HP, the pips, the red hit flash, the dark death freeze and the room reload. Every scene and asset change goes through the Unity CLI against the open Editor.

**Model:** Claude Opus 5.5 (Claude Code desktop app, a side session started from the session 6 spec interview)

**Key prompts:**
> Build the health-and-restart feature exactly as written in Docs/Specs/health-and-restart.md. The spec is the source of truth. Don't add anything that isn't in it, and list ideas as suggestions at the end instead.

> Every reference comes from the Inspector: no Find* and no singletons. The game code never references the UI.

> Check in Play Mode (for example by calling TakeDamage and Heal through the CLI) that the pips, the flash, the death freeze and the reload work. Re-check that the 23 movement criteria still hold.

**What the AI produced:**
* Health.cs in Assets/_Game/Scripts/Core/: the only place HP changes, with TakeDamage, Heal, the Changed and Died events and the "Take 1 damage" / "Heal 1" items in the component's ⋮ menu. It has no number field. The player passes PlayerConfig.maxHealth to it in Awake. A small Inspector script (Core/Editor/HealthEditor.cs) shows the current HP read-only in Play Mode.
* Three new PlayerConfig fields with their values on the asset: maxHealth 3, hitFlashDuration 0.15, restartDelay 1.0.
* PlayerController listens to its Health: a 0.15 s red flash on a hit that doesn't kill, and at 0 HP it turns dark, freezes in place (also in mid-air) and ignores input.
* HealthHud.cs (UI) and a HUD canvas in Room01 (Screen Space Overlay, scaled from 1920 × 1080): one 48 × 48 pip per max HP, 16 px apart, 54 px from the top-left corner, orange when full and dark grey when empty.
* RoomRestart.cs (Rooms) on a RoomRestart object in Room01: it reloads the active scene restartDelay seconds after the player dies, only once.
* A Play Mode check through the CLI with a frame-by-frame recorder: hits, heals, the flash timing, death, the reload timing, the mid-air death, maxHealth 5, restartDelay 3, and all 23 movement checks again. The Console stayed at 0 errors and 0 warnings.

**Accepted:**
* The whole feature as the spec describes it: 3 HP, the pips, the red flash, the dark freeze and the reload after 1.0 s
* The small Inspector script that shows the current HP, so I can see the number while testing

**Changed (and why):**
* Nothing this session.

**Rejected (and why):**
* Nothing this session.

**How I verified it:**
I read the build report. The AI's own Play Mode check through the CLI passed: hits, heals, the flash timing, death, the reload, the mid-air death, changing maxHealth and restartDelay, and all 23 movement checks again, with 0 errors and 0 warnings in the Console. I didn't play-test it myself yet.

**Time spent:** About 15 minutes

## 2026-11-24 · Session 8 · Brush attack build

**Goal:** Build the brush-attack feature from Docs/Specs/brush-attack.md in a fresh session: J swings the brush, a blue stroke shows exactly the hit box, and a walk-through test dummy with 3 HP gives it something to hit. Every scene and asset change goes through the Unity CLI against the open Editor.

**Model:** Claude Opus 5.5 (Claude Code desktop app, a side session started from the session 6 spec interview)

**Key prompts:**
> Build the brush-attack feature exactly as written in Docs/Specs/brush-attack.md. The spec is the source of truth. Don't add anything that isn't in it, and list ideas as suggestions at the end instead.

> Remove `<Mouse>/leftButton` and `<Keyboard>/enter` from `Player/Attack` in `Assets/Settings/InputSystem_Actions.inputactions`, through Unity, then read the bindings back.

> Check in Play Mode through the CLI that the 20 acceptance criteria hold, and re-check the movement and health criteria. Leave Room01 open in the Editor and saved.

**What the AI produced:**
* PlayerAttack.cs in Assets/_Game/Scripts/Player/: one swing per press of J, a press during a running swing or during the death pause is ignored, the side is locked at the press, and an OverlapBox each physics step (triggers included) hits each Health at most once per swing and never the player's own.
* A read-only Facing property on PlayerController, and four new PlayerConfig fields under an "Attack" header with their values on the asset: attackWidth 1.5, attackHeight 1.2, attackDuration 0.1, attackDamage 1.
* A BrushStroke child on the Player: the placeholder square in blue (#4A90D9), sorting order 20, sized and placed from PlayerConfig at each swing and shown only during it.
* TestDummy.cs and DummyConfig.cs in a new Assets/_Game/Scripts/Enemies/ folder, DummyConfig.asset (maxHealth 3, hitFlashDuration 0.15), and a grey TestDummy in Room01 at x = −2 with a trigger collider, a 0.15 s red flash and deactivation at 0 HP.
* The left mouse button and Enter bindings removed from Attack through the Input System API. J and the gamepad, touch, joystick and XR bindings stay.
* A Play Mode check through the CLI: all 20 criteria plus three edge cases (a press during a swing, a target walking into the box, J held for 2 s), the movement checks and the health checks again. The Console stayed at 0 errors and 0 warnings.

**Accepted:**
* The whole feature as the spec describes it: J only, a 1.5 × 1.2 box for 0.1 s, 1 damage, no cooldown, the blue stroke and the walk-through dummy with 3 HP

**Changed (and why):**
* The AI's first run showed the blue stroke 0.12 units ahead of the drawn player while running, because the sprite is smoothed between physics steps. The AI moved the stroke into the player's local space, so it now sits on the drawn front edge. The hit box itself was already right.

**Rejected (and why):**
* Nothing this session.

**How I verified it:**
I checked it myself in Play Mode, and I reviewed the changes in GitHub Desktop before committing. The AI also checked all 20 acceptance criteria itself in Play Mode, driving the keyboard and mouse through the Unity CLI and measuring each result: the stroke's size and position on both sides (1.50 × 1.20, touching the player's front edge), how long it stays on (0.100 s), the dummy's HP after each swing and its 0.15 s red flash, holding J, fast presses, swinging while running at 7 and at the top of a jump, the mouse and Enter doing nothing, running through the dummy, no swing during the death pause, the dummy coming back after the reload, and attackWidth 3 (set back to 1.5 afterwards). It also checked three edge cases (a press during a swing, a target walking into the box, J held for 2 s), re-ran the 23 movement checks and the health checks, and took a Game view screenshot in the middle of a swing. The Console stayed at 0 errors and 0 warnings.

**Time spent:** About 15 minutes (build and the AI's Play Mode check)

## 2026-11-24 · Session 9 · Enemy types and Chaser spec (interview)

**Goal:** Pick the two enemy types for the game and write the spec for enemy type A, while the brush attack build ran.

**Model:** Claude Opus 5.5 (Claude Code desktop app, the session 4 interview session)

**Key prompts:**
> lets interview for the feature 3 and the rest

> btw from now on you test them yourself, i wont test them and fill everything i will only do the commits and check that everything is right

**What the AI produced:**
* A choice of enemy pairs. I picked a Chaser (enemy A, close range) and a Shooter (enemy B, stands on the platform and fires slow paint blobs).
* 7 questions about the Chaser and Docs/Specs/enemy-type-a.md with 18 acceptance criteria.
* A finding while checking the numbers: a jump straight up doesn't dodge the Chaser, because it waits under the player, but a running jump does. Both are written into the criteria and examples.
* A new way of working from now on: the build sessions test every criterion themselves and fill in the whole Dev Log entry, and I review and commit.

**Accepted:**
* Chaser: 1 × 1, 3 HP, 4 units/s (slower than my 7), notices me within 6 units and then chases for good, waits under the platform
* A touch costs 1 HP, then the Chaser pauses for 1.0 s. I'm never invulnerable, which keeps my Session 1 decision
* A brush hit gives a red flash and −1 HP with no knockback, and I can pass through the Chaser
* One Chaser at x = 6, and the test dummy is removed

**Changed (and why):**
* Who tests: from this point the AI's build sessions test the features themselves and fill in the Dev Log, so I can focus on reviewing and committing.

**Rejected (and why):**
* Knockback on the Chaser, because it could be juggled and never reach me, which makes it too easy.
* A solid Chaser, because I could get pinned against a wall.

**How I verified it:**
I read the Chaser spec and checked that the Decisions table matches my answers before approving it.

**Time spent:** About 20 minutes

## 2026-11-24 · Session 10 · Chaser build (enemy type A)

**Goal:** Build enemy type A, the Chaser, from Docs/Specs/enemy-type-a.md in a fresh session, and remove the test dummy. Every scene and asset change goes through the Unity CLI against the open Editor, and the AI tests all 18 criteria itself in Play Mode.

**Model:** Claude Opus 5.5 (Claude Code desktop app, a side session started from the session 4 interview session)

**Key prompts:**
> Build enemy type A, the Chaser, exactly as written in `Docs/Specs/enemy-type-a.md`. The spec is the source of truth. Don't add anything that isn't in it, and list ideas as suggestions at the end instead.

> Remove the test dummy from Room01, and delete `TestDummy.cs`, `DummyConfig.cs` (if separate) and `DummyConfig.asset` through Unity.

> Test all 18 acceptance criteria yourself in Play Mode through the CLI, measuring the results (velocities, HP, timings). Re-check the movement and health criteria and the brush criteria that don't use the dummy.

**What the AI produced:**
* Chaser.cs and ChaserConfig.cs in Assets/_Game/Scripts/Enemies/, and ChaserConfig.asset (maxHealth 3, moveSpeed 4, noticeRange 6, contactDamage 1, hitPause 1.0, hitFlashDuration 0.15). The Chaser waits until the player's x is within noticeRange, then chases for good along the floor by setting linearVelocity.x. On the last step it moves only the distance that is left, so it stops right under the player without shaking. A touch (OnTriggerStay2D) calls the player's Health.TakeDamage and starts the 1.0 s pause. A brush hit gives a 0.15 s red flash, and at 0 HP the object is deactivated.
* A Chaser in Room01 at x = 6, y = −3.5: a 1 × 1 dark purple (#5A4A6A) square, a trigger BoxCollider2D, a Kinematic Rigidbody2D (Interpolate like the player, and Never Sleep so the touch check keeps running while both stand still), its own Health, and the Player reference set in the Inspector.
* The TestDummy object removed from Room01, and TestDummy.cs, DummyConfig.cs and DummyConfig.asset deleted with AssetDatabase.DeleteAsset. One comment in PlayerAttack.cs that named the dummy now names the Chaser.
* A Play Mode check through the CLI with a frame-by-frame driver: all 18 criteria, the spec's edge cases, the movement, health and brush checks again, and a Game view screenshot of the Chaser mid-chase.

**Accepted:**
* The whole feature as the spec describes it: waits until 6 units, chases for good at 4 units/s, waits still under the platform, 1 HP per touch with a 1.0 s pause, a red flash and −1 HP per brush hit with no knockback, walk-through, and gone at 0 HP
* The test dummy removed from the room and the project

**Changed (and why):**
* The AI's first test run measured the pause as 1.02 s instead of 1.0 s, because adding up 50 physics steps of 0.0199999 s left a tiny remainder and cost one extra step. The AI added a small tolerance to the pause timer, and the next run measured exactly 1.00 s between hits.

**Rejected (and why):**
* Nothing this session.

**How I verified it:**
I checked it myself in Play Mode, and I reviewed the changes in GitHub Desktop before committing. The AI also checked all 18 criteria itself in Play Mode, driving the keyboard through the Unity CLI and measuring each result: the Chaser at (6, −3.5), 1 × 1, #5A4A6A, 3 HP and no dummy; no movement for 5 s with the player at x = −6 and for 3 s at x = −1 (6.01 units away doesn't notice, exactly 6.00 does); Linear Velocity x −4.00 once the player reaches x = 0; −4.00 all the way back to the left wall until it touched the player; standing still under the player on the platform (|dx| 0.0000, velocity 0, no HP lost); touches 3 → 2 → 1 → 0 exactly 1.00 s apart and the reload 1.00 s after death; no second hit when running through it during the pause; a brush hit giving HP 2, a 0.150 s red flash and still −4.00 with no knockback; dead after two more hits, then no damage while walking where it stood; a running jump from 2.00 units away clearing it by 0.28 units and landing on its other side with no damage (a jump straight up lost 1 HP, as the spec says); back at x = 6 with 3 HP after the reload; moveSpeed 2 chasing at −2.00 and noticeRange 10 starting at player x −3.90, each set with Play Mode stopped and set back to 4 and 6 afterwards; only the config and player fields on the Chaser component; the dummy files gone and 0 compile errors. It also checked the spec's edge cases (exactly at noticeRange, a brush hit and a kill during the pause, the fall reset, landing on it, a touch and a brush hit close together), re-ran the 23 movement checks, the health checks and the brush checks that don't use the dummy (with the Chaser switched off for the movement and brush runs, so it couldn't interrupt them), and took a Game view screenshot mid-chase. The Console stayed at 0 errors and 0 warnings.

**Time spent:** About 17 minutes (build and the AI's Play Mode check)

## 2026-11-24 · Session 11 · Shooter, door, painting reveal, main menu and ending specs (interview)

**Goal:** Write the remaining five HA02 specs while the Chaser build ran, so every feature has a spec ready for its build session.

**Model:** Claude Opus 5.5 (Claude Code desktop app, the session 4 interview session)

**Key prompts:**
> ok commited, continue how many features left for this hw ?

**What the AI produced:**
* A status table: 3 features done, 1 building and 5 left to spec and build, then the Windows build.
* Interviews and specs, each approved by me before moving on:
  * Docs/Specs/enemy-type-b.md (the Shooter, 16 acceptance criteria)
  * Docs/Specs/room-clear-door.md (13)
  * Docs/Specs/painting-reveal.md (12)
  * Docs/Specs/main-menu.md (11)
  * Docs/Specs/ending.md (11)
* Two findings while checking the numbers:
  * Blobs must fly through the platform, or almost every shot would hit the platform under the Shooter.
  * The brush can only reach the Shooter from the platform, not from a jump beside it, which corrected something the AI had told me earlier.
* Placeholder PaintingReveal and Ending scenes in the door and painting specs, so each build has a scene to load before the next feature fills it in.

**Accepted:**
* Shooter: on the platform at x = 3, 2 HP, harmless to touch, aimed blobs every 2 s at 5 units/s starting 2 s after the room loads, and the brush swats blobs
* Door: a 1 × 2 door at the bottom of the right wall that glows open 0.5 s after both enemies die, and touching it loads PaintingReveal at once
* Painting reveal: a framed painting of 5 panels, Space to continue to the Ending
* Main menu: the title, a grey painting teaser, Start and Quit (keyboard or mouse) and a controls line
* Ending: "The journey continues…", the painting with part 1 coloured, "1 of 5 parts painted", and Main Menu and Quit

**Changed (and why):**
* Painting reveal: the AI recommended a simple fade, but I picked a brush wipe from top to bottom, because it fits the painter theme. The spec uses a mask so the real HA04 art won't be stretched.

**Rejected (and why):**
* An automatic room end with no door, because the Backlog says the door opens when the room is clear, and walking out feels better.
* A menu without a Quit button, because players would be stuck in the fullscreen Windows build.

**How I verified it:**
I read each spec and checked that its Decisions table matches my answers before approving it.

**Time spent:** About 40 minutes

## 2026-11-24 · Session 12 · Shooter build (enemy type B)

**Goal:** Build enemy type B, the Shooter, from Docs/Specs/enemy-type-b.md in a fresh session. Every scene, prefab and asset change goes through the Unity CLI against the open Editor, and the AI tests all 16 criteria itself in Play Mode.

**Model:** Claude Opus 5.5 (Claude Code desktop app, a side session started from the session 4 interview session)

**Key prompts:**
> Build enemy type B, the Shooter, exactly as written in `Docs/Specs/enemy-type-b.md`. The spec is the source of truth. Don't add anything that isn't in it, and list ideas as suggestions at the end instead.

> Test all 16 acceptance criteria yourself in Play Mode through the CLI, measuring the results (timings, velocities, HP). Re-check the earlier specs' criteria (movement, health, brush, Chaser), switching enemies off where they would interrupt an older check.

**What the AI produced:**
* Shooter.cs, ShooterConfig.cs and PaintBlob.cs in Assets/_Game/Scripts/Enemies/, and ShooterConfig.asset (maxHealth 2, firstShotDelay 2.0, fireInterval 2.0, blobSpeed 5, blobSize 0.4, blobDamage 1, hitFlashDuration 0.15). The Shooter fires its first blob 2.0 s after the room loads, then one every 2.0 s, aimed in a straight line at the player's centre at the moment of firing (straight left if the player stands exactly inside it). The timer counts from the planned time, so the rhythm doesn't drift. A brush hit gives a 0.15 s red flash, and at 0 HP the object is deactivated, so it stops firing.
* Assets/_Game/Prefabs/PaintBlob.prefab, made through the Editor: a 1 × 1 #E0603A square (the Shooter scales it to 0.4 × 0.4), a trigger BoxCollider2D, a Kinematic Rigidbody2D moved by linearVelocity, and PaintBlob. A blob disappears on touching a RoomBounds or the player (found with TryGetComponent<PlayerController>(), then its Health), and flies through everything else.
* RoomBounds.cs in Rooms, an empty marker, on the Floor, WallLeft, WallRight and Ceiling (not on the Platform).
* ISwattable.cs in Core. PaintBlob implements it, and PlayerAttack now calls Swat() on anything swattable in its box, so the Player code doesn't depend on the Enemies code. A blob holds its damage until LateUpdate, so a swing started in the same frame as the touch still swats it first (one of the spec's edge cases).
* One Shooter in Room01 at (3, −1.0): a 1 × 1 #3F5A5A square with a trigger collider, its own Health, and the config, the Player and the blob prefab set in the Inspector.
* A Play Mode check through the CLI with frame-by-frame drivers: all 16 criteria, the spec's edge cases, and the movement, health, brush and Chaser drivers from Session 10 again.

**Accepted:**
* The whole feature as the spec describes it: standing on the platform with 2 HP, blobs at 2, 4, 6 s aimed where the player was, at 5 units/s, 1 HP per blob, through the platform and the enemies, stopped by the floor, the walls and the ceiling, swatted by the brush, harmless to touch, gone at 0 HP, and back after the reload

**Changed (and why):**
* No code changed after the first test run. Two of the AI's checks had to be run again because of the test, not the game: the first "run away" check (C5) only ran the player to x = −2.4, which is still on the blob's line, so the blob hit it. The re-run ran to x = −0.5, and the blob missed. A later run got no keyboard input at all, because the Game view had lost focus. Restarting Play Mode right before each driver fixed that.

**Rejected (and why):**
* Nothing this session.

**How I verified it:**
I checked it myself in Play Mode, and I reviewed the changes in GitHub Desktop before committing. The AI also tested all 16 acceptance criteria in Play Mode: the first blob after 2 s and then one every 2 s, blobs aimed at the player and flying through the platform, swatting a blob with the brush, two hits from the platform killing the Shooter, the Shooter coming back after a restart, and changing the config values. It also re-ran the checks from the earlier specs, and the Console stayed at 0 errors and 0 warnings.

**Time spent:** About 15 minutes (build and the AI's Play Mode check)

## 2026-11-24 · Session 13 · Room clear and door build

**Goal:** Build the room-clear-and-door feature from Docs/Specs/room-clear-door.md in a fresh session. Every scene and asset change goes through the Unity CLI against the open Editor, and the AI tests all 13 criteria itself in Play Mode.

**Model:** Claude Opus 5.5 (Claude Code desktop app, a side session started from the session 4 interview session)

**Key prompts:**
> Build the room-clear-and-door feature exactly as written in `Docs/Specs/room-clear-door.md`. The spec is the source of truth. Don't add anything that isn't in it, and list ideas as suggestions at the end instead.

> Test all 13 acceptance criteria yourself in Play Mode through the CLI. Re-check the earlier specs' criteria, switching enemies off where they'd interrupt an older check. Make sure the Console is clean.

**What the AI produced:**
* RoomConfig.cs, RoomClear.cs and Door.cs in Assets/_Game/Scripts/Rooms/, and RoomConfig.asset (doorOpenDelay 0.5). RoomClear listens to the Died event of every Health in its Inspector list (the Chaser and the Shooter). When all of them are dead, in any order or in the same frame, it opens the door once, doorOpenDelay seconds later.
* The door in Room01 at x 8 to 9, y −4 to −2: a "Door" object with the yellow glow (#F2D27A), the exit trigger, RoomBounds and Door, and a "ClosedBlock" child with the dark brown-grey block (#4A4038), its solid collider and RoomBounds. Opening turns the block off and the glow and the trigger on. Touching the open doorway (the player found with TryGetComponent<PlayerController>(), ignored while its Health is dead) loads PaintingReveal. Turning the trigger on also catches a player who is already standing in the doorway.
* The right wall shortened to y −2 to 5, a RoomClear object with the Chaser's and the Shooter's Health, the Door and the RoomConfig set in the Inspector.
* Assets/_Game/Scenes/PaintingReveal.unity as a placeholder (only an orthographic camera with a plain #1E1E1E background), and the build list set to Room01, then PaintingReveal.
* A Play Mode check through the CLI with frame-by-frame drivers: all 13 criteria, the spec's edge cases, and the movement, health, brush, Chaser and Shooter drivers from Sessions 10 and 12 again.

**Accepted:**
* The whole feature as the spec describes it: a solid door while any enemy lives, the glow 0.5 s after the last kill, PaintingReveal the moment the living player touches the doorway (also when already pressed against it), blobs stopped at the doorway closed or open, and a closed door with both enemies back after a reload

**Changed (and why):**
* No game code changed after the first test run. One check (the blobs at the door) had to be run again because of the test, not the game: the driver had kept a reference to the blob prefab from an earlier command, which Unity had already thrown away. The re-run took the prefab from the Shooter's own Inspector field, and every blob disappeared at the door.

**Rejected (and why):**
* Nothing this session.

**How I verified it:**
I checked it myself in Play Mode, and I reviewed the changes in GitHub Desktop before committing. The AI also tested all 13 acceptance criteria in Play Mode: the closed door and the shorter wall, the player stopping at the door, the door staying closed with one enemy left, the glow half a second after the last kill, PaintingReveal loading on touch and when already pressed against the door, the reload closing the door again, blobs stopping at the door, the build list, the delay set to 2 on the config and the Inspector fields. It also re-ran the checks from the earlier specs, and the Console stayed at 0 errors and 0 warnings.

**Time spent:** About 15 minutes (build and the AI's Play Mode check)

## 2026-11-24 · Session 14 · Painting reveal build

**Goal:** Build the painting reveal from Docs/Specs/painting-reveal.md in a fresh session. Every scene and asset change goes through the Unity CLI against the open Editor, and the AI tests all 12 criteria itself in Play Mode.

**Model:** Claude Opus 5.5 (Claude Code desktop app, a side session started from the session 4 interview session)

**Key prompts:**
> Build the painting reveal exactly as written in `Docs/Specs/painting-reveal.md`. The spec is the source of truth. Don't add anything that isn't in it, and list ideas as suggestions at the end instead.

> Test all 12 acceptance criteria yourself in Play Mode through the CLI. Take a Game view screenshot mid-wipe and after it to confirm the look. Re-check that Room01's door still loads PaintingReveal, and the earlier specs' main checks. Make sure the Console is clean.

**What the AI produced:**
* PaintingConfig.cs and PaintingReveal.cs in Assets/_Game/Scripts/UI/, and PaintingConfig.asset (revealedPart 1, totalParts 5, wipeDelay 0.5, wipeDuration 1.5).
* The painting in PaintingReveal, centred at (0, 0.5): a dark brown frame (#5A3E2B) 0.3 thick around five grey panels (#6A6A6A), and over panel 1 the placeholder landscape (sky blue #7EC8E3 on top, grass green #6DBE5A below) set to "Visible Inside Mask". A SpriteMask under a pivot at the panel's top edge grows down at an even speed, so only the mask is scaled and the art is never stretched.
* "Part 1 of 5" (white) and "Press Space to continue" (light grey, smaller) in TextMeshPro on a uGUI canvas under the painting. The TMP Essential Resources were imported, because the project didn't have them yet.
* The script hides the text, waits wipeDelay, runs the wipe over wipeDuration, then shows both lines in the same frame. From the next frame on, a new press of the Jump action (the same InputActionReference the player uses) loads Ending, once.
* Assets/_Game/Scenes/Ending.unity as a placeholder (only a camera with the plain #1E1E1E background), and the build list set to Room01, PaintingReveal, Ending.
* A Play Mode check through the CLI with frame-by-frame drivers: all 12 criteria, the spec's edge cases, two Game view screenshots, and the movement, health, brush, Chaser, Shooter and door drivers from Sessions 10, 12 and 13 again.

**Accepted:**
* The whole feature as the spec describes it: the grey framed painting with no text at the start, the colour brushed down panel 1 from 0.5 s to 2.0 s, the two lines appearing together, Space ignored during the wipe and while held from before the prompt, and the Ending loading on a new press
* A thin strip of frame colour between neighbouring panels (each panel is drawn 1.9 wide in its 2-wide slot), because five touching panels of the same flat grey would look like one grey block

**Changed (and why):**
* The first test run showed the reveal script with no config and no input action. The setup had assigned them and then opened another scene before saving, which threw the references away. The AI assigned them again with the scene open, and the next run passed. No game code changed after that.

**Rejected (and why):**
* Nothing this session.

**How I verified it:**
I played the reveal myself: I watched the colour brush down the first panel, tried pressing Space too early, and continued to the Ending. I also went through the diff in GitHub Desktop before committing. On top of that, the AI ran all 12 acceptance criteria in Play Mode (including a full run from Room01 through the door and a longer wipe set in the config) and re-checked the earlier features, with no errors or warnings in the Console.

**Time spent:** About 15 minutes (build and the AI's Play Mode check)
