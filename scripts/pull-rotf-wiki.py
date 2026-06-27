#!/usr/bin/env python3
"""Pull the ROTF Fandom wikis into docs/rotf-wiki/ as raw wikitext.

The wiki HTML is bot-blocked (HTTP 403), but the MediaWiki API is open, so we
enumerate every page via list=allpages and fetch each page's wikitext via
action=parse. Re-run any time to refresh:  python3 scripts/pull-rotf-wiki.py
"""
import urllib.request, urllib.parse, json, os, time, re

UA = ("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 "
      "(KHTML, like Gecko) Chrome/120 Safari/537.36")

WIKIS = {
    # THE COMPREHENSIVE one (500+ pages: every dungeon, boss, item, armor set, ring, system).
    # Found 2026-06-01 — far more complete than the others. This is the primary content source.
    "revengeofthefallengame": "https://revengeofthefallengame.fandom.com/api.php",
    # the mid one (142 pages: Enemies, Dungeons, Galactic Zones, bosses, items, classes...)
    "rotfserver": "https://rotfserver.fandom.com/api.php",
    # older/smaller one (16 pages, mostly equipment categories)
    "revenge-of-the-fallen-rotmg": "https://revenge-of-the-fallen-rotmg.fandom.com/api.php",
}
ROOT = os.path.join(os.path.dirname(__file__), "..", "docs", "rotf-wiki")


def api(base, params):
    url = base + "?" + urllib.parse.urlencode({**params, "format": "json"})
    req = urllib.request.Request(url, headers={"User-Agent": UA})
    with urllib.request.urlopen(req, timeout=30) as r:
        return json.load(r)


def allpages(base):
    titles, cont = [], None
    while True:
        p = {"action": "query", "list": "allpages", "aplimit": "500"}
        if cont:
            p["apcontinue"] = cont
        d = api(base, p)
        titles += [x["title"] for x in d["query"]["allpages"]]
        cont = d.get("continue", {}).get("apcontinue")
        if not cont:
            return titles


def wikitext(base, title):
    d = api(base, {"action": "parse", "page": title, "prop": "wikitext"})
    return d.get("parse", {}).get("wikitext", {}).get("*", "")


def sanitize(t):
    return (re.sub(r"[^A-Za-z0-9._ -]", "_", t).strip() or "untitled")[:120]


def main():
    idx = ["# ROTF Wiki — local dump (pulled via MediaWiki API)\n",
           "HTML pages are bot-blocked (403); the API is open. "
           "Refresh: `python3 scripts/pull-rotf-wiki.py`\n"]
    total = 0
    for name, base in WIKIS.items():
        d = os.path.join(ROOT, name)
        os.makedirs(d, exist_ok=True)
        titles = allpages(base)
        idx.append(f"\n## {name} — {len(titles)} pages\nSource: {base}\n")
        for t in sorted(titles):
            try:
                wt = wikitext(base, t)
            except Exception as e:
                wt = f"<<fetch error: {e}>>"
            fn = sanitize(t) + ".wiki"
            with open(os.path.join(d, fn), "w", encoding="utf-8") as f:
                f.write(f"# {t}\n# {base}?action=parse&page="
                        f"{urllib.parse.quote(t)}&prop=wikitext\n\n{wt}")
            idx.append(f"- [{t}]({name}/{fn})")
            total += 1
            time.sleep(0.12)
        print(f"{name}: {len(titles)} pages")
    with open(os.path.join(ROOT, "INDEX.md"), "w", encoding="utf-8") as f:
        f.write("\n".join(idx) + "\n")
    print("TOTAL pages:", total)


if __name__ == "__main__":
    main()
