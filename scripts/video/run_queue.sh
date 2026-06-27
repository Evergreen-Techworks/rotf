#!/usr/bin/env bash
# Autonomous "over time" driver: processes the NEXT unprocessed video in the queue, then exits.
# Run on a loop/cron so it grinds through the queue unattended and builds up docs/video-recovery/.
# queue file lines:  DungeonName|youtube_url
# Needs: yt-dlp, a vision provider key (see 03_analyze.py). PROVIDER env picks the model.
set -euo pipefail
cd "$(dirname "$0")/../.."
Q="${1:-scripts/video/queue.txt}"
DONE="scripts/video/.done"; touch "$DONE"
PROVIDER="${PROVIDER:-gemini}"

while IFS='|' read -r DUN URL; do
  [ -z "${DUN:-}" ] && continue
  grep -qxF "$URL" "$DONE" && continue                       # already processed
  echo "=== $DUN  <-  $URL ==="
  bash scripts/video/01_fetch.sh "$URL" videos
  VID=$(ls -t videos/* | head -1)
  if [ "$PROVIDER" = "gemini" ]; then
    python3 scripts/video/03_analyze.py --provider gemini --dungeon "$DUN" --video "$VID" \
      --out "docs/video-recovery/${DUN// /_}.json"
  else
    bash scripts/video/02_frames.sh "$VID"
    python3 scripts/video/03_analyze.py --provider "$PROVIDER" --dungeon "$DUN" \
      --frames "frames/$(basename "$VID" | sed 's/\.[^.]*$//')" --out "docs/video-recovery/${DUN// /_}.json"
  fi
  echo "$URL" >> "$DONE"
  python3 scripts/video/04_merge.py
  echo "=== done $DUN (one per run; re-run for the next) ==="
  exit 0
done < "$Q"
echo "queue empty — all videos processed."
