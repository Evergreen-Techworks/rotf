#!/usr/bin/env bash
# Start the full ROTF backend locally: redis + app server (:8080) + world server (:2050).
# Run from WSL:  ./run-local.sh     (Ctrl+C stops everything)
set -e
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_ROOT="$HOME/.dotnet"
ROOT="$(cd "$(dirname "$0")" && pwd)"

echo "[0/3] clearing any previous instance / freeing ports 8080 2050 843..."
pkill -f "dotnet.*server.dll"  2>/dev/null || true
pkill -f "dotnet.*wServer.dll" 2>/dev/null || true
for p in 8080 2050 843; do fuser -k -n tcp $p 2>/dev/null || true; done

echo "[1/3] redis..."
redis-cli ping >/dev/null 2>&1 || redis-server --daemonize yes --save "" --appendonly no
redis-cli ping >/dev/null 2>&1 && echo "      redis up" || { echo "      redis FAILED"; exit 1; }

echo "[2/3] building server (.NET 8)..."
( cd "$ROOT/server" && dotnet build realm-src.sln -c Debug 2>&1 | grep -iE "Build succeeded|Build FAILED|error CS" | head )

APP="$ROOT/server/server/bin/Debug/net8.0"
WLD="$ROOT/server/wServer/bin/Debug/net8.0"

echo "[3/3] starting servers — app :8080, world :2050  (Ctrl+C to stop)"
( cd "$APP" && exec dotnet server.dll ) &  APP_PID=$!
( cd "$WLD" && exec dotnet wServer.dll ) & WLD_PID=$!
trap 'echo; echo "stopping..."; kill $APP_PID $WLD_PID 2>/dev/null' EXIT INT TERM
echo "      app=$APP_PID  world=$WLD_PID"
echo "      now open client/bin-debug/WebMain.swf in the standalone Flash Projector (see docs/HOW_TO_TEST.md)"
wait
