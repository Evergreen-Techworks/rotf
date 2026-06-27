#!/usr/bin/env bash
# Find dungeon/world ENTRIES in a long stream by detecting loading-screen (black) segments,
# then grab the frames right AFTER each black segment (= the new area just loaded in).
# RotMG fades to black while loading a world, so each black segment ≈ "entered a new dungeon".
# Usage:  ./detect_transitions.sh <video> [Tag]
set -euo pipefail
cd "$(dirname "$0")/../.."
V="$1"; TAG="${2:-$(basename "$V" | sed 's/\.[^.]*$//')}"
OUT="docs/video-recovery/frames/${TAG}/transitions"; mkdir -p "$OUT"
# bossends = frames just BEFORE each black/loading screen. You fade to black when LEAVING a
# dungeon/world, and you typically leave right after the boss dies -> the seconds before
# black_start are the boss-fight CLIMAX (densest bullets, boss on screen) = best source for
# attack patterns + phases. (Symmetric to transitions/, which are the post-load ENTRY frames.)
BOSS="docs/video-recovery/frames/${TAG}/bossends"; mkdir -p "$BOSS"

echo "scanning $V for black/loading segments (this reads the whole file once)..."
# d=min black duration (s); pix_th=how dark a pixel counts as black; pic_th=fraction of black pixels
ffmpeg -nostdin -hide_banner -i "$V" -vf "blackdetect=d=0.10:pix_th=0.10:pic_th=0.85" -an -f null - 2>&1 \
  | grep -oE "black_start:[0-9.]+ black_end:[0-9.]+" > /tmp/${TAG}_black.txt || true
N=$(wc -l < /tmp/${TAG}_black.txt)
echo "found $N black/loading transitions"

i=0
PREV_END=0   # black_end of the previous loading screen = when we ENTERED the dungeon we're now leaving
while read -r line; do
  i=$((i+1))
  START=$(echo "$line" | grep -oE "black_start:[0-9.]+" | cut -d: -f2)
  END=$(echo "$line" | grep -oE "black_end:[0-9.]+" | cut -d: -f2)
  # ENTRY: grab frames at +1s, +3s, +6s after the load (new area now visible)
  for off in 1 3 6; do
    T=$(python3 -c "print($END + $off)")
    ffmpeg -nostdin -hide_banner -y -ss "$T" -i "$V" -frames:v 1 \
      "$OUT/t$(printf %03d $i)_at${END%.*}s_+${off}.png" 2>/dev/null || true
  done
  # BOSS FIGHT: dense burst over the WHOLE pre-fade window so we can watch the FULL fight (all
  # phases/attack rotations), not just the climax. Window = the last up-to-150s before the fade,
  # but never earlier than when we entered this dungeon (PREV_END+2). ~1 frame / 4s, ONE ffmpeg
  # call per window (fast input-seek) -> efficient even on 100+-transition marathons.
  read -r LO DUR < <(python3 -c "
s=$START; pe=$PREV_END
lo=max(pe+2, s-150, 2); dur=(s-1)-lo
print(f'{lo:.2f} {dur:.2f}' if dur>=2 else '0 0')")
  if [ "$DUR" != "0" ]; then
    ffmpeg -nostdin -hide_banner -y -ss "$LO" -i "$V" -t "$DUR" -vf "fps=1/4" \
      "$BOSS/b$(printf %03d $i)_${LO%.*}s_%03d.png" 2>/dev/null || true
  fi
  PREV_END=$END
done < /tmp/${TAG}_black.txt

echo "extracted post-load ENTRY frames -> $OUT  ($(ls "$OUT" 2>/dev/null | wc -l) frames)"
echo "extracted FULL-FIGHT BOSS-END frames -> $BOSS  ($(ls "$BOSS" 2>/dev/null | wc -l) frames) [attack-pattern source]"
echo "Each t### group = a dungeon ENTRY; each b### group = the FULL boss fight (dense ~4s burst) just before leaving that dungeon."
