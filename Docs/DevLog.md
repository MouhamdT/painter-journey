# Dev Log

## 2026-11-11 · Session 1 · Feature backlog with MoSCoW

**Goal:** Get a list of features for my game and sort them with MoSCoW for Backlog.md.

**Model:** Claude Opus 5.5

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

**Model:** Claude Opus 5.5

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
