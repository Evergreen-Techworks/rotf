#!/usr/bin/env python3
"""
Generate docs/mechanics_recovery.json — a per-built-boss tracker for the
MECHANICS RECOVERY LOOP (attacks / maps / loot / stats), the non-taunt layers.

It is the attack/map analogue of docs/taunt_verification.json:
- enumerates BUILT bosses (from docs/taunt_verification.json, which already
  tracks every .Init'd ROTF boss + its source_tags),
- checks which source tags actually have bossends/ (attack-recoverable) and
  minimap_ frames (map-recoverable) on disk,
- best-effort parses each boss's .Init block to see if its attack comments
  are already DOCUMENTED-from-frames vs RECONSTRUCTED,
- PRESERVES prior verdicts (mechanics_status / map_status / loot_status /
  stats_status / note) so the loop's progress is not clobbered on regen.

Sprites are intentionally NOT tracked here: they cannot be recovered from
compressed video frames (needs original assets / hand-art).
"""
import json, os, re, glob

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
FRAMES = os.path.join(ROOT, "docs/video-recovery/frames")
DB = os.path.join(ROOT, "server/wServer/logic/db")
TAUNT_LEDGER = os.path.join(ROOT, "docs/taunt_verification.json")
OUT = os.path.join(ROOT, "docs/mechanics_recovery.json")


def tag_has_bossends(tag):
    d = os.path.join(FRAMES, tag, "bossends")
    return os.path.isdir(d) and any(f.endswith(".png") for f in os.listdir(d))


def tag_has_minimap(tag):
    d = os.path.join(FRAMES, tag)
    return os.path.isdir(d) and any(f.startswith("minimap_") for f in os.listdir(d))


def read_init_block(file, boss):
    """Return the text of .Init("boss", ...) up to the next .Init( or EOF."""
    path = os.path.join(DB, file)
    if not os.path.isfile(path):
        return ""
    txt = open(path, encoding="utf-8", errors="replace").read()
    m = re.search(r'\.Init\("' + re.escape(boss) + r'"', txt)
    if not m:
        return ""
    start = m.start()
    nxt = txt.find('.Init("', m.end())
    return txt[start: nxt if nxt != -1 else len(txt)]


def attack_status_from_cs(block, has_bossends):
    """documented | partial | reconstructed | no-footage | unknown"""
    if not block:
        return "unknown"
    has_doc = "DOCUMENTED-from-frames" in block or "DOCUMENTED-FROM-FRAMES" in block.upper()
    has_recon = "RECONSTRUCTED" in block.upper()
    if has_doc and not has_recon:
        return "documented"
    if has_doc and has_recon:
        return "partial"
    if has_recon and not has_bossends:
        return "no-footage"      # reconstructed AND no frames to ever fix it
    if has_recon:
        return "reconstructed"   # reconstructed BUT bossends exist -> ADDRESSABLE
    return "unknown"


def main():
    tl = json.load(open(TAUNT_LEDGER))
    # preserve prior mechanics verdicts
    prior = {}
    if os.path.isfile(OUT):
        try:
            for b in json.load(open(OUT)).get("bosses", []):
                prior[b["boss"]] = b
        except Exception:
            pass

    out_bosses = []
    for b in tl["bosses"]:
        boss = b["boss"]
        file = b.get("file", "")
        tags = sorted({s for t in b.get("taunts", []) for s in (t.get("source_tags") or [])})
        be_tags = [t for t in tags if tag_has_bossends(t)]
        mm_tags = [t for t in tags if tag_has_minimap(t)]
        block = read_init_block(file, boss)
        atk = attack_status_from_cs(block, bool(be_tags))

        p = prior.get(boss, {})
        rec = {
            "boss": boss,
            "file": file,
            "source_tags": tags,
            "bossends_tags": be_tags,          # attack-recoverable from these
            "minimap_tags": mm_tags,           # map-recoverable from these
            # statuses the loop maintains:
            "attacks_status": p.get("attacks_status") or atk,
            "map_status": p.get("map_status") or ("undocumented" if mm_tags else "no-footage"),
            "loot_status": p.get("loot_status") or "designed",
            "stats_status": p.get("stats_status") or "designed-placeholder",
            "sprite_status": "placeholder-unrecoverable-from-video",
            "note": p.get("note", ""),
        }
        out_bosses.append(rec)

    # summary
    def count(field, val):
        return sum(1 for x in out_bosses if x[field] == val)

    addressable_atk = [x for x in out_bosses if x["attacks_status"] not in ("documented", "no-footage") and x["bossends_tags"]]
    addressable_map = [x for x in out_bosses if x["map_status"] == "undocumented" and x["minimap_tags"]]
    doc = {
        "_doc": "MECHANICS RECOVERY LOOP tracker. Upgrades non-taunt layers (attacks/maps/loot/stats) from already-prepped frames. Sprites NOT recoverable from video. Regenerate with scripts/mechanics_recovery_ledger.py (preserves verdicts).",
        "summary": {
            "bosses": len(out_bosses),
            "attacks_documented": count("attacks_status", "documented"),
            "attacks_partial": count("attacks_status", "partial"),
            "attacks_reconstructed": count("attacks_status", "reconstructed"),
            "attacks_no_footage": count("attacks_status", "no-footage"),
            "attacks_ADDRESSABLE_now": len(addressable_atk),
            "map_documented": count("map_status", "documented"),
            "map_undocumented": count("map_status", "undocumented"),
            "map_no_footage": count("map_status", "no-footage"),
            "map_ADDRESSABLE_now": len(addressable_map),
        },
        "bosses": out_bosses,
    }
    json.dump(doc, open(OUT, "w"), indent=1)
    print("mechanics ledger ->", OUT)
    for k, v in doc["summary"].items():
        print(f"  {k}: {v}")


if __name__ == "__main__":
    main()
