#!/usr/bin/env python3
"""Generate TwilightNecropolis.jm — ROTF Twilight Necropolis (graveyard/tomb dungeon).
VIDEO-DOCUMENTED progression, south -> north:
  green forest ENTRANCE (Grass) -> gothic gray/black STONE corridors (Dark Cobblestone) ->
  PURPLE rooms (Purple Stone Dark) -> RED CROSS-PATTERN final arena (Red Quad + Red Closed X).
Bosses placed by progression: Necro Doggo (gothic), Diagon + Grave Caretaker (purple), Gravedigger
(red arena, north). Minions (Troll Matriarch / Ghost Bride) seeded; Gravedigger summons his own.
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

# --- biome bands (south=entrance -> north=final arena) ---
rect(38, H - 5, 1, W - 2, "G")   # green forest entrance (south)
rect(27, 37, 1, W - 2, "c")      # gothic stone corridors
rect(17, 26, 1, W - 2, "u")      # purple rooms
rect(2, 16, 1, W - 2, "r")       # red final arena (north)

# --- RED CROSS-PATTERN arena: draw a big X of darker red across the arena ---
for i in range(0, 15):
    for (yy, xx) in [(2 + i, cx - 7 + i), (2 + i, cx + 7 - i)]:
        if 1 < xx < W - 1 and 2 <= yy <= 16:
            setcell(yy, xx, "x")
# vertical + horizontal cross bars too (tombstone cross)
for yy in range(2, 17):
    setcell(yy, cx, "x")
for xx in range(cx - 9, cx + 10):
    setcell(9, xx, "x")

# --- bosses by progression ---
setcell(7, cx, "Q")              # Gravedigger (red arena, north)
setcell(21, cx - 9, "I")         # Diagon (purple room, left)
setcell(21, cx + 9, "C")         # Grave Caretaker (purple room, right)
setcell(32, cx, "K")             # Necro Doggo (gothic corridor)

# seeded undead minions
for (py, px) in [(30, 10), (30, W - 11), (34, 16), (34, W - 17)]:
    setcell(py, px, "M")         # Troll Matriarch
for (py, px) in [(20, 12), (24, W - 13), (23, cx)]:
    setcell(py, px, "B")         # Ghost Bride

# south: spawn strip + Nexus-return portal (in the green entrance)
for x in range(cx - 3, cx + 4):
    setcell(H - 5, x, "S")
setcell(H - 5, cx, "N")

legend = {
    "#": {},
    ".": {"ground": "Castle Stone Floor Tile Dark"},
    "G": {"ground": "Grass"},
    "c": {"ground": "Dark Cobblestone"},
    "u": {"ground": "Purple Stone Dark"},
    "r": {"ground": "Red Quad"},
    "x": {"ground": "Red Closed"},
    "S": {"ground": "Grass", "regions": [{"id": "Spawn"}]},
    "N": {"ground": "Grass", "objs": [{"id": "Nexus Portal"}]},
    "Q": {"ground": "Red Closed", "objs": [{"id": "Gravedigger"}]},
    "I": {"ground": "Purple Stone Dark", "objs": [{"id": "Diagon"}]},
    "C": {"ground": "Purple Stone Dark", "objs": [{"id": "Grave Caretaker"}]},
    "K": {"ground": "Dark Cobblestone", "objs": [{"id": "Necro Doggo"}]},
    "M": {"ground": "Dark Cobblestone", "objs": [{"id": "Troll Matriarch"}]},
    "B": {"ground": "Purple Stone Dark", "objs": [{"id": "Ghost Bride"}]},
}

out = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "TwilightNecropolis.jm"))
write_jm(out, rows, legend)
