#!/usr/bin/env python3
"""Generate default AI behaviors for the imported ROTF enemies (the real client data
carries stats but NO behaviors). Reads docs/rotf-wiki/parsed/imported_enemies.json and
emits server/wServer/logic/db/BehaviorDb.ROTFImported.cs as ONE `_` field (auto-registered).

Rules: proj==0 -> skip (stationary destructibles/droppers; hand-tune later).
       proj>0  -> Prioritize(Wander) + Shoot(s); bosses (high HP) get extra spread + tier loot.
"""
import json, os, re

ROOT = os.path.normpath(os.path.join(os.path.dirname(__file__), ".."))
roster = json.load(open(os.path.join(ROOT, "docs/rotf-wiki/parsed/imported_enemies.json")))

# names already handled by an EXISTING behavior (vanilla or hand-made) — skip to avoid
# duplicate-key crashes (e.g. vanilla OceanTrench already defines "Sea Slurp Home").
import glob as _glob
existing = set()
for bf in _glob.glob(os.path.join(ROOT, "server/wServer/logic/db/BehaviorDb*.cs")):
    if bf.endswith("ROTFImported.cs"):
        continue
    for m in re.findall(r'\.Init\(\s*"((?:[^"\\]|\\.)*)"', open(bf, encoding="utf-8").read()):
        existing.add(m)

def esc(s): return s.replace("\\", "\\\\").replace('"', '\\"')

inits = []
made = 0
skipped_existing = 0
for e in roster:
    name, hp, proj = e["name"], e.get("hp", 0), e.get("proj", 0)
    if proj <= 0:
        continue  # walls / droppers / static — leave stationary
    if name in existing:
        skipped_existing += 1
        continue  # an existing behavior already owns this name
    boss = hp >= 100000
    mid = hp >= 8000
    speed = 0.45 if not boss else 0.5
    shoots = []
    if boss:
        shoots.append('new Shoot(9, count: 5, shootAngle: 20, coolDown: 700)')
        if proj >= 2:
            shoots.append('new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)')
        else:
            shoots.append('new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)')
    elif mid:
        shoots.append('new Shoot(8, count: 3, shootAngle: 15, coolDown: 900)')
        if proj >= 2:
            shoots.append('new Shoot(8, projectileIndex: 1, coolDown: 1600)')
    else:
        shoots.append('new Shoot(8, coolDown: 1100)')

    body = 'new Prioritize(new Wander(%s))' % speed
    state_inner = ",\n                    ".join([body] + shoots)
    loot = ""
    if boss:
        loot = (',\n                new Threshold(0.01,\n'
                '                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),\n'
                '                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),\n'
                '                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)\n'
                '                    )')
    elif mid:
        loot = (',\n                new Threshold(0.05,\n'
                '                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)\n'
                '                    )')
    inits.append(
        '            .Init("%s",\n'
        '                new State(\n'
        '                    %s\n'
        '                    )%s\n'
        '            )' % (esc(name), state_inner, loot))
    made += 1

src = (
    "using common.resources;\n"
    "using wServer.logic.behaviors;\n"
    "using wServer.logic.loot;\n"
    "using wServer.logic.transitions;\n\n"
    "namespace wServer.logic\n{\n"
    "    // AUTO-GENERATED default behaviors for the imported real-ROTF enemies\n"
    "    // (scripts/gen_imported_behaviors.py). Stats are real; AI is a sensible default\n"
    "    // (wander + shoot, bosses get spread + tier loot). Hand-tune marquee bosses separately.\n"
    "    partial class BehaviorDb\n    {\n"
    "        private _ ROTFImported = () => Behav()\n"
    + "\n".join(inits) + ";\n"
    "    }\n}\n"
)
out = os.path.join(ROOT, "server/wServer/logic/db/BehaviorDb.ROTFImported.cs")
open(out, "w", encoding="utf-8").write(src)
print(f"wrote {out}: {made} behaviors (of {len(roster)} enemies; "
      f"{skipped_existing} skipped as already-handled by existing behaviors)")
