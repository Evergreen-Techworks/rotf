#!/usr/bin/env python3
"""Parse {{ItemInfo}} (and {{Classbox}}) templates from the mirrored ROTF wiki into JSON.

Wiki item pages embed structured templates like:
    {{ItemInfo|name=...|slot=wand|tier=UT|minDmg=180|maxDmg=200|rof=110|...|bonuses={{Stat|att|5}}}}
This extracts them with real brace-matching (templates nest {{Stat}}/{{Classbox}}), so the
output drives gen_equip_xml.py.  Run:  python3 scripts/wiki/parse_items.py
"""
import os, re, json, glob

HERE = os.path.dirname(__file__)
WIKI = os.path.join(HERE, "..", "..", "docs", "rotf-wiki")
OUT = os.path.join(WIKI, "parsed")


def find_templates(text, name):
    """Yield the inner body of every {{<name> ... }} block, brace-matched."""
    needle = "{{" + name
    i = 0
    while True:
        s = text.find(needle, i)
        if s < 0:
            return
        depth = 0
        j = s
        while j < len(text):
            if text[j:j+2] == "{{":
                depth += 1; j += 2; continue
            if text[j:j+2] == "}}":
                depth -= 1; j += 2
                if depth == 0:
                    break
                continue
            j += 1
        yield text[s+2:j-2]  # inner (without outer {{ }})
        i = j


def split_top(body):
    """Split a template body on top-level '|' (ignoring nested {{}} and [[]])."""
    parts, depth, buf = [], 0, []
    k = 0
    while k < len(body):
        two = body[k:k+2]
        if two in ("{{", "[["):
            depth += 1; buf.append(two); k += 2; continue
        if two in ("}}", "]]"):
            depth -= 1; buf.append(two); k += 2; continue
        if body[k] == "|" and depth == 0:
            parts.append("".join(buf)); buf = []; k += 1; continue
        buf.append(body[k]); k += 1
    parts.append("".join(buf))
    return parts


def parse_stat_block(val):
    """bonuses value may contain one or more {{Stat|<name>|<amount>}} (params may be
    newline-separated). Return list of (statName, amount)."""
    out = []
    for inner in find_templates("{{" + val + "}}" if not val.strip().startswith("{{") else val, "Stat"):
        toks = [t.strip() for t in re.split(r"[|\n]+", inner) if t.strip()]
        toks = [t for t in toks if t.lower() != "stat"]
        if len(toks) >= 2:
            out.append((toks[-2], toks[-1]))
    return out


def parse_template(body):
    d, positional = {}, []
    for part in split_top(body):
        if "=" in part:
            k, v = part.split("=", 1)
            d[k.strip()] = v.strip()
        elif part.strip():
            positional.append(part.strip())
    if positional:
        d["_positional"] = positional
    if "bonuses" in d and "{{Stat" in d["bonuses"]:
        d["bonuses_parsed"] = parse_stat_block(d["bonuses"])
    return d


def main():
    os.makedirs(OUT, exist_ok=True)
    items, classes = [], []
    for wiki_dir in ("rotfserver", "revenge-of-the-fallen-rotmg"):
        for path in sorted(glob.glob(os.path.join(WIKI, wiki_dir, "*.wiki"))):
            page = os.path.splitext(os.path.basename(path))[0]
            text = open(path, encoding="utf-8", errors="replace").read()
            for body in find_templates(text, "ItemInfo"):
                t = parse_template(body)
                t["_page"] = page; t["_wiki"] = wiki_dir
                items.append(t)
            for body in find_templates(text, "Classbox"):
                c = parse_template(body)
                c["_page"] = page
                classes.append(c)
    json.dump(items, open(os.path.join(OUT, "items.json"), "w"), indent=1)
    json.dump(classes, open(os.path.join(OUT, "classes.json"), "w"), indent=1)

    # summary
    from collections import Counter
    slots = Counter(i.get("slot", "?").lower() for i in items)
    tiers = Counter(i.get("tier", "?") for i in items)
    print(f"parsed {len(items)} items, {len(classes)} classes")
    print("slots:", dict(slots.most_common()))
    print("tiers:", dict(tiers.most_common()))
    fields = Counter(k for i in items for k in i if not k.startswith("_"))
    print("field coverage:", dict(fields.most_common(20)))
    miss = [i.get("_page") for i in items if not i.get("name")]
    if miss:
        print("WARN items without name:", miss[:10])


if __name__ == "__main__":
    main()
