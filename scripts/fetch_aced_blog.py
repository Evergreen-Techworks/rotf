#!/usr/bin/env python3
"""Archive the aced.gg ROTF dev blog from the Wayback Machine (text only)."""
import urllib.request, re, html, time, os, sys

UA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"
OUT = "docs/aced-blog"
os.makedirs(OUT, exist_ok=True)

POSTS = [
 ("2020/07/13/revenge-of-the-fallen-the-realm-gameplay-preview", "realm-gameplay-preview"),
 ("2020/09/07/revenge-of-the-fallen-jellies-potions-and-more-potions", "jellies-potions"),
 ("2020/12/19/revenge-of-the-fallen-market-parties-party-in-the-market", "market-parties"),
 ("2020/12/26/revenge-of-the-fallen-spellbooks-scrolls-mages-and-summoners", "spellbooks-mages-summoners"),
 ("2021/01/02/revenge-of-the-fallen-gate-to-the-underworld-biome-showcase", "gate-underworld-biome"),
 ("2021/01/10/revenge-of-the-fallen-demon-lady-world-boss-design", "demon-lady-design"),
 ("2021/01/17/revenge-of-the-fallen-nyx-of-fire-that-demon-lady-from-last-week", "nyx-of-fire"),
 ("2021/01/31/revenge-of-the-fallen-thor-and-heimdallr-asgard-dungeon-preview", "asgard-thor-heimdall"),
 ("2021/02/06/revenge-of-the-fallen-loki-hela-asgard-dungeon-preview-continued", "asgard-loki-hela"),
 ("2021/02/15/revenge-of-the-fallen-runes-the-new-skill-tree", "runes-skill-tree"),
 ("2021/03/14/revenge-of-the-fallen-masterwork-runes-the-new-nexus", "masterwork-runes-nexus"),
 ("2021/03/21/revenge-of-the-fallen-the-vault-and-the-fruit-stand", "vault-fruit-stand"),
 ("2021/03/28/revenge-of-the-fallen-dragons-midgame-content-more", "dragons-midgame"),
 ("2021/04/11/revenge-of-the-fallen-gate-to-the-underworld-sia-and-hu-eternal-companions", "gate-underworld-sia-hu"),
 ("2021/04/17/revenge-of-the-fallen-the-riverborn-riverside-refuge-preview", "riverborn-riverside"),
 ("2021/04/26/revenge-of-the-fallen-realm-rework-progression-overhaul", "realm-rework"),
 ("2021/05/02/revenge-of-the-fallen-primal-effects-but-good-this-time", "primal-effects"),
 ("2021/05/17/revenge-of-the-fallen-legendary-effects-preview", "legendary-effects"),
 ("2021/05/30/revenge-of-the-fallen-firing-laz0rs", "firing-laz0rs"),
]

def get(url):
    req = urllib.request.Request(url, headers={"User-Agent": UA})
    return urllib.request.urlopen(req, timeout=30).read().decode("utf-8", "replace")

def clean(t):
    m = re.search(r'entry-content.*?</div>\s*<(?:footer|/article)', t, re.S) or re.search(r'<article.*?</article>', t, re.S)
    b = m.group(0) if m else t
    b = re.sub(r'<(script|style).*?</\1>', '', b, flags=re.S)
    b = re.sub(r'<[^>]+>', ' ', b)
    b = html.unescape(b)
    b = re.sub(r'[ \t]+', ' ', b)
    b = re.sub(r'\n\s*\n+', '\n\n', b)
    return b.strip()[:7000]

saved = 0
for path, out in POSTS:
    fp = f"{OUT}/{out}.txt"
    if os.path.exists(fp):
        print("skip", out); continue
    page = f"aced.gg/{path}/"
    try:
        cdx = get(f"http://web.archive.org/cdx/search/cdx?url={page}&output=text&fl=timestamp&limit=1")
        ts = cdx.strip().split("\n")[0].strip()
        if not ts:
            print("NO SNAP", out); time.sleep(1); continue
        raw = get(f"http://web.archive.org/web/{ts}id_/http://{page}")
        txt = clean(raw)
        with open(fp, "w") as f:
            f.write(txt)
        print(f"saved {out} ({len(txt.split())}w) [snap {ts}]")
        saved += 1
    except Exception as e:
        print("ERR", out, str(e)[:80])
    time.sleep(1)

print("=== newly saved:", saved, "| total:", len([f for f in os.listdir(OUT) if f.endswith('.txt')]))
