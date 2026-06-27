# Claude Code vision /loop — ROTF dungeon recovery from frames

Claude Code reads the extracted frames itself (it has vision) and writes the dungeon specs —
no external API. Each loop iteration processes **one** dungeon, so it's resumable and grinds
through unattended.

## Prep (per dungeon, once)
```bash
# downloads + extracts frames into docs/video-recovery/frames/<Dungeon>/
./scripts/video/prep_frames.sh "Asgard" https://www.youtube.com/watch?v=-N3MH_woifQ
# (or a local file, with optional boss-fight start/end for a high-fps burst:)
./scripts/video/prep_frames.sh "Asgard" /path/to/asgard.mp4 12:30 16:00
```

## Kick off the loop
In Claude Code, paste:

> `/loop` Process the next pending ROTF dungeon in `docs/video-recovery/frames/`:
> 1. List `docs/video-recovery/frames/*/`. Pick the first `<Dungeon>` dir that does NOT yet have
>    `docs/video-recovery/<Dungeon>.json`. If every dir has one, say "all done" and stop.
> 2. Read its frames with vision: ALL `chat_*.png` (transcribe boss taunts + loot text **VERBATIM**),
>    ALL `minimap_*.png` (describe the map shape: arena/linear/branching, #rooms, boss room, biome),
>    and ~12 evenly-spaced `f_*.png`/`boss_*.png` (describe the boss's attacks concretely — bullet
>    count/shape/direction, phase changes + color flashes, minion spawns).
> 3. Write `docs/video-recovery/<Dungeon>.json` in the schema from `docs/VIDEO_RECOVERY_PIPELINE.md`
>    (dungeon, map, bosses[{name,phases,attacks,minions,colorFlashes,taunts}], drops, uncertain).
> 4. Run `python3 scripts/video/04_merge.py` to fold it into `docs/DUNGEON_SPECS.json`.
> 5. Report what you recovered + label DOCUMENTED-from-video vs uncertain. Then continue the loop.

(Give `/loop` an interval like `/loop 300` to self-pace, or plain `/loop` and it decides cadence.)

## Frame-region tuning (important)
The `crop=` values in `prep_frames.sh` assume a ~1280×720 layout. If `chat_*.png`/`minimap_*.png`
don't capture the right region for a given video, open one frame, eyeball the pixel box, and adjust
`crop=W:H:X:Y`. Overview `f_*.png` (full screen) always works as a fallback.

## When the loop finishes
`docs/DUNGEON_SPECS.json` now has the video-recovered maps/attacks/taunts/drops merged in. Tell the
main build session **"rebuild dungeons from specs"** → it turns each spec into `Taunt(...)`,
`Shoot`/`Spawn`/phase behaviors, `ItemLoot` drops, and a `.jm` map, then builds + verifies.
