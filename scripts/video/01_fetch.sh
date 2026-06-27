#!/usr/bin/env bash
# Download a ROTF gameplay video for analysis. Needs: pip install yt-dlp
# Usage: ./01_fetch.sh <youtube_url> [out_dir]
set -euo pipefail
URL="$1"; OUT="${2:-videos}"; mkdir -p "$OUT"
yt-dlp -f 'bestvideo[height>=720]+bestaudio/best/best' -o "$OUT/%(id)s.%(ext)s" "$URL"
echo "saved to $OUT/"
