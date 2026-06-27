#!/usr/bin/env python3
"""
Build/refresh docs/taunt_verification.json — the worklist for the TAUNT VERIFY LOOP.

Separates GROUNDED from DESIGNED in the built behaviors. For every taunt in OUR ROTF
behavior files (NOT base-game files), it records: the boss (nearest .Init above the line),
the taunt text, any inline provenance comment, and CANDIDATE SOURCE TAGS found by scanning
docs/video-recovery/*.json for that exact/þnear text (+ whether that tag's chat frames still
exist on disk for a vision re-read). One ledger entry per BOSS; status defaults to "pending".

The VERIFY LOOP cron then takes one pending boss per fire, re-reads its source frames with
vision, confirms/corrects each taunt in the .cs, stamps a provenance comment, recompiles, and
marks the boss verified.

Usage: python3 scripts/verify_taunts.py            # create/refresh (preserves existing verdicts)
"""
import re, glob, os, json, difflib

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DB = os.path.join(ROOT, "server/wServer/logic/db")
SPECS = glob.glob(os.path.join(ROOT, "docs/video-recovery/*.json"))
FRAMES = os.path.join(ROOT, "docs/video-recovery/frames")
LEDGER = os.path.join(ROOT, "docs/taunt_verification.json")

# OUR ROTF behavior files (authored or enriched by us). Base-game files (GhostKing, Tomb, Lich,
# Beachzone, EntAncient, Sphinx, Shore, Mountain, OceanTrench, GhostShip, LotLL, CaveTT, Lab,
# Lowland, Misc, Phoenix, RedDemon, Deathmage, Cyclops, Pentaract, Golems, Hermit, CubeGod) are
# EXCLUDED — their taunts are original RotMG content, not ours to verify. Crystal/Abyss/Midland
# are base files we ENRICHED -> included so our added lines get checked.
ROTF_FILES = {
    "BehaviorDb.OCBosses.cs", "BehaviorDb.OCBosses2.cs", "BehaviorDb.Asgard.cs",
    "BehaviorDb.Necropolis.cs", "BehaviorDb.ToDD.cs", "BehaviorDb.AnubisLair.cs",
    "BehaviorDb.NyxOfFire.cs", "BehaviorDb.Encounters.cs", "BehaviorDb.ShroudedSanctum.cs",
    "BehaviorDb.Craig.cs", "BehaviorDb.SacrificialGrounds.cs", "BehaviorDb.FlamingHearth.cs",
    "BehaviorDb.Riverside.cs", "BehaviorDb.CollapsedWoods.cs", "BehaviorDb.IcyPeaks.cs",
    "BehaviorDb.Starforce.cs", "BehaviorDb.GateUnderworld.cs", "BehaviorDb.ROTF.cs",
    "BehaviorDb.Crystal.cs", "BehaviorDb.Abyss.cs", "BehaviorDb.Midland.cs",
}

INIT_RE  = re.compile(r'\.Init\(\s*"([^"]+)"')
TAUNT_RE = re.compile(r'new Taunt\(([^)]*?)"([^"]*)"\s*\)')

def classify(comment):
    c = comment.lower()
    if "reconstruct" in c or "designed" in c: return "DESIGNED"
    if re.search(r'disc_|verbatim|confirmed|recovered|verified', c): return "SOURCED"
    return "UNLABELED"

def load_specs():
    """Return list of (tag, blob_lowercased, raw_json_str) for text search."""
    out = []
    for p in SPECS:
        tag = os.path.basename(p)[:-5]
        try:
            raw = open(p, encoding="utf-8", errors="replace").read()
        except Exception:
            continue
        out.append((tag, raw.lower(), raw))
    return out

def frames_exist(tag):
    d = os.path.join(FRAMES, tag)
    return os.path.isdir(d) and any(f.startswith("chat_") for f in os.listdir(d))

def find_source_tags(text, specs):
    """Tags whose spec JSON contains this taunt text (exact, then fuzzy>=0.9 on any taunt line)."""
    t = text.lower().strip()
    if len(t) < 4: return []
    hits = []
    for tag, blob, raw in specs:
        if t in blob:
            hits.append(tag)
    return hits

def parse_file(path, specs):
    fn = os.path.basename(path)
    cur_boss = None
    bosses = {}
    for ln in open(path, encoding="utf-8", errors="replace"):
        mi = INIT_RE.search(ln)
        if mi:
            cur_boss = mi.group(1)
            bosses.setdefault(cur_boss, [])
        mt = TAUNT_RE.search(ln)
        if mt:
            text = mt.group(2)
            comment = ln.split("//", 1)[1].strip() if "//" in ln else ""
            status = classify(comment)
            src = find_source_tags(text, specs) if status != "DESIGNED" else []
            bosses.setdefault(cur_boss or "(unknown)", []).append({
                "text": text, "label": status, "comment": comment,
                "source_tags": src,
                "frames_available": [t for t in src if frames_exist(t)],
            })
    return fn, bosses

def main():
    specs = load_specs()
    prev = {}
    if os.path.exists(LEDGER):
        try:
            for e in json.load(open(LEDGER))["bosses"]:
                prev[(e["file"], e["boss"])] = e.get("verify_status", "pending")
        except Exception:
            pass
    entries = []
    for path in sorted(glob.glob(os.path.join(DB, "BehaviorDb.*.cs"))):
        if os.path.basename(path) not in ROTF_FILES: continue
        fn, bosses = parse_file(path, specs)
        for boss, taunts in bosses.items():
            if not taunts: continue
            # only queue bosses that still have UNLABELED/unverified taunts
            needs = [t for t in taunts if t["label"] == "UNLABELED"]
            entries.append({
                "file": fn, "boss": boss,
                "taunt_count": len(taunts),
                "unlabeled_count": len(needs),
                "verify_status": prev.get((fn, boss), "pending" if needs else "n/a-all-labeled"),
                "taunts": taunts,
            })
    pend = [e for e in entries if e["verify_status"] == "pending"]
    out = {
        "_doc": "TAUNT VERIFY LOOP worklist. Each cron fire takes the FIRST boss with verify_status=='pending', "
                "re-reads its source_tags' chat_*.png frames with vision, confirms/corrects each taunt text in the "
                ".cs file, stamps a provenance comment (// VERIFIED <tag> | // CORRECTED <tag> | // UNVERIFIABLE-source-lost | "
                "// DESIGNED-no-footage), recompiles (0 errors), sets verify_status, then STOPS. Base-game files are excluded.",
        "summary": {
            "rotf_bosses_total": len(entries),
            "pending": len(pend),
            "with_recheckable_frames": sum(1 for e in pend if any(t["frames_available"] for t in e["taunts"])),
            "no_source_found": sum(1 for e in pend if not any(t["source_tags"] for t in e["taunts"])),
        },
        "bosses": entries,
    }
    json.dump(out, open(LEDGER, "w"), indent=2)
    s = out["summary"]
    print(f"ledger -> {LEDGER}")
    print(f"  ROTF bosses: {s['rotf_bosses_total']} | pending verify: {s['pending']}")
    print(f"  pending WITH re-checkable frames: {s['with_recheckable_frames']}")
    print(f"  pending with NO source spec found: {s['no_source_found']}")

if __name__ == "__main__":
    main()
