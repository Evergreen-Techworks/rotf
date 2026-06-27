# ROTF Dungeon Recovery from Gameplay Video — Setup Guide

The exact maps / shot patterns / taunts only survive in **gameplay footage**. RotMG renders
everything as crisp 2D, so a vision model can read it reliably:
- **Chat box** (bottom-left) = literal text → boss **taunts** + **loot** notifications (verbatim, high accuracy)
- **Minimap** (top-right) = the dungeon **layout** (rooms/corridors/arena)
- **Main view** = the boss + **projectile patterns** + phase color-flashes + minion spawns

**Accuracy you can expect:** taunts/chat ★★★ (it's text), drops ★★★, boss attack *description* ★★☆,
exact shot angles/timings ★★☆ (needs frame bursts), full tile-map ★☆☆ (minimap gives shape, not every tile).

---

## Pick a vision engine (ranked by effort)

**A. Gemini (EASIEST — native video input).** Gemini 1.5/2.x Pro/Flash accept a video file
directly (up to ~1 hr) and analyze motion across time — ideal for shot patterns. One call per
dungeon video. This is the recommended path.

**B. Claude / GPT-4o on extracted frames.** No native video; you sample frames (ffmpeg) and send
batches of images. More plumbing, but works with any vision LLM. Use frame *bursts* during boss
fights to capture patterns.

**C. Tesseract OCR on the chat-box crop (precision add-on).** Pure-text OCR of just the chat
region → exact taunt/drop strings, no LLM hallucination. Run alongside A or B to verify quotes.

---

## Pipeline (4 stages)

### 1. Acquire — `scripts/video/01_fetch.sh`
```bash
# yt-dlp (pip install yt-dlp). Download target videos at 720p+ (sprites must be legible).
yt-dlp -f 'bestvideo[height>=720]+bestaudio/best' -o 'videos/%(id)s.%(ext)s' \
  --download-sections "*0:00-60:00" "<YOUTUBE_URL>"
# A whole playlist:
yt-dlp -f 'bv[height>=720]+ba/b' -o 'videos/%(playlist_index)s-%(id)s.%(ext)s' \
  "https://www.youtube.com/playlist?list=PLT0HJvAhlmVQpR1Jlv063wtBBmK5be-wA"
```

### 2. Frames + region crops (only needed for path B/C) — `scripts/video/02_frames.sh`
```bash
V="$1"; OUT="frames/$(basename "$V" .mp4)"; mkdir -p "$OUT"
ffmpeg -i "$V" -vf fps=2 "$OUT/f_%05d.png"                      # 2 fps general
# boss-fight burst (pass start/end as args): higher fps captures bullet patterns
ffmpeg -ss "$2" -to "$3" -i "$V" -vf fps=8 "$OUT/boss_%05d.png" # 8 fps during a fight
# crop regions (RotMG 1280x720 layout; adjust to the video's resolution):
#   chat box  ~ bottom-left  : crop=440:150:8:560
#   minimap   ~ top-right     : crop=180:180:1090:8
ffmpeg -i "$V" -vf "fps=1,crop=440:150:8:560" "$OUT/chat_%05d.png"
ffmpeg -i "$V" -vf "fps=0.5,crop=180:180:1090:8" "$OUT/minimap_%05d.png"
```

### 3. Analyze — `scripts/video/03_analyze.py`
Sends the video (Gemini) or frame batches (Claude/GPT-4o) with the **extraction prompt** below,
gets a structured JSON spec per dungeon. (Skeleton script provided; plug in your API key.)

### 4. Aggregate → spec — `scripts/video/04_merge.py`
Merges all per-video/per-segment observations for a dungeon into one spec matching the
`DUNGEON_SPEC` schema in `docs/DUNGEON_SPECS.json`, then it feeds the build (gen_dungeons /
BehaviorDb / loot). Dedupe taunts, union drops, pick the most-detailed map description.

---

## THE EXTRACTION PROMPT (the heart of it — give this to the vision model)

> You are analyzing gameplay footage of the RotMG private server **Revenge of the Fallen** to
> reconstruct the dungeon **"{DUNGEON}"** (boss(es): {BOSSES}). RotMG is a top-down 2D bullet-hell.
> On screen: the **player** is centered; **enemies/bosses** are larger sprites; **projectiles** are
> small bright bullets; the **minimap** (top-right) shows the dungeon layout; the **chat box**
> (bottom-left) shows text — system messages, **boss taunts** (often `BossName: ...` or a colored
> announcement), and **loot** notifications.
>
> Produce JSON:
> ```
> {
>  "dungeon": "...",
>  "map": "from the minimap: overall shape (arena/linear/branching), #rooms, boss-room location, biome/tile colors/theme",
>  "bosses": [{
>    "name": "...",
>    "phases": ["phase 1: <what attacks>", "phase 2 (triggers when ...): <attacks>"],
>    "attacks": ["e.g. 'spiral of ~8 bullets clockwise'", "'shotgun spread of 5'", "'expanding ring'"],
>    "minions": ["names/looks of summoned adds"],
>    "colorFlashes": "any color flash on phase change",
>    "taunts": ["VERBATIM chat text, in quotes, exactly as shown"]
>  }],
>  "drops": ["item names seen in loot bags / loot chat"],
>  "timestamps": "note where key events happen",
>  "uncertain": "anything you couldn't read clearly"
> }
> ```
> Rules: transcribe chat/taunt text **EXACTLY** (don't paraphrase). Describe attacks concretely
> (count, shape, direction). If unsure, say so in "uncertain" rather than inventing.

For OCR (path C), no prompt — just `tesseract chat_00001.png - --psm 6` on each chat crop, then
grep for `:` lines / known boss names to pull taunts.

---

## Long streams: auto-find dungeon entries via loading screens (`detect_transitions.sh`)

RotMG fades to **black** while loading a world, so every dungeon/area entry = a short black segment.
`scripts/video/detect_transitions.sh <video> [Tag]` runs ffmpeg `blackdetect`, finds those segments,
and extracts the frames just AFTER each (the new area, now loaded) into
`docs/video-recovery/frames/<Tag>/transitions/`. A 4-hour stream collapses to ~N entry snapshots —
read each `t###_*` group to ID which dungeon it is, then sample that segment's fight.
(Proven: caught the Asgard arena entry at 34.9s in the test clip. Tune `pix_th`/`pic_th` if a
dungeon's load screen isn't pure black.) Brightness fallback: `signalstats` YAVG≈0 == loading.
**Use this on the GalacticZones/Scorched/General streams to reach the dungeons that lack dedicated videos.**

## Run it autonomously "over time"

Make a queue + loop so it grinds through videos unattended:
```bash
# videos/queue.txt = one YouTube URL per line, tagged with the dungeon:
#   Asgard|https://youtube.com/watch?v=-N3MH_woifQ
#   GTU|https://youtube.com/watch?v=...
while read line; do
  DUN="${line%%|*}"; URL="${line##*|}"
  bash scripts/video/01_fetch.sh "$URL"
  python3 scripts/video/03_analyze.py --dungeon "$DUN" --video "videos/$(...).mp4" \
     --out "docs/video-recovery/$DUN.json"
done < videos/queue.txt
```
Or schedule it: a cron / Claude Code `/loop` that processes **one video per run**, appends to the
spec, and stops when the queue is empty. Each run is independent, so it survives interruptions and
builds up `docs/video-recovery/*.json` over hours/days.

---

## Feeding results back into the game

Each `docs/video-recovery/<Dungeon>.json` merges into `docs/DUNGEON_SPECS.json`, then drives:
- **Taunts** → `Taunt("...")` lines in the dungeon's `BehaviorDb.<Dungeon>.cs`
- **Attacks/phases** → `State`/`Shoot`/`Spawn`/`HpLessTransition` in that behavior
- **Drops** → `ItemLoot(...)` thresholds (create the item if missing)
- **Map** → the `.jm` layout via `scripts/maps/make_jm.py` (shape from the minimap)

Then `scripts/gen_dungeons.py`-style build + the WSL client/server build verifies it.

---

## Target videos (start here)
- **Asgard (closed testing):** `youtube.com/watch?v=-N3MH_woifQ`
- **ROTF playlist:** `youtube.com/playlist?list=PLT0HJvAhlmVQpR1Jlv063wtBBmK5be-wA`
- Search per dungeon: `"revenge of the fallen" rotmg <DungeonName>` on YouTube; prefer **boss-kill /
  "first clear" / dungeon-guide** videos (they linger on the fight + show drops + chat).

## Best-bang order (what to capture first)
1. **Chat/taunts + drops** (path C OCR or any vision) — cheapest, highest accuracy, recovers the
   "chat messages" + confirms loot.
2. **Boss attack descriptions** (Gemini video) — enough to author faithful `Shoot`/phase behaviors.
3. **Map shape** (minimap) — enough to lay out a faithful `.jm`.
