#!/usr/bin/env bash
# Find MORE ROTF videos and append new ones to scripts/video/queue.txt, so the overnight prep+analysis
# never runs dry. Searches many terms (weighted toward the no-footage dungeons), dedupes by video id,
# filters obvious non-ROTF, prefers videos > 60s. Quick: metadata-only (no downloads).
set -uo pipefail
cd "$(dirname "$0")/../.."
export PATH="$HOME/.local/bin:$PATH"
Q="scripts/video/queue.txt"; SEEN="scripts/video/discovered.txt"; LOG="docs/video-recovery/discover.log"
touch "$SEEN"
# seed SEEN with ids already in the queue (so we never re-add)
grep -oE "youtu\.be/[A-Za-z0-9_-]{11}" "$Q" 2>/dev/null | sed "s#.*/##" >> "$SEEN"
sort -u "$SEEN" -o "$SEEN"

TERMS=(
  "revenge of the fallen rotmg dungeon" "revenge of the fallen rotmg boss" "rotf rotmg RPE"
  "revenge of the fallen rotmg gameplay" "rotf rotmg stream" "revenge of the fallen rotmg white bag"
  "revenge of the fallen craig castle" "revenge of the fallen gate to the underworld"
  "revenge of the fallen tomb of decaying death" "revenge of the fallen broken forest"
  "revenge of the fallen the showcase" "revenge of the fallen rotmg event boss"
  "revenge of the fallen asgard" "revenge of the fallen scorched plains" "rotf rotmg galactic zones"
  "rotf rotmg first clear" "revenge of the fallen rotmg new dungeon"
)
TMP=$(mktemp)
for t in "${TERMS[@]}"; do
  yt-dlp --flat-playlist --print "%(id)s|%(duration)s|%(title)s" "ytsearch12:$t" 2>/dev/null >> "$TMP" || true
done

added=0
while IFS='|' read -r id dur title; do
  [ -z "${id:-}" ] && continue
  [ "${#id}" -ne 11 ] && continue
  echo "$title" | grep -qiE "rotf|fallen" || continue                 # keep ROTF-ish only
  echo "$title" | grep -qiE "action movie|transformers|hollywood|optimus|trailer|reaction" && continue
  [ "${dur:-0}" -lt 60 ] 2>/dev/null && continue                       # skip <60s
  grep -qxF "$id" "$SEEN" && continue                                  # already seen
  echo "$id" >> "$SEEN"
  echo "disc_${id}|https://youtu.be/${id}" >> "$Q"
  added=$((added+1))
done < "$TMP"
rm -f "$TMP"
echo "[$(date '+%m-%d %H:%M')] discovery: +$added new videos (queue now $(grep -cE '^[^#]' "$Q"))" >> "$LOG"
echo "added $added new videos; queue now $(grep -cE '^[^#]' "$Q")"
