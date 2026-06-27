#!/usr/bin/env python3
"""Generate a RotMG server .jm map from an ASCII layout + legend.

The engine (common/terrain/Json2Wmap.cs) reads a .jm as:
  { "width":W, "height":H, "data":<base64 of zlib bytes>, "dict":[ loc, ... ] }
where the decompressed bytes are H*W Int16 cell indices, read row-major (y outer,
x inner) via NReader.ReadInt16 == BIG-ENDIAN (NetworkToHostOrder). Each index points
into `dict`; a loc = {"ground":name?, "objs":[{"id":..,"name":..}]?, "regions":[{"id":..}]?}.

Usage (as a module): make_jm(rows, legend) -> dict ready for json.dump.
  rows   : list[str]  ASCII grid (rows top->bottom). Short rows are padded with rows[0]'s void char.
  legend : dict[str -> dict]  char -> loc dict (the JSON loc). Order defines dict indices.
           The FIRST legend entry is the void/border tile (used to pad).
"""
import json, zlib, struct, base64


def make_jm(rows, legend):
    chars = list(legend.keys())
    idx_of = {c: i for i, c in enumerate(chars)}
    void = chars[0]
    w = max(len(r) for r in rows)
    h = len(rows)
    grid = []
    for r in rows:
        r = r + void * (w - len(r))
        for c in r:
            if c not in idx_of:
                raise ValueError(f"char {c!r} not in legend")
            grid.append(idx_of[c])
    raw = b"".join(struct.pack(">h", i) for i in grid)
    data = base64.b64encode(zlib.compress(raw, 9)).decode("ascii")
    return {"width": w, "height": h, "data": data, "dict": [legend[c] for c in chars]}


def write_jm(path, rows, legend):
    with open(path, "w") as f:
        json.dump(make_jm(rows, legend), f)
    print(f"wrote {path}  ({max(len(r) for r in rows)}x{len(rows)}, {len(legend)} dict entries)")


if __name__ == "__main__":
    # ---- Illusion dungeon: a 44x44 grass arena, void border, central boss, spawn at south ----
    W, H = 44, 44
    void = "#" * W
    floor = "#" + "." * (W - 2) + "#"
    rows = [void, void]
    for _ in range(H - 4):
        rows.append(floor)
    rows += [void, void]

    def setcell(y, x, ch):
        row = list(rows[y]); row[x] = ch; rows[y] = "".join(row)

    cx, cy = W // 2, H // 2
    setcell(cy, cx, "B")                       # The Illusionist (center)
    for dx, dy in [(-5, -3), (5, -3), (-5, 3), (5, 3)]:
        setcell(cy + dy, cx + dx, "m")         # a few pre-placed Mirages
    for x in range(cx - 3, cx + 4):            # player spawn strip near south wall
        setcell(H - 4, x, "S")

    legend = {
        "#": {},                                                   # void / border (no ground)
        ".": {"ground": "Grass"},                                  # floor
        "S": {"ground": "Grass", "regions": [{"id": "Spawn"}]},    # player entry
        "B": {"ground": "Grass", "objs": [{"id": "The Illusionist"}]},
        "m": {"ground": "Grass", "objs": [{"id": "Illusion Mirage"}]},
    }
    import os
    out = os.path.join(os.path.dirname(__file__), "..", "..",
                       "server", "common", "resources", "worlds", "Illusion.jm")
    write_jm(os.path.normpath(out), rows, legend)
