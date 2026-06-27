#!/usr/bin/env bash
# Linux/WSL build of the Ordinary Client (ROTF) -> bin-debug/WebMain.swf.
# Mirrors build-client.bat exactly; reuses the already-downloaded .buildsdk/.
# Requires: java on PATH (have OpenJDK 17). Usage: ./build-client.sh
set -euo pipefail
ROOT="$(cd "$(dirname "$0")" && pwd)"
SDK="$ROOT/.buildsdk"
[ -f "$SDK/lib/mxmlc.jar" ] || { echo "[ERROR] Flex SDK not found in $SDK"; exit 1; }
export PLAYERGLOBAL_HOME="$SDK/frameworks/libs/player"
export FLEX_HOME="$SDK"
# WSL has no X server; DISPLAY points at a dead address. Force headless Java so the
# compiler's Java2D font/asset rasterization doesn't try to open an X11 connection.
export _JAVA_OPTIONS="-Djava.awt.headless=true"
unset DISPLAY
chmod +x "$SDK/bin/mxmlc" 2>/dev/null || true
mkdir -p "$ROOT/bin-debug"

echo "=== Ordinary Client / ROTF : building WebMain.swf (WSL) ==="
"$SDK/bin/mxmlc" "$ROOT/src/WebMain.as" \
  -source-path+="$ROOT/src" \
  -library-path+="$ROOT/libs" \
  -locale=en_US \
  -default-size 800 600 \
  -default-frame-rate=60 \
  -default-background-color=0x000000 \
  -swf-version=15 \
  -target-player=11.2 \
  -optimize=true \
  -use-direct-blit=true \
  -keep-as3-metadata+=Inject \
  -keep-as3-metadata+=Embed \
  -keep-as3-metadata+=PostConstruct \
  -keep-as3-metadata+=ArrayElementType \
  -static-link-runtime-shared-libraries=true \
  -output="$ROOT/bin-debug/WebMain.swf"

echo "[OK] Built: $ROOT/bin-debug/WebMain.swf"
ls -la "$ROOT/bin-debug/WebMain.swf"
