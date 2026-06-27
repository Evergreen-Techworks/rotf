#!/usr/bin/env bash
# Prepare a per-dungeon frame set for the Claude Code vision /loop to analyze.
# Downloads (if a URL) or uses a local file, then extracts general frames + chat-box & minimap crops
# into docs/video-recovery/frames/<Dungeon>/.  Needs ffmpeg (present); yt-dlp only if given a URL.
#
# Usage:  ./prep_frames.sh "Asgard" https://youtube.com/watch?v=-N3MH_woifQ
#         ./prep_frames.sh "Asgard" /path/to/local.mp4  [bossStart bossEnd]
set -euo pipefail
cd "$(dirname "$0")/../.."
DUN="$1"; SRC="$2"; OUT="docs/video-recovery/frames/${DUN// /_}"; mkdir -p "$OUT"

if [[ "$SRC" == http* ]]; then
  command -v yt-dlp >/dev/null || { echo "pip install yt-dlp first"; exit 1; }
  mkdir -p videos
  yt-dlp -f 'bestvideo[height>=720]+bestaudio/best/best' -o "videos/${DUN// /_}.%(ext)s" "$SRC"
  V=$(ls -t videos/${DUN// /_}.* | head -1)
else
  V="$SRC"
fi

echo "extracting frames from $V -> $OUT"
ffmpeg -y -i "$V" -vf fps=1 "$OUT/f_%05d.png" 2>/dev/null                          # 1 fps overview
ffmpeg -y -i "$V" -vf "fps=1,crop=440:150:8:ih-160" "$OUT/chat_%05d.png" 2>/dev/null    # chat box (taunts/drops) — TUNE crop to the video
ffmpeg -y -i "$V" -vf "fps=0.5,crop=190:190:iw-200:8" "$OUT/minimap_%05d.png" 2>/dev/null # minimap (map shape)
[ "${3:-}" != "" ] && ffmpeg -y -ss "$3" -to "$4" -i "$V" -vf fps=6 "$OUT/boss_%05d.png" 2>/dev/null  # boss-fight burst
echo "$OUT ready:  $(ls "$OUT" | wc -l) frames (f_*=overview, chat_*=taunts/drops, minimap_*=map, boss_*=fight)"
echo "Now run the Claude Code loop (see docs/video-recovery/LOOP_TASK.md)."
