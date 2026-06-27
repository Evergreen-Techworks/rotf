#!/usr/bin/env python3
"""
RESEARCH -> DEVELOPMENT auto-bridge.

Scans the merged recovery specs (docs/DUNGEON_SPECS.json), normalizes boss names,
drops everything already built / already coded / base-game (the collision PRE-filter,
i.e. the safety gate's first layer), and appends fresh build/enrich tasks to
docs/dev_queue.json so the DEV cron never runs dry.

Two safety layers (per user choice "full auto, with safety gate"):
  1) PRE-FILTER here: exact + normalized + fuzzy name match against the 3370 base/OC
     object ids and 459 coded .Init names + a curated alias/exclude table -> a candidate
     that collides is never queued (it becomes an enrich, a skip-for-review, or is dropped).
  2) BUILD-TIME gate (carried in each task's "how"): the dev step must re-run the
     base-game collision grep + the static dup-key check and SKIP/flag on any conflict.

Idempotent: re-running never duplicates a task already in dev_queue.json (matched by id).
Caps how many tasks it appends per run (default 20) so the queue stays a sane size;
the full candidate list is always written to docs/dev_bridge_report.json.

Usage:  python3 scripts/build_dev_queue.py [--max N] [--dry-run]
"""
import json, re, sys, subprocess, difflib, argparse
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
SPECS = ROOT / "docs/DUNGEON_SPECS.json"
QUEUE = ROOT / "docs/dev_queue.json"
LEDGER = ROOT / "docs/build_ledger.json"
REPORT = ROOT / "docs/dev_bridge_report.json"

# --- curated heritage-rename / base-game / system aliases -------------------
# recovered-name(normalized) -> ("ENRICH", coded_name) | ("DROP", reason)
ALIASES = {
    "craig the worst nightmare": ("ENRICH", "Craig the Mad Intern"),
    "craig": ("ENRICH", "Craig the Mad Intern"),
    "death king": ("DROP", "renamed to base 'Dwarf King' (build-crash if duplicated)"),
    "oryx the mad god": ("DROP", "base game (Oryx the Mad God 1/2)"),
    "realm giant": ("DROP", "base realm-event enemy"),
}
# normalized-prefix drops: handled by other systems / are not standalone objects
DROP_PREFIXES = ("epic ",)        # Epic-Enemy modifier system (D1), not separate objects
DROP_SUBSTR  = ("megamad ", "encounter")  # oryx-fight variants / realm-event encounters: enrich-only, not new

FUZZY_SKIP = 0.88   # >= this similarity to a known object/Init name -> don't auto-build, flag for review

def norm(s: str) -> str:
    s = s or ""
    s = re.sub(r"\(.*?\)", " ", s)              # drop parentheticals
    s = re.split(r"[–—\-:]", s)[0]    # drop after dash/colon descriptors
    s = s.lower()
    s = re.sub(r"[^a-z0-9 ]", " ", s)
    s = re.sub(r"\s+", " ", s).strip()
    return s

def load_known():
    xml = subprocess.run(
        r"""grep -rhoE 'id="[^"]+"' """ + str(ROOT/"server") + r""" --include='*.xml' | sed -E 's/id="//;s/"$//'""",
        shell=True, capture_output=True, text=True).stdout.splitlines()
    inits = subprocess.run(
        r"""grep -rhoE '\.Init\("[^"]+"' """ + str(ROOT/"server/wServer/logic/db") + r"""/*.cs | sed -E 's/.*\.Init\("//;s/"$//'""",
        shell=True, capture_output=True, text=True).stdout.splitlines()
    known_raw = set(x.strip() for x in xml if x.strip()) | set(i.strip() for i in inits if i.strip())
    known_norm = {}                       # norm -> a representative original (prefer Init/coded name)
    for x in known_raw:
        known_norm.setdefault(norm(x), x)
    init_norm = set(norm(i) for i in inits)
    return known_norm, init_norm

def collect_bosses():
    specs = json.loads(SPECS.read_text())
    agg = {}   # norm -> {name, taunts:set, attacks:set, flashes:set, conf:[], dungeons:set}
    for spec in specs:
        conf = (spec.get("confidence") or "")
        dn = spec.get("dungeon") or ""
        for b in (spec.get("bosses") or []):
            if not isinstance(b, dict): continue
            nm = (b.get("name") or "").strip()
            if not nm or nm.startswith("(") or "none" in nm.lower() or "not rotf" in nm.lower():
                continue
            n = norm(nm)
            if not n: continue
            r = agg.setdefault(n, {"name": nm, "taunts": set(), "attacks": set(),
                                   "flashes": set(), "conf": [], "dungeons": set()})
            # prefer the shortest clean display name as canonical
            if len(nm) < len(r["name"]): r["name"] = nm
            for t in (b.get("taunts") or []):
                if isinstance(t, str) and t.strip() and not t.startswith("("):
                    r["taunts"].add(t.strip())
            for a in (b.get("attacks") or []):
                if isinstance(a, str) and a.strip(): r["attacks"].add(a.strip())
            cf = b.get("colorFlashes")
            if isinstance(cf, str) and cf.strip(): r["flashes"].add(cf.strip())
            if conf: r["conf"].append(conf)
            if dn: r["dungeons"].add(dn[:60])
    return agg

def slug(name):
    return re.sub(r"[^a-z0-9]+", "-", name.lower()).strip("-")

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--max", type=int, default=20)
    ap.add_argument("--dry-run", action="store_true")
    args = ap.parse_args()

    known_norm, init_norm = load_known()
    known_keys = list(known_norm.keys())
    agg = collect_bosses()
    queue = json.loads(QUEUE.read_text())
    existing_ids = {t["id"] for t in queue["tasks"]}
    # bosses already referenced by an existing (hand-written) task -> don't re-queue an enrich for them
    existing_blob = " ".join((t.get("name","")+" "+t.get("how","")) for t in queue["tasks"]).lower()

    enrich, builds, skips, drops = [], [], [], []

    for n, r in agg.items():
        nm = r["name"]
        taunts = sorted(r["taunts"]); attacks = sorted(r["attacks"])
        conf = " ".join(r["conf"]).upper()
        # ---- exclusions ----
        if any(n.startswith(p) for p in DROP_PREFIXES) or any(s in n for s in DROP_SUBSTR):
            drops.append((nm, "system/variant prefix")); continue
        if n in ALIASES:
            kind, target = ALIASES[n]
            if kind == "DROP": drops.append((nm, target)); continue
            if kind == "ENRICH" and taunts:
                enrich.append((slug("enrich-"+target), target, nm, taunts, attacks, "alias"));
                continue
            drops.append((nm, "alias->"+target+" (no new taunts)")); continue
        # ---- already an object / coded behavior -> ENRICH only (low risk) ----
        if n in known_norm or n in init_norm:
            coded = known_norm.get(n, nm)
            if taunts or attacks:
                enrich.append((slug("enrich-"+coded), coded, nm, taunts, attacks, "exact"))
            else:
                drops.append((nm, "already coded, no recovered taunts/attacks"))
            continue
        # ---- fuzzy near-match to something coded -> SKIP for human review ----
        close = difflib.get_close_matches(n, known_keys, n=1, cutoff=FUZZY_SKIP)
        if close:
            skips.append((nm, known_norm[close[0]],
                          difflib.SequenceMatcher(None, n, close[0]).ratio()))
            continue
        # ---- genuinely NEW -> build candidate (content + confidence gate) ----
        if "LOW" in conf and not taunts:
            drops.append((nm, "new but LOW confidence + no taunts")); continue
        if not taunts and not attacks:
            drops.append((nm, "new but no recovered content")); continue
        builds.append((slug("boss-"+nm), nm, taunts, attacks, sorted(r["flashes"]),
                       conf, sorted(r["dungeons"])))

    # ---- emit tasks: enrich (low risk) first, then new builds, by content richness ----
    enrich = {e[0]: e for e in enrich}.values()             # dedup by id
    enrich = sorted(enrich, key=lambda e: -(len(e[3])+len(e[4])))
    builds = {b[0]: b for b in builds}.values()
    builds = sorted(builds, key=lambda b: -(len(b[2])*2+len(b[3])))

    # reserve ~40% of the per-run cap for NEW builds so actual new content isn't starved by enrichments
    build_slots = min(len(list(builds)), max(1, round(args.max * 0.4))) if builds else 0
    enrich_cap = args.max - build_slots
    new_tasks, appended, enrich_added = [], 0, 0
    for eid, coded, recname, taunts, attacks, how in enrich:
        if eid in existing_ids or appended >= args.max or enrich_added >= enrich_cap: continue
        if coded.lower() in existing_blob:        # boss already covered by a hand-written task
            continue
        tl = ", ".join('new Taunt(0.0016, "%s")' % t.replace('"', r'\"') for t in taunts)
        new_tasks.append({
            "id": eid, "type": "fix", "status": "pending", "risk": "low",
            "name": f"Enrich {coded} with recovered lines (auto-bridge)",
            "how": (f"ENRICH the already-coded '{coded}' behavior (recovered as '{recname}'). "
                    f"grep the behavior file for its fight State, and ADD ONLY the recovered "
                    f"taunts not already present (verbatim): {tl}. "
                    f"{'Recovered attacks to reflect in Shoot/phases if not already: '+'; '.join(attacks)+'. ' if attacks else ''}"
                    f"If every taunt is already present, mark done as a no-op. Server-only."),
            "result": "", "_source": "auto-bridge"})
        existing_ids.add(eid); appended += 1; enrich_added += 1

    for bid, nm, taunts, attacks, flashes, conf, dungeons in builds:
        if bid in existing_ids or appended >= args.max: continue
        tl = ", ".join('new Taunt(0.0016, "%s")' % t.replace('"', r'\"') for t in taunts) or "(no recovered taunts)"
        new_tasks.append({
            "id": bid, "type": "boss", "status": "pending", "risk": "med",
            "name": f"Build recovered boss '{nm}' (auto-bridge)",
            "how": (f"COLLISION-CHECK '{nm}' in base xml + behaviors FIRST "
                    f"(grep server/**/*.xml for id=\"{nm}\" and logic/db/*.cs for .Init(\"{nm}\")). "
                    f"If it EXISTS, ENRICH that behavior instead and do NOT add a new object (duplicate id = boot crash). "
                    f"If clean: add object at the NEXT FREE enemy id (0x8f0f+ or 0x8fd7-0x8fdf) to "
                    f"OrdinaryClient_Bosses.xml (placeholder chars16x16dEncounters2 sprite, a reused projectile, "
                    f"~90-120k HP, Class=Character + Enemy + God + StasisImmune + Quest) + .Init(\"{nm}\", ...) in "
                    f"BehaviorDb.OCBosses2.cs with taunts: {tl}. "
                    f"{'Recovered attacks (build Shoot patterns from these): '+'; '.join(attacks)+'. ' if attacks else 'No attack pattern recovered -> use a generic radial+aimed mix. '}"
                    f"{'colorFlashes: '+', '.join(flashes)+'. ' if flashes else ''}"
                    f"Add Tuple.Create(\"{nm}\",0.03) to the Oryx Mountains RegionMobs pool + resync client "
                    f"EmbeddedData_OC_BossesCXML.dat. THEN run the static dup-key check "
                    f"(grep -rhoE '\\.Init\\(\"[^\"]+\"' logic/db/*.cs | sort | uniq -d  MUST be empty). "
                    f"Source dungeons: {'; '.join(dungeons[:3])}."),
            "result": "", "_source": "auto-bridge", "_confidence": conf[:40]})
        existing_ids.add(bid); appended += 1

    report = {
        "generated_from": "docs/DUNGEON_SPECS.json",
        "summary": {
            "recovered_boss_names": len(agg),
            "enrich_candidates": len(list(enrich)) if not isinstance(enrich, list) else len(enrich),
            "new_build_candidates": len(list(builds)) if not isinstance(builds, list) else len(builds),
            "skipped_for_review_fuzzy": len(skips),
            "dropped": len(drops),
            "appended_to_queue": len(new_tasks),
        },
        "skipped_for_review": [{"recovered": s[0], "looks_like": s[1], "similarity": round(s[2], 2)} for s in skips],
        "dropped": [{"name": d[0], "reason": d[1]} for d in drops],
        "appended": [{"id": t["id"], "type": t["type"], "risk": t["risk"], "name": t["name"]} for t in new_tasks],
    }
    REPORT.write_text(json.dumps(report, indent=2))

    if args.dry_run:
        print(json.dumps(report["summary"], indent=2))
        print("\n-- would append %d tasks (dry-run, queue NOT modified) --" % len(new_tasks))
        for t in new_tasks: print(f"   [{t['risk']:4}] {t['id']}")
        print("\nfull report -> docs/dev_bridge_report.json")
        return

    queue["tasks"].extend(new_tasks)
    QUEUE.write_text(json.dumps(queue, indent=2))
    print("appended %d tasks to docs/dev_queue.json (now %d pending)" % (
        len(new_tasks), sum(1 for t in queue["tasks"] if t["status"] != "done")))
    print("report -> docs/dev_bridge_report.json")
    for t in new_tasks: print(f"   + [{t['risk']:4}] {t['id']}")

if __name__ == "__main__":
    main()
