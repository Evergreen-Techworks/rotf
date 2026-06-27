#!/usr/bin/env python3
"""Generate FlamingHearth.jm — ROTF Flaming Hearth (Medium-rank fire dungeon / realm-Lord den).
Scorching plains: Scorch ground laced with Red-Quad-Lava fiery grass and scalding LAVA LAKES
(walkable damage tiles), descending to Firebreather's central den. South spawn + Nexus-return.
Salamanders/armadillos/tumbleweeds are summoned; a few Firespitting Flowers seeded as turrets.
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 48, 48
void = "#" * W

def blank():
    rows = [void, void]
    base = "#" + "s" * (W - 2) + "#"   # scorched ground by default
    for _ in range(H - 4):
        rows.append(base)
    rows += [void, void]
    return rows

rows = blank()
def setcell(y, x, ch):
    row = list(rows[y]); row[x] = ch; rows[y] = "".join(row)
def rect(y0, y1, x0, x1, ch):
    for y in range(y0, y1 + 1):
        for x in range(x0, x1 + 1):
            setcell(y, x, ch)

cx = W // 2

# Firebreather's den (Red-Quad-Lava fiery floor) in the north-center
rect(5, 16, 13, W - 14, "f")
setcell(10, cx, "F")          # Firebreather

# scalding lava lakes (walkable damage tiles) scattered across the plains
lakes = [(22,10,26,16),(22,W-17,26,W-11),(30,18,34,24),(30,W-25,34,W-19),(38,12,41,18),(38,W-19,41,W-13)]
for (y0,x0,y1,x1) in lakes:
    rect(y0, x0, y1, x1, "L")

# fiery-grass patches (Red Quad Lava, walkable) speckled around
for y in range(18, H - 5):
    for x in range(2, W - 2):
        if (x * 5 + y * 3) % 17 == 0 and rows[y][x] == "s":
            setcell(y, x, "f")

# Firespitting Flowers planted as turrets flanking the approach
for (py, px) in [(28,12),(28,W-13),(34,16),(34,W-17),(20,cx-8),(20,cx+8)]:
    if rows[py][px] in ("s", "f"):
        setcell(py, px, "P")

# south: spawn strip + Nexus-return portal
for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    "s": {"ground": "Scorch"},
    "f": {"ground": "Red Quad Lava"},
    "L": {"ground": "Lava"},
    "S": {"ground": "Scorch", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Scorch", "objs": [{"id": "Nexus Portal"}]},
    "F": {"ground": "Red Quad Lava", "objs": [{"id": "Firebreather"}]},
    "P": {"ground": "Scorch", "objs": [{"id": "Firespitting Flower"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "FlamingHearth.jm"))
write_jm(out, rows, legend)
