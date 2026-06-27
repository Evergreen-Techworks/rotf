#!/usr/bin/env python3
"""Full import of the REAL ROTF object data (ripped from the X3.1.1 client) into the
engine. Reads assets/rotf-original-data/rotf_objects_FULL.xml, finds objects NOT already
in my base (vanilla) content nor in my hand-made ROTF files, assigns safe type ids, and
writes server XML + client .dat/.as, registering the client files in EmbeddedData.as.

Splits output into:
  EmbeddedData_ROTF_ImpEnemies  — Enemy Characters (need behaviors)
  EmbeddedData_ROTF_ImpObjects  — everything else (props/walls/portals/containers)

Also reports texture-sheet coverage (which referenced sheets my client has).
Idempotent-ish: regenerates the two Imp files from scratch each run.
"""
import re, glob, os, json

ROOT = os.path.normpath(os.path.join(os.path.dirname(__file__), ".."))
SRC = os.path.join(ROOT, "assets/rotf-original-data/rotf_objects_FULL.xml")
SXML = os.path.join(ROOT, "server/common/resources/xml")
CLI = os.path.join(ROOT, "client/src/kabam/rotmg/assets")

def objs_of(text):
    return re.findall(r"<Object\b.*?</Object>", text, re.S)

def name_of(o):
    m = re.search(r'\bid="([^"]+)"', o); return m.group(1) if m else None

def type_of(o):
    m = re.search(r'\btype="(0x[0-9a-fA-F]+)"', o); return m.group(1).lower() if m else None

def known_condition_effects():
    t = open(os.path.join(ROOT, "server/common/resources/XmlDescriptors.cs"), encoding="utf-8").read()
    m = re.search(r"enum ConditionEffectIndex\s*\{(.*?)\}", t, re.S)
    names = set(re.findall(r"([A-Za-z][A-Za-z0-9]*)\s*=", m.group(1)))
    names.add("None")
    return {n.lower(): n for n in names}  # lower -> canonical (no spaces)

_KNOWN_CE = None
def sanitize(o):
    """Drop/normalize values my engine's enums don't recognize (real ROTF used custom ones)."""
    global _KNOWN_CE
    if _KNOWN_CE is None:
        _KNOWN_CE = known_condition_effects()
    def fix_ce(m):
        val = m.group(2).strip()
        key = val.replace(" ", "").lower()
        if key in _KNOWN_CE:
            return "%s%s</ConditionEffect>" % (m.group(1), _KNOWN_CE[key])  # canonical, no space
        return ""  # unknown effect (Unstable/Darkness/...) -> remove element entirely
    return re.sub(r"(<ConditionEffect\b[^>]*>)([^<]+)</ConditionEffect>", fix_ce, o)

def main():
    real = [sanitize(o) for o in objs_of(open(SRC, encoding="utf-8").read())]

    # base (vanilla) + my hand-made ROTF content: collect names + used type ids
    base_names, used_types = set(), set()
    for f in glob.glob(os.path.join(SXML, "*.xml")):
        if "EmbeddedData_ROTF_Imp" in os.path.basename(f):
            continue  # don't count our own output as base (idempotency)
        t = open(f, encoding="utf-8", errors="replace").read()
        for o in objs_of(t):
            n = name_of(o); ty = type_of(o)
            if n: base_names.add(n)
            if ty: used_types.add(ty)

    # pick objects to import: new name, not already present, and not a duplicate
    # (the real data is 14 concatenated files, so some objects are defined twice).
    to_import = []
    seen = set()
    for o in real:
        n = name_of(o)
        if n and n not in base_names and n not in seen:
            seen.add(n)
            to_import.append(o)

    # remap projectile ObjectIds that reference objects we don't have (real data ships
    # some projectile objects we couldn't extract) -> "Blade" (a guaranteed-loaded projectile).
    # Else Projectile..ctor does IdToObjectType[missing] -> KeyNotFoundException every tick.
    valid_names = base_names | {name_of(o) for o in to_import}
    def fix_projectiles(o):
        def fix_block(m):
            blk = m.group(0)
            return re.sub(r"<ObjectId>([^<]+)</ObjectId>",
                          lambda mm: mm.group(0) if mm.group(1) in valid_names else "<ObjectId>Blade</ObjectId>",
                          blk)
        return re.sub(r"<Projectile\b.*?</Projectile>", fix_block, o, flags=re.S)
    to_import = [fix_projectiles(o) for o in to_import]

    # assign type ids: keep real id if free, else remap into 0x8300.. (skip used)
    next_id = 0x8300
    def alloc():
        nonlocal next_id
        while ("0x%04x" % next_id) in used_types:
            next_id += 1
        v = "0x%04x" % next_id; used_types.add(v); next_id += 1; return v

    enemies, others = [], []
    sheets = {}
    remapped = 0
    for o in to_import:
        ty = type_of(o); n = name_of(o)
        if ty is None or ty in used_types:
            new = alloc();
            if ty: remapped += 1
            o = re.sub(r'(\btype=")0x[0-9a-fA-F]+(")', r'\g<1>%s\g<2>' % new, o, count=1) if ty \
                else o.replace("<Object ", '<Object type="%s" ' % new, 1)
            ty = new
        else:
            used_types.add(ty)
        for sh in re.findall(r'<File>([^<]+)</File>', o):
            sheets[sh] = sheets.get(sh, 0) + 1
        (enemies if "<Enemy/>" in o or "<Enemy />" in o else others).append(o)

    def write_pair(basename, objlist, header):
        xml = "<Objects>\n  <!-- %s -->\n%s\n</Objects>\n" % (header, "\n".join(objlist))
        open(os.path.join(SXML, basename + ".xml"), "w", encoding="utf-8").write(xml)
        open(os.path.join(CLI, basename + ".dat"), "w", encoding="utf-8").write(xml)
        open(os.path.join(CLI, basename + ".as"), "w").write(
            'package kabam.rotmg.assets {\nimport mx.core.*;\n\n'
            '[Embed(source="%s.dat", mimeType="application/octet-stream")]\n'
            'public class %s extends ByteArrayAsset {\n   public function %s() { super(); }\n}\n}\n'
            % (basename, basename, basename))
        return len(objlist)

    ne = write_pair("EmbeddedData_ROTF_ImpEnemies", enemies, "REAL ROTF enemies (X3.1.1 rip) — need behaviors")
    no = write_pair("EmbeddedData_ROTF_ImpObjects", others, "REAL ROTF props/walls/portals/containers (X3.1.1 rip)")

    # register the two new files in client EmbeddedData.as (idempotent)
    ed = os.path.join(CLI, "EmbeddedData.as")
    s = open(ed, encoding="utf-8").read()
    for const, cls in [("ImpEnemiesCXML", "EmbeddedData_ROTF_ImpEnemies"),
                        ("ImpObjectsCXML", "EmbeddedData_ROTF_ImpObjects")]:
        if ("const %s:Class" % const) not in s:
            s = s.replace("private static const ROTF_ObjectsCXML:Class = EmbeddedData_ROTF_ObjectsCXML;",
                          "private static const ROTF_ObjectsCXML:Class = EmbeddedData_ROTF_ObjectsCXML;\n"
                          "      private static const %s:Class = %s;" % (const, cls))
            s = s.replace("new ROTF_ObjectsCXML(),",
                          "new ROTF_ObjectsCXML(),new %s()," % const)
    open(ed, "w", encoding="utf-8").write(s)

    # texture sheet coverage vs client AssetLoader
    al = open(os.path.join(ROOT, "client/src/com/company/assembleegameclient/util/AssetLoader.as"),
              encoding="utf-8", errors="replace").read()
    have = {sh for sh in sheets if ('"%s"' % sh) in al}
    miss = {sh: c for sh, c in sheets.items() if sh not in have and sh != "invisible"}

    print(f"imported {len(to_import)} objects: {ne} enemies, {no} others; {remapped} type-ids remapped")
    print(f"\nsprite sheets referenced: {len(sheets)} | present in client: {len(have)} | MISSING: {len(miss)}")
    if miss:
        print("  missing sheets (objects using them will render blank until added):")
        for sh, c in sorted(miss.items(), key=lambda x: -x[1]):
            print(f"    {c:4}  {sh}")
    # save the enemy roster for the behavior generator
    roster = [{"name": name_of(o), "type": type_of(o),
               "hp": int((re.search(r"<MaxHitPoints>(\d+)", o) or [0,0])[1]) if re.search(r"<MaxHitPoints>(\d+)", o) else 0,
               "proj": len(re.findall(r"<Projectile", o)),
               "group": (re.search(r"<Group>([^<]+)", o) or [None, ""])[1]}
              for o in enemies]
    json.dump(roster, open(os.path.join(ROOT, "docs/rotf-wiki/parsed/imported_enemies.json"), "w"), indent=1)
    print(f"\nenemy roster -> docs/rotf-wiki/parsed/imported_enemies.json ({len(roster)} enemies)")

if __name__ == "__main__":
    main()
