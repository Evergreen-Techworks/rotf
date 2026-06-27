#!/usr/bin/env python3
"""Sweep Wayback (with backoff) for ALL text-based ROTF sources we haven't fully mined."""
import urllib.request, urllib.parse, time, sys

UA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"

def cdx(urlpat, fl="original", extra="&collapse=urlkey&limit=80"):
    """Query Wayback CDX with retry/backoff; return list of lines."""
    api = f"http://web.archive.org/cdx/search/cdx?url={urllib.parse.quote(urlpat, safe='*')}&output=text&fl={fl}{extra}"
    for attempt in range(5):
        try:
            req = urllib.request.Request(api, headers={"User-Agent": UA})
            data = urllib.request.urlopen(req, timeout=40).read().decode("utf-8", "replace")
            if "503" in data[:120] or "Service Unavailable" in data[:200]:
                raise RuntimeError("503")
            return [l for l in data.splitlines() if l.strip()]
        except Exception as e:
            time.sleep(4 * (attempt + 1))
    return [f"(failed after retries: {urlpat})"]

TARGETS = [
    ("rotf.io — ALL archived URLs", "rotf.io*", "timestamp,original,mimetype,statuscode", "&collapse=urlkey&limit=120"),
    ("aced.gg — ALL pages (find non-blog: item db, classes, about)", "aced.gg*", "original", "&collapse=urlkey&limit=400"),
    ("drips.pw — ROTF private-server assets?", "drips.pw*", "original", "&collapse=urlkey&limit=200"),
    ("realmstock / rotmg ps asset archives", "realmstock.com*", "original", "&collapse=urlkey&limit=80"),
    ("Reddit r/RotMGPrivateServers", "reddit.com/r/RotMGPrivateServers*", "original", "&collapse=urlkey&limit=120"),
]

for title, pat, fl, extra in TARGETS:
    print(f"\n########## {title}")
    lines = cdx(pat, fl, extra)
    # for the big ones, filter to ROTF-relevant
    if pat.startswith(("aced.gg", "reddit", "drips", "realmstock")):
        kw = ("fallen", "rotf", "underworld", "decaying", "craig", "showcase", "broken", "asgard",
              "dungeon", "riverborn", "darkest", "nyx", "rune", "galactic", "item", "drop", "class", "boss")
        flt = [l for l in lines if any(k in l.lower() for k in kw)]
        for l in flt[:60]:
            print(" ", l)
        print(f"  [{len(flt)} relevant / {len(lines)} total]")
    else:
        for l in lines[:120]:
            print(" ", l)
        print(f"  [{len(lines)} entries]")
    time.sleep(3)

print("\n=== sweep done ===")
