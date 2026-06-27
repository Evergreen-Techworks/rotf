#!/usr/bin/env python3
"""Generate IcyPeaks.jm — ROTF Icy Peaks (seasonal ice dungeon).
South spawn + Nexus-return -> snow corridors -> the Ice Queen's frozen FLOWER-GARDEN arena
(grass center ringed by snow/ice). The Queen's 4 Towers are spawned by her behavior, not placed
here (mirrors Ra's Staves in make_gtu.py / Anubis's Pillars in make_anubislair.py).
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 48, 48
void = "#" * W

def blank():
    rows = [void, void]
    floor = "#" + "s" * (W - 2) + "#"   # default floor = Snow White
    for _ in range(H - 4):
        rows.append(floor)
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

# central frozen FLOWER-GARDEN arena (grass ring center; Ice Queen + Towers spawn here)
rect(14, 30, 12, W - 13, "g")
setcell(22, cx, "Q")      # Ice Queen

# ice-accent the approach (lower-middle)
for y in range(31, 41):
    for x in range(2, W - 2):
        if (x + y) % 5 == 0:
            setcell(y, x, "i")

# south: spawn strip + Nexus-return portal
for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    "s": {"ground": "Snow White"},
    "i": {"ground": "Ice"},
    "g": {"ground": "Grass"},
    "S": {"ground": "Snow White", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Snow White", "objs": [{"id": "Nexus Portal"}]},
    "Q": {"ground": "Grass", "objs": [{"id": "Ice Queen"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "IcyPeaks.jm"))
write_jm(out, rows, legend)
