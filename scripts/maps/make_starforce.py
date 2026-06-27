#!/usr/bin/env python3
"""Generate Starforce.jm — ROTF Starforce tech/space zone.
South spawn + Nexus-return -> a metal-plated approach across open SPACE -> a central grey-metal
ARENA where the 4 bosses stand: The Zuck + Gem Gem (new) and Cracked Core + Ortar (reused from
OCBosses). Grey-tech tiles over a Space backdrop. Ground-tile names verified present in
EmbeddedData_GroundCXML.xml: 'Space', 'Metal Plates', 'Grey Squares', 'Grey Pathway'.
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 52, 52
void = "#" * W

def blank():
    rows = [void, void]
    floor = "#" + "." * (W - 2) + "#"   # default floor = open Space
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

# central grey-metal ARENA (Metal Plates floor, Grey-Squares inner ring)
rect(12, 32, 11, W - 12, "m")        # metal-plate arena
rect(18, 26, 18, W - 19, "q")        # grey-squares inner platform

# the 4 bosses, spaced around the inner platform
setcell(16, cx, "Z")                 # The Zuck (north)
setcell(28, cx, "G")                 # Gem Gem (south of center)
setcell(22, 16, "C")                 # Cracked Core (west)  -- reused OCBoss
setcell(22, W - 17, "O")             # Ortar (east)         -- reused OCBoss

# metal-plated approach corridor (lower-middle) with grey-pathway accents
for y in range(33, 43):
    for x in range(cx - 4, cx + 5):
        setcell(y, x, "p")

# south: spawn strip + Nexus-return portal
for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    ".": {"ground": "Space"},
    "m": {"ground": "Metal Plates"},
    "q": {"ground": "Grey Squares"},
    "p": {"ground": "Grey Pathway"},
    "S": {"ground": "Metal Plates", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Metal Plates", "objs": [{"id": "Nexus Portal"}]},
    "Z": {"ground": "Grey Squares", "objs": [{"id": "The Zuck"}]},
    "G": {"ground": "Grey Squares", "objs": [{"id": "Gem Gem"}]},
    "C": {"ground": "Grey Squares", "objs": [{"id": "Cracked Core"}]},
    "O": {"ground": "Grey Squares", "objs": [{"id": "Ortar"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "Starforce.jm"))
write_jm(out, rows, legend)
