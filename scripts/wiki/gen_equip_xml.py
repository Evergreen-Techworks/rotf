#!/usr/bin/env python3
"""Generate ROTF equipment <Object> XML from parsed wiki items.

Reads docs/rotf-wiki/parsed/items.json, emits:
  - server/common/resources/xml/EmbeddedData_ROTF_ItemsCXML.xml   (auto-globbed by server)
  - client/src/kabam/rotmg/assets/EmbeddedData_ROTF_ItemsCXML.dat  (same XML, client embed)
  - client/.../EmbeddedData_ROTF_ItemsCXML.as                      (embed class)
  - patches client/.../EmbeddedData.as objectFiles[] to load it (idempotent)
Type IDs are assigned from 0x8000+ (above the 0x7F22 vanilla max, within ushort) and kept
stable in docs/rotf-wiki/parsed/typemap.json.  Run after parse_items.py.

Grounded in the vanilla EmbeddedData_EquipCXML.xml: each slot borrows a vanilla item's
Texture (placeholder until ROTF sprites are imported), projectile ObjectId, and Sound.
"""
import os, re, json, glob, html

HERE = os.path.dirname(__file__)
ROOT = os.path.normpath(os.path.join(HERE, "..", ".."))
PARSED = os.path.join(ROOT, "docs", "rotf-wiki", "parsed")
SRV_XML = os.path.join(ROOT, "server", "common", "resources", "xml")
CLI_ASSETS = os.path.join(ROOT, "client", "src", "kabam", "rotmg", "assets")
BASE_ID = 0x8000

SLOT_TYPE = {  # wiki slot name -> engine SlotType (verified against vanilla equip XML)
    "sword": 1, "dagger": 2, "bow": 3, "tome": 4, "shield": 5, "leather": 6,
    "heavy": 7, "wand": 8, "ring": 9, "spell": 11, "seal": 12, "cloak": 13,
    "robe": 14, "quiver": 15, "helm": 16, "staff": 17, "poison": 18, "skull": 19,
    "trap": 20, "orb": 21, "prism": 22, "scepter": 23, "katana": 24, "star": 25,
}
WEAPON_SLOTS = {1, 2, 3, 8, 17, 24}  # carry the main attack projectile
STAT_IDX = {  # wiki stat name -> StatsType index (from wServer/realm/Stats.cs)
    "att": 20, "attack": 20, "def": 21, "defense": 21, "spd": 22, "speed": 22,
    "dex": 28, "dexterity": 28, "vit": 26, "vitality": 26, "wis": 27, "wisdom": 27,
    "hp": 0, "life": 0, "health": 0, "mp": 3, "mana": 3,
}


def esc(s):
    return html.escape(str(s), quote=True)


def num(v, default=None):
    if v is None:
        return default
    m = re.search(r"-?\d+(\.\d+)?", str(v))
    return m.group(0) if m else default


def slot_templates():
    """For each SlotType, grab a vanilla item to borrow texture/projectile/sound."""
    xml = open(os.path.join(SRV_XML, "EmbeddedData_EquipCXML.xml"), encoding="utf-8", errors="replace").read()
    tmpl = {}
    for b in re.findall(r"<Object [^>]*>.*?</Object>", xml, re.S):
        st = re.search(r"<SlotType>(\d+)</SlotType>", b)
        if not st:
            continue
        st = int(st.group(1))
        if st in tmpl:
            continue
        tex = re.search(r"<Texture>(.*?)</Texture>", b, re.S)
        proj = re.search(r"<ObjectId>([^<]*)</ObjectId>", b)
        spd = re.search(r"<Speed>([^<]*)</Speed>", b)
        life = re.search(r"<LifetimeMS>([^<]*)</LifetimeMS>", b)
        snd = re.search(r"<Sound>([^<]*)</Sound>", b)
        tmpl[st] = {
            "texture": tex.group(1).strip() if tex else "<File>lofiObj5</File><Index>0x30</Index>",
            "proj": proj.group(1) if proj else "Blade",
            "speed": spd.group(1) if spd else "100",
            "life": life.group(1) if life else "600",
            "sound": snd.group(1) if snd else None,
        }
    return tmpl


def load_typemap():
    p = os.path.join(PARSED, "typemap.json")
    return json.load(open(p)) if os.path.exists(p) else {}


def main():
    items = json.load(open(os.path.join(PARSED, "items.json")))
    tmpl = slot_templates()
    typemap = load_typemap()
    used = {int(v, 16) for v in typemap.values()}
    nxt = BASE_ID

    def assign(name):
        nonlocal nxt
        if name in typemap:
            return int(typemap[name], 16)
        while nxt in used:
            nxt += 1
        used.add(nxt)
        typemap[name] = "0x%x" % nxt
        return nxt

    out, skipped, gen = ['<Objects>'], [], 0
    for it in items:
        name = it.get("name", "").strip()
        slot_name = it.get("slot", "").strip().lower()
        if not name or slot_name not in SLOT_TYPE:
            skipped.append((it.get("_page"), slot_name)); continue
        st = SLOT_TYPE[slot_name]
        t = tmpl.get(st, tmpl.get(1))
        tid = assign(name)
        L = [f'    <Object type="0x{tid:x}" id="{esc(name)}">',
             '        <Class>Equipment</Class>', '        <Item/>',
             f'        <Texture>{t["texture"]}</Texture>',
             f'        <SlotType>{st}</SlotType>']
        tier = it.get("tier", "").strip()
        if tier.isdigit():
            L.append(f'        <Tier>{tier}</Tier>')
        elif tier.upper().startswith("T") and tier[1:].isdigit():
            L.append(f'        <Tier>{tier[1:]}</Tier>')
        # UT/ST/GL -> no <Tier> (matches vanilla untiered items)
        if it.get("desc"):
            L.append(f'        <Description>{esc(it["desc"])}</Description>')
        fb = num(it.get("fameb"))
        if fb:
            L.append(f'        <FameBonus>{fb}</FameBonus>')
        fp = num(it.get("feedp"))
        if fp:
            L.append(f'        <feedPower>{fp}</feedPower>')
        if st in WEAPON_SLOTS or num(it.get("minDmg")):
            mn = num(it.get("minDmg"), "50"); mx = num(it.get("maxDmg"), mn)
            spd = num(it.get("projSpeed"), t["speed"]); life = num(it.get("lifetime"), t["life"])
            P = ['        <Projectile>', f'            <ObjectId>{esc(t["proj"])}</ObjectId>',
                 f'            <Speed>{spd}</Speed>',
                 f'            <MinDamage>{mn}</MinDamage>', f'            <MaxDamage>{mx}</MaxDamage>',
                 f'            <LifetimeMS>{life}</LifetimeMS>']
            if str(it.get("multihit", "")).lower().startswith(("y", "t")):
                P.append('            <MultiHit/>')
            if str(it.get("pierce", "")).lower().startswith(("y", "t")):
                P.append('            <PassesCover/>')
            P.append('        </Projectile>')
            L += P
            rof = num(it.get("rof"))
            if rof:
                L.append(f'        <RateOfFire>{round(float(rof)/100.0, 3)}</RateOfFire>')
            shots = num(it.get("shots"))
            if shots and int(float(shots)) > 1:
                L.append(f'        <NumProjectiles>{int(float(shots))}</NumProjectiles>')
                L.append('        <ArcGap>11.25</ArcGap>')
        for stat, amt in it.get("bonuses_parsed", []):
            idx = STAT_IDX.get(stat.strip().lower())
            a = num(amt)
            if idx is not None and a:
                L.append(f'        <ActivateOnEquip stat="{idx}" amount="{a}">IncrementStat</ActivateOnEquip>')
        if t.get("sound"):
            L.append(f'        <Sound>{esc(t["sound"])}</Sound>')
        L.append('        <BagType>0</BagType>')
        L.append('    </Object>')
        out.append("\n".join(L)); gen += 1
    out.append('</Objects>\n')
    xml = "\n".join(out)

    # write server + client data
    open(os.path.join(SRV_XML, "EmbeddedData_ROTF_ItemsCXML.xml"), "w", encoding="utf-8").write(xml)
    open(os.path.join(CLI_ASSETS, "EmbeddedData_ROTF_ItemsCXML.dat"), "w", encoding="utf-8").write(xml)
    open(os.path.join(CLI_ASSETS, "EmbeddedData_ROTF_ItemsCXML.as"), "w", encoding="utf-8").write(
        'package kabam.rotmg.assets {\nimport mx.core.*;\n\n'
        '[Embed(source="EmbeddedData_ROTF_ItemsCXML.dat", mimeType="application/octet-stream")]\n'
        'public class EmbeddedData_ROTF_ItemsCXML extends ByteArrayAsset {\n'
        '   public function EmbeddedData_ROTF_ItemsCXML() {\n      super();\n   }\n}\n}\n')

    # patch EmbeddedData.as objectFiles[] (idempotent)
    ed_path = os.path.join(CLI_ASSETS, "EmbeddedData.as")
    ed = open(ed_path, encoding="utf-8", errors="replace").read()
    if "ROTF_ItemsCXML" not in ed:
        ed = ed.replace(
            "private static const EquipCXML:Class = EmbeddedData_EquipCXML;",
            "private static const EquipCXML:Class = EmbeddedData_EquipCXML;\n"
            "      private static const ROTF_ItemsCXML:Class = EmbeddedData_ROTF_ItemsCXML;", 1)
        ed = ed.replace("new EquipCXML(),", "new EquipCXML(),new ROTF_ItemsCXML(),", 1)
        open(ed_path, "w", encoding="utf-8").write(ed)
        patched = True
    else:
        patched = False

    json.dump(typemap, open(os.path.join(PARSED, "typemap.json"), "w"), indent=1)
    print(f"generated {gen} items (type 0x{BASE_ID:x}..0x{max(used):x}); skipped {len(skipped)}")
    if skipped:
        print("  skipped (unmapped slot):", skipped[:10])
    print(f"client EmbeddedData.as patched: {patched}")


if __name__ == "__main__":
    main()
