#!/usr/bin/env python3
"""Validate generated + existing game XML before a build.

Checks across all server EmbeddedData_*.xml:
  - duplicate type IDs (fatal: two objects share a type)
  - duplicate object IDs/names (warn: ROTF name collides with vanilla)
  - ROTF item textures reference sprite sheets present in the client
  - server vs client ROTF data files are byte-identical
Exit code 1 if any fatal issue.  Run:  python3 scripts/wiki/validate_xml.py
"""
import os, re, glob, sys

ROOT = os.path.normpath(os.path.join(os.path.dirname(__file__), "..", ".."))
SRV_XML = os.path.join(ROOT, "server", "common", "resources", "xml")
CLI_ASSETS = os.path.join(ROOT, "client", "src", "kabam", "rotmg", "assets")


def main():
    types, ids, fatal, warn = {}, {}, [], []
    for f in sorted(glob.glob(os.path.join(SRV_XML, "*.xml"))):
        txt = open(f, encoding="utf-8", errors="replace").read()
        for b in re.findall(r"<Object [^>]*>", txt):
            t = re.search(r'type="(0x[0-9A-Fa-f]+)"', b)
            i = re.search(r'id="([^"]*)"', b)
            base = os.path.basename(f)
            if t:
                key = int(t.group(1), 16)
                if key in types and types[key][1] != (i.group(1) if i else ""):
                    fatal.append(f"dup type 0x{key:x}: '{types[key][1]}'({types[key][0]}) vs '{i.group(1) if i else '?'}'({base})")
                types[key] = (base, i.group(1) if i else "")
            if i:
                nm = i.group(1)
                if nm in ids and ids[nm] != base:
                    warn.append(f"dup id '{nm}': {ids[nm]} vs {base}")
                ids[nm] = base

    # ROTF texture sheets exist in client?
    rotf = os.path.join(SRV_XML, "EmbeddedData_ROTF_ItemsCXML.xml")
    if os.path.exists(rotf):
        sheets = set(re.findall(r"<File>([^<]+)</File>", open(rotf, encoding="utf-8").read()))
        for s in sorted(sheets):
            png = os.path.join(CLI_ASSETS, f"EmbeddedAssets_{s}Embed_.png")
            if not os.path.exists(png):
                warn.append(f"texture sheet '{s}' has no client EmbeddedAssets_{s}Embed_.png")
        # server vs client data identical?
        cli = os.path.join(CLI_ASSETS, "EmbeddedData_ROTF_ItemsCXML.dat")
        if os.path.exists(cli) and open(cli).read() != open(rotf).read():
            fatal.append("server ROTF xml != client ROTF .dat (regenerate)")

    print(f"objects scanned: {len(types)} | unique ids: {len(ids)}")
    for w in warn:
        print("  WARN:", w)
    for e in fatal:
        print("  FATAL:", e)
    print("RESULT:", "FAIL" if fatal else "OK")
    sys.exit(1 if fatal else 0)


if __name__ == "__main__":
    main()
