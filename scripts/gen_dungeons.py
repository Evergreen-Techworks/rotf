#!/usr/bin/env python3
"""Generate ROTF dungeons (map + .jw + portal) around imported real bosses, using the
proven Illusion/Craig recipe. Each dungeon: a Grass arena, boss in the center, a few
matching minions scattered, a south spawn strip, and a Portal object (0x82xx) wired via
the world's portals[]. Portals are appended to EmbeddedData_ROTF_ObjectsCXML.xml.

Only places enemies that actually exist as loaded objects (checked against the import).
"""
import json, os, sys, re
sys.path.insert(0, os.path.join(os.path.dirname(__file__), "maps"))
from make_jm import make_jm

ROOT = os.path.normpath(os.path.join(os.path.dirname(__file__), ".."))
WORLDS = os.path.join(ROOT, "server/common/resources/worlds")
OBJX = os.path.join(ROOT, "server/common/resources/xml/EmbeddedData_ROTF_ObjectsCXML.xml")

# ALL loaded enemy object names (vanilla + imported), straight from server XML.
import glob as _glob
enemy_names = set()
for _f in _glob.glob(os.path.join(ROOT, "server/common/resources/xml/*.xml")):
    for _o in re.findall(r"<Object\b.*?</Object>", open(_f, encoding="utf-8", errors="replace").read(), re.S):
        if "<Enemy" in _o:
            _m = re.search(r'id="([^"]+)"', _o)
            if _m:
                enemy_names.add(_m.group(1))

# (Dungeon display name, world-file base, portal type hex, boss id, minion name-substrings)
DUNGEONS = [
    ("Limon's Lair",    "LimonsLair",    0x8202, "Limon the Sprite God",        ["Native", "Limon Element"]),
    ("Deadwater Docks", "DeadwaterDocks",0x8203, "Thessal the Mermaid Goddess", ["Fishman", "Sea ", "Coral", "Deep Sea", "Ink", "Squid"]),
    ("Esben's Lair",    "EsbensLair",    0x8204, "ic Esben the Unwilling",      ["ic "]),
    ("Slime God Den",   "SlimeGodDen",   0x8205, "DS Gulpord the Slime God",    ["DS "]),
    ("Bone Dungeon",    "BoneDungeon",   0x8206, "Bonegrind the Butcher",       ["Pile of Bones", "Skeleton", "Bone"]),
    ("Chicken Coop",    "ChickenCoop",   0x8207, "Evil Chicken God",            ["Evil Chicken", "Chicken"]),
    ("Ruthven's Castle","RuthvensCastle",0x8208, "Lord Ruthven",                ["Vampire", "Bat", "Coffin"]),
    ("Bunny Hollow",    "BunnyHollow",   0x8209, "BB Biff the Buffed Bunny",    ["BB Bunny", "BB Egg", "BB Colored"]),
]

def minions_for(subs, boss):
    out = []
    for n in enemy_names:
        if n == boss:
            continue
        if any(s.lower() in n.lower() for s in subs):
            out.append(n)
    return sorted(out)[:6]

portals_xml = []
made = []
for disp, base, ptype, boss, subs in DUNGEONS:
    if boss not in enemy_names:
        print(f"SKIP {disp}: boss '{boss}' not loaded"); continue
    minions = minions_for(subs, boss)
    W = H = 44
    rows = ["#"*W, "#"*W] + ["#" + "."*(W-2) + "#" for _ in range(H-4)] + ["#"*W, "#"*W]
    def setc(y, x, ch):
        r = list(rows[y]); r[x] = ch; rows[y] = "".join(r)
    cx, cy = W//2, H//2
    setc(cy, cx, "B")
    spots = [(-7,-5),(7,-5),(-7,5),(7,5),(-10,0),(10,0)]
    for i, m in enumerate(minions):
        dx, dy = spots[i % len(spots)]; setc(cy+dy, cx+dx, chr(ord('a')+i))
    for x in range(cx-3, cx+4): setc(H-4, x, "S")
    legend = {
        "#": {}, ".": {"ground": "Grass"},
        "S": {"ground": "Grass", "regions": [{"id": "Spawn"}]},
        "B": {"ground": "Grass", "objs": [{"id": boss}]},
    }
    for i, m in enumerate(minions):
        legend[chr(ord('a')+i)] = {"ground": "Grass", "objs": [{"id": m}]}
    json.dump(make_jm(rows, legend), open(os.path.join(WORLDS, base + ".jm"), "w"))
    jw = {"id": 0, "name": disp, "sbName": disp, "difficulty": 4, "background": 0,
          "blocking": 0, "restrictTp": False, "showDisplays": True, "persist": False,
          "portals": [ptype]}
    json.dump(jw, open(os.path.join(WORLDS, base + ".jw"), "w"), indent=2)
    portals_xml.append(
        f'    <Object type="0x{ptype:04x}" id="{disp} Portal">\n'
        f'        <Class>Portal</Class>\n        <IntergamePortal/>\n'
        f'        <DungeonName>{disp}</DungeonName>\n'
        f'        <Texture><File>lofiObj3</File><Index>0x21f</Index></Texture>\n'
        f'        <Size>120</Size>\n        <ShowName/>\n    </Object>')
    made.append((disp, boss, minions, ptype))
    print(f"OK {disp}: boss={boss}, minions={minions}, portal=0x{ptype:04x}")

# append portals to the ROTF objects XML (idempotent: skip any already present)
d = open(OBJX, encoding="utf-8").read()
new = [p for p in portals_xml if re.search(r'id="%s' % re.escape(p.split('id="')[1].split('"')[0]), d) is None]
if new:
    d = d.rstrip()
    assert d.endswith("</Objects>")
    d = d[:-len("</Objects>")].rstrip() + "\n\n" + "\n".join(new) + "\n</Objects>\n"
    open(OBJX, "w", encoding="utf-8").write(d)
print(f"\nadded {len(new)} portal object(s) to {os.path.basename(OBJX)}")
print(f"dungeons generated: {len(made)}")
