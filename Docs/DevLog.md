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
