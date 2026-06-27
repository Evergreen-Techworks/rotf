#!/usr/bin/env python3
"""Import the ripped ROTF sprite atlases into the client as registered image sets.

For each PNG in assets/sprites/raw/ it:
  - copies it to client/.../assets/EmbeddedAssets_rotfNN_Embed_.png
  - writes the BitmapAsset embed class EmbeddedAssets_rotfNN_Embed_.as
  - declares it in EmbeddedAssets.as
  - adds AssetLibrary.addImageSet("rotfNN", ..., 8,8) (+ a "rotfNN_16" 16x16 set when the
    sheet is big enough) inside AssetLoader.addImages()
so objects can reference <Texture><File>rotfNN</File><Index>cell</Index>.
Idempotent. Writes docs/rotf-wiki/parsed/sprite_catalog.{json,md}.
Run:  python3 scripts/sprites/import_sheets.py
"""
import os, glob, re, json, struct, shutil

ROOT = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", ".."))
SRC = os.path.join(ROOT, "assets", "sprites", "raw")
CLI = os.path.join(ROOT, "client", "src", "kabam", "rotmg", "assets")
PARSED = os.path.join(ROOT, "docs", "rotf-wiki", "parsed")


def png_dims(path):
    with open(path, "rb") as f:
        f.read(16)
        return struct.unpack(">II", f.read(8))


def main():
    pngs = sorted(glob.glob(os.path.join(SRC, "*.png")))
    if not pngs:
        print("no sprites in", SRC); return
    sheets = []
    for i, p in enumerate(pngs):
        m = re.search(r"_(\d+)_png_1\.png$", os.path.basename(p))
        idx = int(m.group(1)) if m else i
        name = "rotf%02d" % idx
        w, h = png_dims(p)
        cells = [8] + ([16] if min(w, h) >= 16 else [])
        # copy png + write embed class
        shutil.copy(p, os.path.join(CLI, f"EmbeddedAssets_{name}Embed_.png"))
        with open(os.path.join(CLI, f"EmbeddedAssets_{name}Embed_.as"), "w") as f:
            f.write('package kabam.rotmg.assets {\nimport mx.core.*;\n\n'
                    f'[Embed(source="EmbeddedAssets_{name}Embed_.png")]\n'
                    f'public class EmbeddedAssets_{name}Embed_ extends BitmapAsset {{\n'
                    f'   public function EmbeddedAssets_{name}Embed_() {{\n      super();\n   }}\n}}\n}}\n')
        sheets.append({"name": name, "src": os.path.basename(p), "w": w, "h": h, "cells": cells})

    # --- patch EmbeddedAssets.as : declare each embed var (idempotent) ---
    ed_path = os.path.join(CLI, "EmbeddedAssets.as")
    ed = open(ed_path, encoding="utf-8", errors="replace").read()
    decls = [f"      public static var {s['name']}Embed_:Class = EmbeddedAssets_{s['name']}Embed_;"
             for s in sheets if f"{s['name']}Embed_:Class" not in ed]
    if decls:
        lines = ed.splitlines()
        last = max(i for i, l in enumerate(lines) if "Embed_:Class =" in l)
        lines[last + 1:last + 1] = ["", "      // --- ROTF imported sprite sheets ---"] + decls
        ed = "\n".join(lines)
        open(ed_path, "w", encoding="utf-8").write(ed)

    # --- patch AssetLoader.addImages() : addImageSet calls (idempotent) ---
    al_path = os.path.join(CLI, "..", "..", "..", "com", "company", "assembleegameclient", "util", "AssetLoader.as")
    al_path = os.path.normpath(al_path)
    al = open(al_path, encoding="utf-8", errors="replace").read()
    calls = []
    for s in sheets:
        for c in s["cells"]:
            sheet = s["name"] if c == 8 else f"{s['name']}_{c}"
            if f'"{sheet}"' in al:
                continue
            calls.append(f'         AssetLibrary.addImageSet("{sheet}",new EmbeddedAssets.{s["name"]}Embed_().bitmapData,{c},{c});')
    if calls:
        lines = al.splitlines()
        last = max(i for i, l in enumerate(lines) if "AssetLibrary.addImageSet(" in l)
        lines[last + 1:last + 1] = ["         // --- ROTF imported sprite sheets ---"] + calls
        al = "\n".join(lines)
        open(al_path, "w", encoding="utf-8").write(al)

    os.makedirs(PARSED, exist_ok=True)
    json.dump(sheets, open(os.path.join(PARSED, "sprite_catalog.json"), "w"), indent=1)
    with open(os.path.join(PARSED, "sprite_catalog.md"), "w") as f:
        f.write("# ROTF sprite sheets imported into the client\n\n")
        f.write("Sheet name | source rip | dims | cell sizes registered\n---|---|---|---\n")
        for s in sheets:
            f.write(f"`{s['name']}` | {s['src']} | {s['w']}x{s['h']} | {', '.join(f'{c}x{c}' for c in s['cells'])}\n")
    print(f"imported {len(sheets)} sheets; new decls {len(decls)}, new addImageSet {len(calls)}")
    print("catalog: docs/rotf-wiki/parsed/sprite_catalog.md")


if __name__ == "__main__":
    main()
