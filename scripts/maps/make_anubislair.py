#!/usr/bin/env python3
"""Generate AnubisLair.jm — ROTF Anubi's Lair (gold sandstone Egyptian temple).
South spawn + Nexus-return -> sand antechamber -> Anubis's gold sanctum. Anubis's
4 Pillars are spawned by his behavior, not placed here (mirrors Ra's Staves in make_gtu.py).
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 48, 48
void = "#" * W

def blank():
    rows = [void, void]
    floor = "#" + "." * (W - 2) + "#"
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

# central gold sanctum (Anubis; Pillars spawn around him)
rect(14, 30, 12, W - 13, "g")
setcell(22, cx, "A")      # Anubis

# sand antechamber accents around the sanctum
for y in range(31, 41):
    for x in range(2, W - 2):
        if (x + y) % 5 == 0:
            setcell(y, x, ",")

# south: spawn strip + Nexus-return portal
for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    ".": {"ground": "Sandstone Tile"},
    ",": {"ground": "Sand Covered Tile"},
    "g": {"ground": "Gold Tile"},
    "S": {"ground": "Sandstone Tile", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Sandstone Tile", "objs": [{"id": "Nexus Portal"}]},
    "A": {"ground": "Gold Tile", "objs": [{"id": "Anubis"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "AnubisLair.jm"))
write_jm(out, rows, legend)
