#!/usr/bin/env python3
"""Import the standard DECA sprite sheets that the real ROTF data references but my
realm-cli base lacks. PNGs were fetched from static.drips.pw (authoritative named sheets)
into /tmp/dripsheets. This copies them into the client, makes embed classes, declares
them in EmbeddedAssets.as, and registers each under the EXACT sheet name the ROTF objects
reference (in AssetLoader.addImages) at the right cell size. Idempotent."""
import os, shutil, re

ROOT = os.path.normpath(os.path.join(os.path.dirname(__file__), ".."))
SRC = "/tmp/dripsheets"
ASSETS = os.path.join(ROOT, "client/src/kabam/rotmg/assets")
EA = os.path.join(ASSETS, "EmbeddedAssets.as")
AL = os.path.join(ROOT, "client/src/com/company/assembleegameclient/util/AssetLoader.as")

# (sheet-name-as-referenced, drips-png-basename, cellW, cellH)
REG = [
    ("oryxHordeObjects8x8",    "oryxHordeObjects8x8",    8, 8),
    ("oryxHordeObjects16x16",  "oryxHordeObjects16x16", 16, 16),
    ("oryxHordeChars8x8",      "oryxHordeChars8x8",      8, 8),
    ("oryxHordeChars16x16",    "oryxHordeChars16x16",   16, 16),
    ("buffedBunnyObjects8x8",  "buffedBunnyObjects8x8",  8, 8),
    ("buffedBunnyObjects16x16","buffedBunnyObjects16x16",16, 16),
    ("buffedBunnyChars16x16",  "buffedBunnyChars16x16", 16, 16),
    ("mountainTempleObjects8x8","mountainTempleObjects8x8",8, 8),
    ("mountainTempleChars16x16","mountainTempleChars16x16",16,16),
    ("d3LofiObjEmbed",         "d3LofiObj",              8, 8),
    ("d3LofiObjEmbed16",       "d3LofiObj",             16, 16),
    ("d3Chars8x8rEmbed",       "d3Chars8x8r",            8, 8),
    ("d2LofiObjEmbed",         "d2LofiObj",              8, 8),
    ("petsDivine",             "petsDivine",             8, 8),
]

# unique png basenames -> embed var "<base>Embed_"
unique = sorted({png for _, png, _, _ in REG})
ea = open(EA, encoding="utf-8").read()
al = open(AL, encoding="utf-8").read()
new_decls, copied = [], 0
for png in unique:
    var = f"{png}Embed_"
    cls = f"EmbeddedAssets_{var}"
    shutil.copy(os.path.join(SRC, png + ".png"), os.path.join(ASSETS, cls + ".png"))
    copied += 1
    open(os.path.join(ASSETS, cls + ".as"), "w").write(
        f'package kabam.rotmg.assets {{\nimport mx.core.*;\n\n'
        f'[Embed(source="{cls}.png")]\n'
        f'public class {cls} extends BitmapAsset {{\n   public function {cls}() {{ super(); }}\n}}\n}}\n')
    if f"var {var}:Class" not in ea:
        new_decls.append(f"      public static var {var}:Class = {cls};")

# inject decls after the ROTF boss sheet decl
if new_decls:
    anchor = "      public static var rotfBossEmbed_:Class = EmbeddedAssets_rotfBossEmbed_;"
    ea = ea.replace(anchor, anchor + "\n\n      // --- DECA sheets the ROTF data needs (from drips.pw) ---\n" + "\n".join(new_decls))
    open(EA, "w", encoding="utf-8").write(ea)

# inject addImageSet calls before the close of addImages() (after the rotfBoss line)
calls = []
for sheet, png, cw, ch in REG:
    line = f'         AssetLibrary.addImageSet("{sheet}",new EmbeddedAssets.{png}Embed_().bitmapData,{cw},{ch});'
    if f'"{sheet}"' not in al:
        calls.append(line)
if calls:
    anchor = '         AssetLibrary.addImageSet("rotfBoss",new EmbeddedAssets.rotfBossEmbed_().bitmapData,16,16);'
    al = al.replace(anchor, anchor + "\n         // --- DECA sheets the ROTF data needs (from drips.pw) ---\n" + "\n".join(calls))
    open(AL, "w", encoding="utf-8").write(al)

print(f"copied {copied} sheet PNGs; new decls {len(new_decls)}; new addImageSet {len(calls)}")
