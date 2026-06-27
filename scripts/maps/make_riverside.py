#!/usr/bin/env python3
"""Generate RiversideRefuge.jm — ROTF Riverside Refuge (Medium-rank swamp / realm-Lord area).
A thick marsh: Light-Moss banks laced with Shallow-Water channels, opening onto Riverborn's
deep Dark-Water home pool at the north. South spawn + Nexus-return. Striders/Serpents/frogs are
summoned (and a few Lurkers / Living Brushes seeded in the reeds), so only Riverborn is placed.
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 48, 48
void = "#" * W

def blank():
    rows = [void, void]
    base = "#" + "m" * (W - 2) + "#"   # mossy swamp banks by default
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

# Riverborn's home pool — walkable Shallow Water, with a non-walkable Dark-Water rim for depth
rect(4, 14, 12, W - 13, "w")
rect(4, 5, 12, W - 13, "D")   # deep rim along the far (north) edge only — decorative, no-walk
setcell(9, cx, "R")           # Riverborn rises from the deep (Light-Moss island)

# winding shallow-water channels feeding the pool (the river)
for y in range(15, H - 5):
    wob = 4 if (y % 6 < 3) else -4
    for x in (cx - 2 + wob, cx - 1 + wob, cx + wob):
        if 1 < x < W - 1:
            setcell(y, x, "w")
# side tributaries
for x in range(8, W - 8):
    if (x % 9) < 2:
        setcell(24, x, "w"); setcell(34, x, "w")

# reed clumps with Lurkers / Living Brushes hiding in them
for (py, px) in [(20,10),(20,W-11),(28,14),(28,W-15),(36,9),(36,W-10),(31,cx+6),(23,cx-7)]:
    setcell(py, px, "L")

# south: spawn strip + Nexus-return portal
for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    "m": {"ground": "Light Moss"},
    "w": {"ground": "Shallow Water"},
    "D": {"ground": "Dark Water"},
    "S": {"ground": "Light Moss", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Light Moss", "objs": [{"id": "Nexus Portal"}]},
    "R": {"ground": "Light Moss", "objs": [{"id": "Riverborn"}]},
    "L": {"ground": "Light Moss", "objs": [{"id": "Living Brush"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "RiversideRefuge.jm"))
write_jm(out, rows, legend)
