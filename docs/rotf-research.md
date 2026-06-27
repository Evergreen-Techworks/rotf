# ROTF ("Revenge of the Fallen") — Research & Reconstruction Notes

Research done 2026-05-31 (web research agent). This is the surviving record of what
ROTF was and where to reconstruct it from, since **no original ROTF source or asset
dump survives publicly** — remaking it = rebuild content from wikis/videos + the
ripped sprite set in `assets/sprites/`.

## What ROTF was

- A **Realm of the Mad God Flash-era private server** with heavy custom content:
  custom dungeons, bosses, items, classes, a reworked endgame (footage references an
  "Oryx 3" boss, custom NPE/tutorial, "runes"), and custom spritework.
- Ran roughly **2017–2018** (cluster of YouTube loot/gameplay uploads + MPGH threads).
  At least two community wikis exist → likely more than one relaunch under the name.
- A community pserver, not an official DECA product. Built on the standard **leaked C#
  server lineage** (almost certainly a fabiano/Nilly-derived source) + the AS3 Flash client.

## Does the source / assets survive? — No

No public source or asset dump found. Only secondary/archival material:
- Fandom wiki: https://revenge-of-the-fallen-rotmg.fandom.com/wiki/Revenge_of_the_Fallen_Rotmg_Wiki
- Fandom wiki #2: https://rotfserver.fandom.com/wiki/Revenge_of_the_Fallen_Wiki
- Wikidot wiki: http://revengeofthefallen.wikidot.com/
- MPGH threads (people asking for the client/source → never openly released):
  https://www.mpgh.net/forum/showthread.php?t=1369797 , https://www.mpgh.net/forum/showthread.php?t=1413253
- YouTube footage (best surviving record of actual content/sprites): e.g.
  https://www.youtube.com/watch?v=hMsGi7jJ-rc , https://www.youtube.com/watch?v=7lIEpNWWTsI

**Implication:** reconstruct ROTF's dungeons/items/bosses from the wikis + video, using
our ripped sprite-sheet PNGs (`assets/sprites/`) as the art base.

### Pulling wiki content (the HTML is bot-blocked, the API is not)

`*.fandom.com` HTML returns **403** to fetchers, but the **MediaWiki API is open**. The
full text of every page is mirrored locally in **`docs/rotf-wiki/`** (158 pages: 142 from
`rotfserver` + 16 from the older wiki) with an `INDEX.md`. Refresh with
`python3 scripts/pull-rotf-wiki.py`. Pull a single page live:
`https://rotfserver.fandom.com/api.php?action=parse&page=<Title>&prop=wikitext&format=json`
(send a browser User-Agent). Boss/content starting points: `Enemies`, `Dungeons`,
`Galactic Zones`, `Legion`, `Runes`, the `Update: *` patch notes, and the many named
item/boss pages. Also: a 2022 WikiTeam XML dump of the *older* wiki at
`archive.org/details/wiki-revenge_of_the_fallen_rotmgfandomcom`, and `aced.gg/tag/revenge-of-the-fallen/`.

## Why we picked realm-src (the base we forked)

ROTF descended from the C# server family **MMOE → Trapped → Phoenix → fabiano "Swagger of
Doom" → Nilly's Realm / NR-CORE**. We chose `moistosaurus/realm-src` (NR-CORE-based, the
exact lineage ROTF used) + its matching AS3 client `moistosaurus/realm-cli`. Other options:
- `ossimc82/fabiano-swagger-of-doom` (AGPL) — the classic, most ROTF-era servers descend from it
- `cp-nilly/NR-CORE` (AGPL, redis) — the continuation; realm-src is based on this
- `Zolmex/alloy-server` (MIT, more recently maintained; pairs with a native C# OpenTK client) — pivot if GPL/Flash becomes a blocker
- `itsEvil/evils-source-pub` (MIT, modern WIP rewrite)

## Client reality: Ruffle is not viable

Ruffle's AS3 (AVM2) support isn't complete enough to run the full RotMG client reliably. Pservers play
via the **standalone Adobe Flash Projector** (desktop exe), or via a buildable AS3 client
(what we do — see `client/build-client.bat`). No mature open-source HTML5/Unity RotMG client
exists. Buildable client refs: `Zolmex/RotMG-Flash-Client`, `moistosaurus/realm-cli` (ours).

## Asset pipeline (how ROTF sprites become game objects)

- Sprites are packed into **named sheet atlases** by cell size: `<name>Chars<NxN>` for
  creatures/players, `<name>Objects<NxN>` for tiles/items/projectiles, `...Mask` for the
  recolor layer. Cell sizes: **8×8** (small enemies/items/tiles/projectiles), **16×16**
  (most enemies/bosses), **32×32 / 64×64** (big bosses/effects). Char sheets are multi-frame
  strips (idle + walk + attack per facing).
- Client registers each sheet: `AssetLibrary.addImageSet("<sheet>", <png>, W, H)` (cell size).
  Sheets live in `client/src/kabam/rotmg/assets/EmbeddedAssets_*Embed_.png` (+ `EmbeddedAssets.as`,
  `AssetLoader.as`).
- An object's texture: in `objects.xml` / `EmbeddedData_*.xml`, `<Object><Texture><File>=sheet
  name, <Index>=cell number`. Full chain: **type id → `<Texture><File>+<Index>` → named sheet →
  cell in PNG → rendered sprite.** Server deals in type ids + stats; client owns the pixels.
- **Server XML and client XML/sheets must stay aligned on object type ids.**
- Tools: `MajorH5/realmspriter` (sprite editor), `Mystery3/RotMG-SpriteRenderer` (preview cell
  offsets), reference dumps at `https://static.drips.pw/rotmg/production/current/`.

## Our ROTF asset status

- **In repo:** `assets/sprites/raw/` (78 ripped atlases — names lost: `Nothing found _<hex>_<idx>`),
  `assets/sprites/named/` (index-renamed), `contact_sheet.png` (visual inventory),
  `rotf-sprites-original.zip` (preserved source). One oddball: a stray **face photo** mixed into
  the rip.
- **Pending (Phase 2):** identify each sheet (by cell size + content vs. vanilla sheets), import
  into the client's `EmbeddedAssets`, and wire to objects via `<Texture><File>+<Index>`. Plus
  re-author ROTF's custom dungeons/items/bosses as XML from the wikis above.
