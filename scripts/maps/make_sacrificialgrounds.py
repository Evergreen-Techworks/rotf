#!/usr/bin/env python3
"""Generate SacrificialGrounds.jm — ROTF Sacrificial Grounds (Medium reptilian-temple dungeon).
A jungle-temple altar: south spawn + Nexus-return -> sandstone approach -> the central ALTAR where the
Devoted Shaman performs the sacrifice. He summons Reptilian Hunters and, at low HP, awakens the Reptilian
God right there (the God is spawned by his behavior, not placed). A few Hunters are seeded on the approach.
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 48, 48
void = "#" * W

def blank():
    rows = [void, void]
    base = "#" + "j" * (W - 2) + "#"
    for _ in range(H - 4):
        rows.append(base)
    rows += [void, void]
    return rows

rows = blank()
def setcell(y, x, ch):
    row = list(rows[y]); row[x] = ch; rows[y] = "".join(row)

cx = W // 2

# central altar (Sandstone) where the Shaman performs the ritual
for y in range(10, 24):
    for x in range(cx - 9, cx + 10):
        setcell(y, x, "a")
setcell(17, cx, "H")          # Devoted Shaman (on the altar)

# sandstone approach causeway
for y in range(24, H - 5):
    setcell(y, cx, "a"); setcell(y, cx - 1, "a"); setcell(y, cx + 1, "a")

# seeded Reptilian Hunters guarding the approach
for (py, px) in [(30, 13), (30, W - 14), (35, 18), (35, W - 19), (27, cx - 8), (27, cx + 8)]:
    setcell(py, px, "R")

for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    "j": {"ground": "Jungle Temple Floor"},
    "a": {"ground": "Sandstone Tile"},
    "S": {"ground": "Jungle Temple Floor", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Jungle Temple Floor", "objs": [{"id": "Nexus Portal"}]},
    "H": {"ground": "Sandstone Tile", "objs": [{"id": "Devoted Shaman"}]},
    "R": {"ground": "Jungle Temple Floor", "objs": [{"id": "Reptilian Hunter"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "SacrificialGrounds.jm"))
write_jm(out, rows, legend)
