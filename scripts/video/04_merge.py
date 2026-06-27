#!/usr/bin/env python3
"""Merge all per-video recovery JSONs (docs/video-recovery/*.json) into the master
docs/DUNGEON_SPECS.json, enriching each dungeon: union drops + taunts, append attack/phase
notes, prefer the most detailed map description. Safe to re-run as more videos are analyzed.
"""
import json, glob, os
ROOT = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", ".."))
SPECS = os.path.join(ROOT, "docs/DUNGEON_SPECS.json")
specs = {s["dungeon"]: s for s in json.load(open(SPECS))} if os.path.exists(SPECS) else {}

for f in glob.glob(os.path.join(ROOT, "docs/video-recovery/*.json")):
    v = json.load(open(f))
    dun = v.get("dungeon") or os.path.basename(f)[:-5]
    s = specs.setdefault(dun, {"dungeon": dun, "bosses": [], "minions": [], "drops": [], "mapLayout": ""})
    # map: keep the longer/more detailed description
    if len(v.get("map", "")) > len(s.get("mapLayout", "")):
        s["mapLayout"] = v["map"]
    s["drops"] = sorted(set(s.get("drops", [])) | set(v.get("drops", [])))
    by = {b["name"]: b for b in s.get("bosses", [])}
    for vb in v.get("bosses", []):
        b = by.setdefault(vb["name"], {"name": vb["name"], "fightPattern": "", "taunts": []})
        b["taunts"] = sorted(set(b.get("taunts", [])) | set(vb.get("taunts", [])))
        notes = "; ".join(vb.get("phases", []) + vb.get("attacks", []))
        if notes:
            b["fightPattern"] = (b.get("fightPattern", "") + " | [VIDEO] " + notes).strip(" |")
        if vb.get("minions"):
            b["minions"] = sorted(set(b.get("minions", [])) | set(vb["minions"]))
    s["bosses"] = list(by.values())
    s.setdefault("sources", []).append(f"video-recovery/{os.path.basename(f)}")

json.dump(list(specs.values()), open(SPECS, "w"), indent=1)
print(f"merged {len(glob.glob(os.path.join(ROOT,'docs/video-recovery/*.json')))} video specs into {SPECS} ({len(specs)} dungeons)")
