#!/usr/bin/env python3
"""Generate TombOfDecayingDeath.jm — ROTF Tomb of Decaying Death (returning legacy graveyard dungeon).
Eldritch graveyard: south spawn + Nexus-return -> winding cemetery paths through grave plots ->
central Grey-Dirt crypt arena where the Tombstone Carrier rises. Minions are summoned by the boss
(and a few Buried Hands seeded in the plots), so only the Carrier is placed here.
"""
import os, sys
sys.path.insert(0, os.path.dirname(__file__))
from make_jm import write_jm

W, H = 48, 48
void = "#" * W

def blank():
    rows = [void, void]
    base = "#" + "g" * (W - 2) + "#"   # cemetery grass everywhere by default
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

# central crypt arena (Grey Dirt) where the Tombstone Carrier rises
rect(15, 30, 14, W - 15, "d")
setcell(22, cx, "T")          # Tombstone Carrier

# stone path spine from the south spawn up into the crypt
for y in range(31, H - 4):
    setcell(y, cx, "p")
    setcell(y, cx - 1, "p")
# crypt approach path
for y in range(28, 32):
    setcell(y, cx, "p")

# grave plots flanking the path — dirt piles + a couple of Buried Hands seeded
for (py, px) in [(34,10),(34,W-11),(38,16),(38,W-17),(40,8),(40,W-9),(26,9),(26,W-10),(20,8),(20,W-9)]:
    setcell(py, px, "x")      # cemetery dirt pile (grave)
for (py, px) in [(34,20),(34,W-21)]:
    setcell(py, px, "B")      # Buried Hand lurking in a plot

# dark-grass overgrowth accents
for y in range(4, H - 4):
    for x in range(2, W - 2):
        if (x * 7 + y * 3) % 23 == 0 and rows[y][x] == "g":
            setcell(y, x, "k")

# south: spawn strip + Nexus-return portal
for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    "g": {"ground": "Cemetery Grass"},
    "k": {"ground": "Dark Grass"},
    "d": {"ground": "Grey Dirt"},
    "p": {"ground": "Cemetery Stone Path"},
    "x": {"ground": "Cemetery Dirt Pile"},
    "S": {"ground": "Cemetery Stone Path", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Cemetery Stone Path", "objs": [{"id": "Nexus Portal"}]},
    "T": {"ground": "Grey Dirt", "objs": [{"id": "Tombstone Carrier"}]},
    "B": {"ground": "Cemetery Dirt Pile", "objs": [{"id": "Buried Hand"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "TombOfDecayingDeath.jm"))
write_jm(out, rows, legend)
