#!/usr/bin/env bash
# Extract frames + region crops from a video (for the frames path / OCR). Needs ffmpeg.
# Usage: ./02_frames.sh <video.mp4> [bossStart bossEnd]   e.g. ./02_frames.sh a.mp4 12:30 16:00
set -euo pipefail
V="$1"; OUT="frames/$(basename "$V" | sed 's/\.[^.]*$//')"; mkdir -p "$OUT"
ffmpeg -y -i "$V" -vf fps=2 "$OUT/f_%05d.png" 2>/dev/null                       # general 2 fps
# chat box (bottom-left) + minimap (top-right) crops — TUNE x/y/w/h to the video's resolution!
ffmpeg -y -i "$V" -vf "fps=1,crop=440:150:8:ih-160" "$OUT/chat_%05d.png" 2>/dev/null   # taunts/drops
ffmpeg -y -i "$V" -vf "fps=0.5,crop=190:190:iw-200:8" "$OUT/minimap_%05d.png" 2>/dev/null # map shape
if [ "${2:-}" != "" ]; then
  ffmpeg -y -ss "$2" -to "$3" -i "$V" -vf fps=8 "$OUT/boss_%05d.png" 2>/dev/null  # boss-fight burst
fi
echo "frames -> $OUT  (general=f_*, chat=chat_*, minimap=minimap_*, boss=boss_*)"
echo "TIP: OCR taunts:  for c in $OUT/chat_*.png; do tesseract \"\$c\" - --psm 6; done | sort -u"
