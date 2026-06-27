#!/usr/bin/env python3
"""next_buildable.py — the readiness gate for the build-when-ready loop.

Prints the FIRST backlog feature that is BUILD-READY and NOT yet built, or 'NONE'.
Readiness gate: type in {boss, dungeon, fix} AND readiness == 'ready' AND built == false.
Systems flagged 'needs_live_client' are NEVER auto-built (they need a human client test).
Items flagged 'partial' need more recovery first.

Usage:
  python3 scripts/next_buildable.py            # print next ready feature (name + info)
  python3 scripts/next_buildable.py --json      # machine-readable
  python3 scripts/next_buildable.py --status     # full ledger summary
"""
import json, os, sys

LEDGER = os.path.join(os.path.dirname(__file__), "..", "docs", "build_ledger.json")

def load():
    return json.load(open(os.path.normpath(LEDGER), encoding="utf-8"))

def next_ready(data):
    for f in data.get("backlog", []):
        if f.get("built"): continue
        if f.get("type") in ("boss", "dungeon", "fix") and f.get("readiness") == "ready":
            return f
    return None

def main():
    data = load()
    if "--status" in sys.argv:
        bl = data["backlog"]
        ready = [f for f in bl if f["type"] in ("boss","dungeon","fix") and f["readiness"]=="ready" and not f.get("built")]
        partial = [f for f in bl if f["readiness"]=="partial" and not f.get("built")]
        nlc = [f for f in bl if f["readiness"]=="needs_live_client"]
        print(f"BUILT: {len(data['built'])} features")
        print(f"BACKLOG ready-to-build (auto): {len(ready)}")
        for f in ready: print(f"   - {f['name']}")
        print(f"BACKLOG partial (needs more recovery): {len(partial)}")
        for f in partial: print(f"   - {f['name']}")
        print(f"BACKLOG needs_live_client (manual test gate): {len(nlc)}")
        for f in nlc: print(f"   - {f['name']}")
        return
    f = next_ready(data)
    if not f:
        print("NONE")
        return
    if "--json" in sys.argv:
        print(json.dumps(f))
    else:
        print(f"BUILD-READY: {f['name']}\n  type: {f['type']}\n  info: {f.get('info','')}")

if __name__ == "__main__":
    main()
