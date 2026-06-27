#!/usr/bin/env bash
# OVERNIGHT bulk prep: for every video in queue.txt — download, extract overview frames +
# chat-box & minimap crops, run blackdetect transition detection, build a contact sheet, then
# delete the source video to save disk. Resilient: skips already-prepped, continues on errors,
# logs progress. The analysis cron reads the frames it produces. Run in the background.
set -uo pipefail
cd "$(dirname "$0")/../.."
export PATH="$HOME/.local/bin:$PATH"
# single-instance lock so a watchdog can safely relaunch us (picks up queue additions / restarts if died)
exec 9>/tmp/rotf_bulkprep.lock
flock -n 9 || { echo "[$(date +%H:%M)] bulk_prep already running — exit" >> "docs/video-recovery/bulk_prep.log"; exit 0; }
Q="scripts/video/queue.txt"
LOG="docs/video-recovery/bulk_prep.log"
echo "=== bulk_prep started $(date) ===" >> "$LOG"

prep_one() {
  local TAG="$1" URL="$2"
  local DIR="docs/video-recovery/frames/$TAG"
  [ -f "$DIR/.prepped" ] && { echo "[skip] $TAG already prepped" >> "$LOG"; return; }
  # disk-space guard: stop the whole run if under 6 GB free (104-video queue could fill disk)
  local FREE; FREE=$(df -P . | awk 'NR==2{print int($4/1048576)}')
  if [ "${FREE:-99}" -lt 6 ]; then echo "[$(date +%H:%M)] LOW DISK (${FREE}GB free) — halting prep to be safe" >> "$LOG"; exit 0; fi
  mkdir -p "$DIR"
  echo "[$(date +%H:%M)] downloading $TAG <- $URL" >> "$LOG"
  # streams / hunt / long => 360p (transition hunting); dedicated dungeon vids => 480p (legible chat)
  local RES=480; case "$TAG" in *tream*|*Hunt*|*General*) RES=360;; esac
  timeout 600 yt-dlp -f "bestvideo[height<=$RES]+bestaudio/best[height<=$RES]/best" -o "videos/$TAG.%(ext)s" "$URL" >>"$LOG" 2>&1 </dev/null || { echo "[fail dl] $TAG" >>"$LOG"; return; }
  local V; V=$(ls -t videos/"$TAG".* 2>/dev/null | grep -viE '\.part$' | head -1)
  [ -z "$V" ] && { echo "[no file] $TAG" >>"$LOG"; return; }
  local DUR; DUR=$(ffprobe -v error -show_entries format=duration -of csv=p=0 "$V" 2>/dev/null | cut -d. -f1)
  local FPS=1; [ "${DUR:-0}" -gt 1200 ] && FPS="1/3"; [ "${DUR:-0}" -gt 3600 ] && FPS="1/6"
  echo "[$(date +%H:%M)] $TAG dur=${DUR}s fps=$FPS — extracting" >> "$LOG"
  ffmpeg -nostdin -hide_banner -y -i "$V" -vf "fps=$FPS" "$DIR/f_%05d.png" 2>/dev/null
  ffmpeg -nostdin -hide_banner -y -i "$V" -vf "fps=1/4,crop=in_w*0.35:in_h*0.22:0:in_h*0.78" "$DIR/chat_%05d.png" 2>/dev/null
  ffmpeg -nostdin -hide_banner -y -i "$V" -vf "fps=1/8,crop=in_w*0.15:in_h*0.26:in_w*0.85:0" "$DIR/minimap_%05d.png" 2>/dev/null
  # transition detection (dungeon entries via loading screens)
  bash scripts/video/detect_transitions.sh "$V" "$TAG" >>"$LOG" 2>&1 || true
  # contact sheet (≤16 evenly spaced overview frames)
  local FRAMES; FRAMES=$(ls "$DIR"/f_*.png 2>/dev/null | wc -l)
  if [ "$FRAMES" -gt 0 ]; then
    local STEP=$(( FRAMES/16 + 1 ))
    montage $(ls "$DIR"/f_*.png | awk -v s=$STEP 'NR%s==1' | head -16) -tile 4x4 -geometry 360x203+2+2 -background black "$DIR/_contact.png" 2>/dev/null || true
  fi
  rm -f "$V"   # free disk; frames are what we need
  touch "$DIR/.prepped"
  echo "[$(date +%H:%M)] DONE $TAG: $FRAMES overview frames + chat/minimap + transitions" >> "$LOG"
}

# read the whole queue into an array FIRST (don't pipe — inner ffmpeg/yt-dlp read stdin and would
# eat the remaining queue lines, which previously stopped the loop after the first video).
mapfile -t LINES < <(grep -E '^[^#]' "$Q")
for line in "${LINES[@]}"; do
  IFS='|' read -r TAG URL <<< "$line"
  TAG=$(echo "$TAG" | xargs); URL=$(echo "$URL" | xargs)
  [ -z "$TAG" ] && continue
  prep_one "$TAG" "$URL" </dev/null   # /dev/null stdin so nothing inside can consume input
done
echo "=== bulk_prep FINISHED $(date) ===" >> "$LOG"
