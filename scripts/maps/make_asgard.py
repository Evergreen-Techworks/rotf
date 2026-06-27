#!/usr/bin/env python3
"""Generate Asgard.jm — the ROTF Asgard dungeon (Phase C).
Grey-stone halls (Castle Stone Floor) leading to Odin's gold Throne Room.
The Norse gods are spread out (their behaviors idle until a player is within range,
so they don't all aggro at once). Spawn + Nexus-return portal at the south.
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

# Throne Room (gold) across the top
rect(3, 12, 12, W - 13, "g")
setcell(6, cx, "O")            # Odin on his throne (center-top)
setcell(9, cx - 6, "a"); setcell(9, cx + 6, "a")  # two guards flanking the throne approach

# checker-accent the main hall a bit (dark tiles)
for y in range(14, 40):
    for x in range(2, W - 2):
        if (x + y) % 6 == 0:
            setcell(y, x, ",")

# the four gods, spread across the main hall (separate quadrants)
setcell(18, cx - 10, "H")      # Heimdall
setcell(18, cx + 10, "T")      # Thor
setcell(30, cx - 10, "L")      # Loki
setcell(30, cx + 10, "E")      # Hela
for (gy, gx) in [(18, cx-10),(18, cx+10),(30, cx-10),(30, cx+10)]:
    setcell(gy+1, gx, "a")     # a guard near each god

# south: player spawn strip + a Nexus-return portal
for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")        # Nexus Portal at spawn center

legend = {
    "#": {},
    ".": {"ground": "Castle Stone Floor Tile"},
    ",": {"ground": "Castle Stone Floor Tile Dark"},
    "g": {"ground": "Gold Tile"},
    "S": {"ground": "Castle Stone Floor Tile", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Castle Stone Floor Tile", "objs": [{"id": "Nexus Portal"}]},
    "O": {"ground": "Gold Tile", "objs": [{"id": "Odin"}]},
    "H": {"ground": "Castle Stone Floor Tile", "objs": [{"id": "Heimdall"}]},
    "T": {"ground": "Castle Stone Floor Tile", "objs": [{"id": "Thor"}]},
    "L": {"ground": "Castle Stone Floor Tile", "objs": [{"id": "Loki"}]},
    "E": {"ground": "Castle Stone Floor Tile", "objs": [{"id": "Hela"}]},
    "a": {"ground": "Castle Stone Floor Tile", "objs": [{"id": "Asgard Guardian"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "Asgard.jm"))
write_jm(out, rows, legend)
