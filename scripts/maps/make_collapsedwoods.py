#!/usr/bin/env python3
"""Generate CollapsedWoods.jm — ROTF Collapsed Woods (Medium forest dungeon).
A grassy fallen-timber grove: south spawn + Nexus-return -> winding woods -> central clearing where
the dormant Treesmasher waits. Critters are summoned by the boss; a few Angry Saplings seeded.
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 48, 48
void = "#" * W

def blank():
    rows = [void, void]
    base = "#" + "g" * (W - 2) + "#"
    for _ in range(H - 4):
        rows.append(base)
    rows += [void, void]
    return rows

rows = blank()
def setcell(y, x, ch):
    row = list(rows[y]); row[x] = ch; rows[y] = "".join(row)

cx = W // 2

# central clearing (Dark Grass) where the dormant Treesmasher lies
for y in range(12, 24):
    for x in range(cx - 9, cx + 10):
        setcell(y, x, "k")
setcell(18, cx, "T")          # Treesmasher (dormant)

# shadowy undergrowth accents
for y in range(4, H - 4):
    for x in range(2, W - 2):
        if (x * 7 + y * 5) % 19 == 0 and rows[y][x] == "g":
            setcell(y, x, "f")

# seeded Angry Saplings flanking the approach
for (py, px) in [(28, 12), (28, W - 13), (33, 18), (33, W - 19)]:
    setcell(py, px, "P")

for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    "g": {"ground": "Bright Grass"},
    "k": {"ground": "Dark Grass"},
    "f": {"ground": "Fall Grass Shadow"},
    "S": {"ground": "Bright Grass", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Bright Grass", "objs": [{"id": "Nexus Portal"}]},
    "T": {"ground": "Dark Grass", "objs": [{"id": "Treesmasher"}]},
    "P": {"ground": "Bright Grass", "objs": [{"id": "Angry Sapling"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "CollapsedWoods.jm"))
write_jm(out, rows, legend)
