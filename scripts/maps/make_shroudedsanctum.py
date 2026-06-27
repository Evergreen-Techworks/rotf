#!/usr/bin/env python3
"""Generate ShroudedSanctum.jm — ROTF Shrouded Sanctum (Medium shadow dungeon).
A dark cloaked temple: south spawn + Nexus-return -> shadowed halls -> central sanctum where
Shadowscale cloaks and stalks. Shades/Cultists are summoned; Shadow Sentinels seeded as wardens.
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 48, 48
void = "#" * W

def blank():
    rows = [void, void]
    base = "#" + "d" * (W - 2) + "#"
    for _ in range(H - 4):
        rows.append(base)
    rows += [void, void]
    return rows

rows = blank()
def setcell(y, x, ch):
    row = list(rows[y]); row[x] = ch; rows[y] = "".join(row)

cx = W // 2

# central sanctum floor (Grey Quad Shadows) around Shadowscale
for y in range(11, 25):
    for x in range(cx - 10, cx + 11):
        setcell(y, x, "q")
setcell(18, cx, "Z")          # Shadowscale

# darker stone accents
for y in range(4, H - 4):
    for x in range(2, W - 2):
        if (x * 5 + y * 7) % 17 == 0 and rows[y][x] == "d":
            setcell(y, x, "c")

# Shadow Sentinels warding the approach
for (py, px) in [(28, 13), (28, W - 14), (33, 19), (33, W - 20), (22, cx - 9), (22, cx + 9)]:
    setcell(py, px, "W")

for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    "d": {"ground": "GhostGround Dark"},
    "q": {"ground": "Grey Quad Shadows"},
    "c": {"ground": "Castle Stone Floor Tile Dark"},
    "S": {"ground": "GhostGround Mid", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "GhostGround Mid", "objs": [{"id": "Nexus Portal"}]},
    "Z": {"ground": "Grey Quad Shadows", "objs": [{"id": "Shadowscale"}]},
    "W": {"ground": "GhostGround Dark", "objs": [{"id": "Shadow Sentinel"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "ShroudedSanctum.jm"))
write_jm(out, rows, legend)
