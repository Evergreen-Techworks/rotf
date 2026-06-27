#!/usr/bin/env python3
"""
Loop A: Wiki Loot Mining
Parse rotfserver wiki pages for dropsFrom data, map to boss ledger, update loot_status.
"""
import os, re, json

WIKI_DIR = '/home/jesse/rotf/docs/rotf-wiki/rotfserver'
LEDGER   = '/home/jesse/rotf/docs/mechanics_recovery.json'

# Normalize boss name for fuzzy matching
def norm(s):
    return re.sub(r'[^a-z0-9]', '', s.lower())

# Map wiki drop-source strings → ledger boss names
# Hand-curated aliases where wiki names differ from our boss names
ALIASES = {
    'gravedigger':           'Gravedigger',
    'thegravedigger':        'Gravedigger',
    'loki':                  'Loki',
    'odin':                  'Odin',
    'hela':                  'Hela',
    'heimdallr':             'Heimdall',
    'heimdall':              'Heimdall',
    'icequeeninicypeaks':    'Ice Queen',
    'icequeenin':            'Ice Queen',
    'theicequeenin':         'Ice Queen',
    'icequeenin icypeaks':   'Ice Queen',
    'loki':                  'Loki',
    'firebreather':          'Firebreather',
    'thefirebreather':       'Firebreather',
    'archdemonmalphas':      'Archdemon Malphas',
    'archdemon malphas':     'Archdemon Malphas',
    'shadowscale':           'Shadowscale',
    'arcanica':              'Arcanica',
    'diagon':                'Diagon',
    'trollmatriarch':        'Troll Matriarch',
    'godofdreptilians':      'God of Reptilians',
    'godofthereptilians':    'God of Reptilians',
    'thegodofthereptilians': 'God of Reptilians',
    'godofrepilians':        'God of Reptilians',
    'godofreptilians':       'God of Reptilians',
    'riverborn':             'Riverborn',
    'uvuvwevwevwe':          'Uvuvwuwuwe',
    'uvuvwewevwe':           'Uvuvwuwuwe',
    'suspiciouslookingdragon': 'Suspicious Looking Dragon',
    'novus':                 'Shade of Novus',
    'shadeofnovus':          'Shade of Novus',
    'crackedcore':           'Cracked Core',
    'thezucc':               'The Zuck',
    'zucc':                  'The Zuck',
    'zuck':                  'The Zuck',
    'ortar':                 'Ortar',
    'gemgem':                'Gem Gem',
    'stonetaker':            'Stonetaker',
    'stoneraker':            'Stonetaker',
    'cookie monster':        'Cookie Monster',
    'cookiemonster':         'Cookie Monster',
    'oryx puppet':           'Oryx Puppet',
    'thepuppetmaster':       'The Puppet Master',
    'mewtwo':                'Mewtwo',
    'kestora':               'Kestora the Lava God',
    'nikad':                 'Nikad the Defiler',
    'melphas':               'Archdemon Malphas',
    'crystalprisoner':       'Crystal Prisoner',
    'craig':                 'Craig the Mad Intern',
    'marblecolossus':        'Marble Colossus',
    'puppetmaster':          'The Puppet Master',
    'urios':                 'Urios, God of Elements',
    'helios':                'Helios',
    'legon':                 'Legon the Weather God',
    'euryale':               'Euryale the Snake Goddess',
    'lycaon':                'Lycaon',
}

# Also build ledger name → norm map for direct matching
def load_ledger():
    with open(LEDGER) as f:
        return json.load(f)

def parse_wiki_drops():
    """Return {item_name: [raw_drop_source, ...]}"""
    drops = {}
    for fn in sorted(os.listdir(WIKI_DIR)):
        if not fn.endswith('.wiki'):
            continue
        with open(os.path.join(WIKI_DIR, fn)) as f:
            content = f.read()
        item_match = re.search(r'name\s*=\s*([^|}\n]+)', content)
        item_name = item_match.group(1).strip() if item_match else fn.replace('.wiki', '')
        drop_matches = re.findall(r'dropsFrom\s*=\s*([^|}\n]+)', content)
        for d in drop_matches:
            d = d.strip()
            if d and d not in ('no idea', 'idk', '', 'zucc', 'e brains'):
                drops.setdefault(item_name, []).append(d)
    return drops

def resolve_boss(drop_str, ledger_bosses):
    """Try to resolve a dropsFrom string to a ledger boss name."""
    # Strip wiki markup [[...]] and extract text
    clean = re.sub(r'\[\[([^\]|]+)(?:\|[^\]]*)?\]\]', r'\1', drop_str)
    # Remove common prefixes
    clean = re.sub(r'^(drops from|can be found in a white bag dropped by|'
                   r'is a white bag drop from|is dropped in a white bag after killing|'
                   r'can be found in white bags dropped by)[\s:]*', '', clean, flags=re.I)
    clean = re.sub(r'[,.].*$', '', clean)  # take only up to first comma/period
    clean = clean.strip().rstrip('.')

    key = norm(clean)
    if key in ALIASES:
        return ALIASES[key]

    # Try direct substring match on norm'd ledger names
    ledger_norm = {norm(b): b for b in ledger_bosses}
    if key in ledger_norm:
        return ledger_norm[key]

    # Try if any ledger boss norm is a substring of key or vice versa
    for nk, boss in ledger_norm.items():
        if nk and (nk in key or key in nk) and len(key) >= 4:
            return boss

    return None

def main():
    data = load_ledger()
    bosses = data['bosses']
    boss_names = [b['boss'] for b in bosses]

    wiki_drops = parse_wiki_drops()

    # Build boss → items mapping
    boss_items = {}  # boss_name → [item_name, ...]
    unresolved = []

    for item, drop_list in wiki_drops.items():
        for drop in drop_list:
            boss = resolve_boss(drop, boss_names)
            if boss:
                boss_items.setdefault(boss, []).append(item)
            else:
                unresolved.append((item, drop))

    print("=== BOSS → WIKI DROPS ===")
    for boss, items in sorted(boss_items.items()):
        print(f"  {boss}: {items}")

    print("\n=== UNRESOLVED dropsFrom strings ===")
    for item, drop in sorted(unresolved):
        print(f"  '{drop}' (from item: {item})")

    # Update ledger
    updated = []
    for b in bosses:
        name = b['boss']
        if name in boss_items:
            items = boss_items[name]
            old_status = b.get('loot_status', 'designed')
            if old_status in ('designed', 'none-observed'):
                b['loot_status'] = 'observed'
                updated.append(name)
            existing_note = b.get('loot_wiki_drops', [])
            # Merge without duplicates
            merged = list(dict.fromkeys(existing_note + items))
            b['loot_wiki_drops'] = merged

    # Recount summary loot
    loot_counts = {}
    for b in bosses:
        s = b.get('loot_status', 'designed')
        loot_counts[s] = loot_counts.get(s, 0) + 1

    print(f"\n=== UPDATED {len(updated)} bosses → loot_status=observed ===")
    for name in updated:
        print(f"  {name}")

    print(f"\n=== LOOT SUMMARY AFTER UPDATE ===")
    for k, v in sorted(loot_counts.items()):
        print(f"  {k}: {v}")

    with open(LEDGER, 'w') as f:
        json.dump(data, f, indent=1)
    print("\nLedger written.")

if __name__ == '__main__':
    main()
