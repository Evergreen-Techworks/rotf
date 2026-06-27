#!/usr/bin/env python3
"""Generate GateToTheUnderworld.jm — ROTF Gate to the Underworld (Egyptian afterlife / Duat).
Sandstone halls: south spawn + Nexus-return -> Sun Docks -> Ra's gold arena -> north Duat
chamber with Sia & Hu. Ra's Staves are spawned by Ra's behavior, not placed here.
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

# north Duat chamber (Sia & Hu)
rect(3, 11, 10, W - 11, ",")
setcell(7, cx - 6, "I")   # Sia
setcell(7, cx + 6, "U")   # Hu

# central Ra arena (gold)
rect(17, 30, 13, W - 14, "g")
setcell(23, cx, "R")      # Ra the Sun God (staves spawn around him)

# sand-accent the Sun Docks (lower-middle)
for y in range(33, 42):
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
    "R": {"ground": "Gold Tile", "objs": [{"id": "Ra the Sun God"}]},
    "I": {"ground": "Sand Covered Tile", "objs": [{"id": "Sia"}]},
    "U": {"ground": "Sand Covered Tile", "objs": [{"id": "Hu"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "GateToTheUnderworld.jm"))
write_jm(out, rows, legend)
