# Overnight ROTF recovery run (started 2026-06-02)

Two autonomous processes gather dungeon data while you sleep:

## 1. bulk_prep.sh — data gathering (background shell, no AI)
Downloads every video in `scripts/video/queue.txt` (~18), extracts overview frames + chat-box &
minimap crops, runs `blackdetect` transition detection (dungeon entries), builds a contact sheet,
deletes the source video to save disk. Resilient: skips `.prepped` dirs, continues on errors.
- Live log: `docs/video-recovery/bulk_prep.log`
- Output: `docs/video-recovery/frames/<Tag>/` (f_*, chat_*, minimap_*, transitions/, _contact.png, .prepped)

## 2. Analysis cron e92e4c80 — vision → specs (fires ~every 17 min: :07 :24 :41 :58)
Each fire re-invokes Claude Code to analyze ONE prepped frame set with vision: reads the contact
sheet + chat crops (taunts/drops VERBATIM) + minimap + transition frames (identifies which dungeon
each entry is; flags Craig's Castle / Gate to the Underworld / ToDD / Broken Forest / Showcase —
the no-footage dungeons). Writes `docs/video-recovery/<Tag>.json`, merges into `DUNGEON_SPECS.json`,
logs a line to `docs/video-recovery/PROGRESS.md`.

## ⚠️ REQUIREMENT: leave the Claude Code terminal OPEN
The cron only fires while this session is alive and idle. If you close Claude Code, analysis stops
(bulk_prep is a detached shell and keeps going regardless). So: leave the terminal open overnight.

## What you'll wake up to
- All ~18 videos downloaded, framed, and transition-detected
- A `docs/video-recovery/<Tag>.json` spec per analyzed video (map / bosses / attacks / taunts / drops)
- `PROGRESS.md` = a running log of what was recovered
- Likely IDs for the no-footage dungeons from the long-stream transitions

## When you're back
1. Read `docs/video-recovery/PROGRESS.md` + the `<Tag>.json` specs.
2. Say **"rebuild dungeons from specs"** → I turn them into real `Taunt(...)`/`Shoot`/phase
   behaviors, `ItemLoot` drops, and `.jm` maps, then build + verify.
3. Separately: the earlier **projectile-crash fix needs a server restart** (`Ctrl+C` → `./run-local.sh`)
   to take effect in-game.

## Check status anytime
`tail docs/video-recovery/bulk_prep.log` · `cat docs/video-recovery/PROGRESS.md` ·
`ls docs/video-recovery/*.json`
