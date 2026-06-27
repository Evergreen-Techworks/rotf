# Ordinary Client / ROTF — Full Implementation Plan

Goal: implement the **entire ROTF game** on top of the working `realm-src` engine,
using **`rotfserver.fandom.com`** (mirrored at `docs/rotf-wiki/`) as the content source
of truth. This plan is grounded in the verified engine content model and the actual wiki
inventory — not aspirational.

> Scope reality: this is a multi-month build. The hard part is not the engine (done) — it's
> (a) the *volume* of content (hundreds of items, dozens of enemies/bosses/dungeons) and
> (b) ROTF's *custom systems* (Runes, Galactic Zones, Skilltree, Legion, Auction House,
> Epic enemies), which require real C# engine work. The plan front-loads **tooling** so the
> high-volume content work is semi-automated, and sequences a **playable vertical slice**
> before scaling.

---

## 0. Where we are (done)

- **Server** (`server/`, .NET 8): app server (accounts/HTTP) + world server (TCP 2050) build
  and run on Linux; redis; loads 214 tiles / 960 items / 1593 objects; account register+login
  verified.
- **Client** (`client/`): builds to `bin-debug/WebMain.swf` (`build-client.bat` on Windows).
- **Content corpus**: all 158 wiki pages in `docs/rotf-wiki/` (`scripts/pull-rotf-wiki.py`).
- **Art**: 78 ROTF sprite atlases staged in `assets/sprites/` (not yet imported).
- Not yet done: client↔server connect; any ROTF content; any custom systems; web launcher; AWS.

---

## 1. The content model (how wiki → engine; the backbone of all content work)

Every piece of content touches a known set of files. **Server XML and client XML must stay
byte-aligned on object `type` IDs**, and the client must own the matching sprite pixels.

| Content type | Server (authoritative) | Client (render) | AI / logic |
|---|---|---|---|
| **Item** | `<Object>` in `server/common/resources/xml/EmbeddedData_EquipCXML.xml` (stats, `<Activate>`, projectile) | same `<Object>` in client equip XML + `<Texture><File>+<Index>` → a sprite sheet | none |
| **Enemy/Boss** | `<Object>` in a dungeon/monster `EmbeddedData_*CXML.xml` (HP, def, `<Projectile>`) | same + texture | **C# behavior** in `server/wServer/logic/behaviors/` (composed from ~96 primitives) wired in a `BehaviorDb` set |
| **Dungeon** | static map `.jw`/`.jm` in `server/common/resources/worlds/` **or** a procedural `DungeonGen/Templates/*` ; a portal object; loot tables | tiles/objects textures | spawn/portal logic; boss behaviors |
| **Class** | `<Object>` in `EmbeddedData_PlayersCXML.xml` (14 already = ROTF's 14) | texture + UI | starting gear, stat caps |
| **Tile/Ground** | `EmbeddedData_GroundCXML.xml` | texture | — |
| **Projectile** | `EmbeddedData_ProjectilesCXML.xml` | texture | — |

**Sprite registration (client):** each sheet = an `EmbeddedAssets_<name>Embed_.as` + entry in
`EmbeddedAssets.as`/`AssetsConfig.as` with its cell size (8×8/16×16/32×32/64×64). An object's
`<Texture><File>=<sheetName>` + `<Index>=<cell>` resolves to the pixels.

**Vanilla baseline already present:** 446 items, 56 monsters, ~20 dungeon data files, 14
classes, Nexus/Realm/Oryx/Vault/etc. worlds. ROTF work = *re-skin + extend + add custom*, not
start-from-zero.

---

## 2. The wiki as source of truth (and why it's automatable)

- Item pages use a structured **`{{ItemInfo|name=|slot=|tier=|minDmg=|maxDmg=|shots=|rof=|range=|fameb=|effects=|dropsFrom=|...}}`** template → parseable directly into `<Object>` equip XML.
- Enemy/dungeon pages are more prose; stats often partial → back-fill from vanilla `objects.xml`
  + YouTube footage. Expect manual judgement for boss HP/phases.
- Systems pages (Runes, Galactic Zones, Legion, Realm Power, Auction House) define **mechanics**
  → drive the C# custom-systems work in §6.
- Coverage is finite and known: ~26 item categories, ~90 named item/boss pages, 14 classes, ~8
  dungeons/zones, ~7 custom systems. We can track 100% coverage against `docs/rotf-wiki/INDEX.md`.

---

## 3. Tooling to build first (Phase A) — makes the volume tractable

1. **Wiki parser** (`scripts/wiki/parse_items.py`): read `docs/rotf-wiki/rotfserver/*.wiki`,
   extract every `{{ItemInfo}}`/`{{EnemyInfo}}` template into normalized JSON
   (`docs/rotf-wiki/parsed/items.json`, `enemies.json`).
2. **XML generator** (`scripts/wiki/gen_equip_xml.py`): map parsed item JSON → `<Object>` equip
   XML fragments (ROTF stat → engine field mapping table), assigning fresh `type` IDs from a
   reserved ROTF range (e.g. `0x9000+`) tracked in `docs/rotf-wiki/typemap.json` to avoid
   collisions with vanilla. Writes to **both** server and client equip XML.
3. **Sprite importer** (`scripts/sprites/import_sheet.py`): given a sheet from `assets/sprites/`,
   emit the client `EmbeddedAssets_<name>Embed_.as` + registration and a `<Texture>` stub, and
   slice/preview cells so we can pick `<Index>` values. Also a **sheet identifier** that compares
   our ripped sheets (by cell size + content) to vanilla sheet names.
4. **Coverage tracker** (`scripts/wiki/coverage.py`): cross-references `INDEX.md` ↔ what's been
   authored into XML/behaviors → a checklist of what's left. Single source of "are we done."
5. **Validation** (`scripts/validate_xml.py`): server↔client `type` ID alignment, texture
   File+Index existence, no duplicate IDs. Run in CI.

> Tooling first is deliberate: hand-authoring ~90 items + their client mirror is error-prone;
> a parser + generator + validator turns it into review-and-tune.

---

## 4. Phasing & milestones

**Phase A — Foundations (tooling + connect)**
- A1. Client↔server connect: point client at server, fix the port-843 socket-policy, log in and
  walk around *vanilla* (proves the full loop before re-skinning). *(also the current ROADMAP next step)*
- A2. Build the §3 tooling.
- A3. Import the ROTF sprite atlases into the client (`EmbeddedAssets`), identify/name sheets.
- ✅ Milestone: walk around in a vanilla world rendered with ROTF sprites loaded.

**Phase B — Vertical slice (one of everything, end-to-end)**
- Pick the simplest ROTF-exclusive dungeon (**Illusion** or **The Showcase**): author its boss
  (`<Object>` + a composed behavior), 1–2 enemies, its map (`.jw` or a `DungeonGen` template),
  a portal, a loot table, and 2–3 of its drop items (via the parser), all with ROTF sprites.
- ✅ Milestone: enter the dungeon from the Nexus/Realm, fight the boss, get a ROTF item drop.
  This validates the *entire* content pipeline before scaling.

**Phase C — Content scale-out (data-heavy, parallelizable)**
- C1. **Items**: parse all `{{ItemInfo}}` pages → generate equip XML for the ~90 named items +
  category fills; assign sprites; validate. Tune outliers by hand.
- C2. **Enemies/bosses**: author behaviors for each ROTF boss/enemy (compose from primitives;
  custom C# for unique mechanics). Epic Gods set.
- C3. **Dungeons/worlds**: build remaining dungeons (Asgard, Tomb of Decaying Death, Twilight
  Necropolis, Icy Peaks, Scorched Plains) as maps/templates + portals + loot.
- C4. **Classes**: apply ROTF starting-gear/stat-cap tweaks to the 14 classes.

**Phase D — Custom systems (the hard C# work — see §6)**
- Runes/Nodestones → Galactic Zones + Fuel + Galactic Essence + Skilltree → Legion → Auction
  House → Epic-enemy modifier → Realm Power/Realm Giant. Sequence by dependency.

**Phase E — Rebrand, balance, polish**
- Rebrand client/server strings, logos, menus to "Ordinary Client / ROTF". Balance pass against
  wiki + footage. UI for runes/skilltree/legion/market. Sound.

**Phase F — Web launcher + AWS deploy (mirror egtw)**
- §7 + §8.

---

## 5. Content workstreams (per-type recipes)

- **Items** — parser → equip XML (server+client) → sprite `<Texture>` → loot-table entry →
  validate. Mostly automated; balance by hand.
- **Enemies/Bosses** — `<Object>` stats → C# behavior (movement/shoot/phase) in
  `wServer/logic/behaviors/` → register in a `BehaviorDb` set → loot table. Custom mechanics
  (phases, minion waves, invulnerability windows) = bespoke C#.
- **Dungeons** — map authoring (static `.jw` via a map editor, or `DungeonGen` template) →
  portal object + spawn rules → enemy/boss placement → loot. Realm/Nexus integration.
- **Classes** — `PlayersCXML` tweaks; starting equipment sets.
- **Sprites/Tiles** — import sheet → register → assign File+Index across the above.

---

## 6. ROTF custom systems (engine features — biggest lift, C# in `wServer`)

Ordered by dependency. Each needs server logic + client UI + data.

1. **Epic enemies** — a modifier applied to spawns: buffed stats + special drop table
   (Illusions/Nodestones/Crates). Hook into enemy spawn + loot. *(unlocks Runes economy)*
2. **Runes + Nodestones** — rune items (small=+3 stat; big=trade-off effects from `Runes.wiki`),
   dropped by nodestones (from Epic enemies/Galactic). A **rune-effect engine**: apply
   stat/regen/damage/debuff-immunity modifiers to a player while equipped. ~25 big-rune effects
   to encode.
3. **Galactic Zones** — 4 tier worlds (Sungravel/Splinterspire/Flamefeather/Frostwood), each with
   5 bosses spawning in random infinite rotation (= ~20 bosses + behaviors); **Spaceship** object
   in the Vault with a tier-select + `/summon`; **Fuel** currency (cost per tier); drop tables.
4. **Galactic Essence + Skilltree** — Essence consumable (8/8 gate) → grants 1 of 82 skill points;
   a per-character **Skilltree** (data model + UI + effect application).
5. **Legion** — track cumulative maxed stats across a player's alive characters → ranks 0–5 →
   stacking HP/MP/pot/loot/all-stat bonuses; show rank in char-select.
6. **Auction House / Market** — list items for fame (48h, then gift chest), `/market` + Nexus
   portal; redis-backed listings.
7. **Realm Power / Realm Giant** — a custom Realm boss dropping the Realm Power wand (UT).

> Several of these (party system, vault, gift chests, loot tables, currencies) have partial
> hooks in the NR-Core base — audit before building. Where the base has it, extend; else add.

---

## 7. Web launcher + accounts (egtw-style, `web/`)

A small **Next.js** site (mirrors `../egtw-main` exactly so deploy is familiar): client download
(SWF + Flash Projector + one-click launcher), account register/login (talks to the app server),
news, leaderboards. SQL only if we add site-only features; **game data stays in redis**.

---

## 8. AWS deployment (mirror egtw-main)

EC2 Ubuntu 22.04 → systemd services: `rotf-appserver` (HTTP behind nginx), `rotf-worldserver`
(TCP 2050), `rotf-policyserver` (843, needs `CAP_NET_BIND_SERVICE`), **redis** on-box (or
ElastiCache). nginx + certbot for the launcher/account HTTP. Security group: 22 (you), 80/443
(web), 2050 + 843 (game). GitHub Actions deploy via SSH/rsync → `Evergreen-Techworks/rotf`.
(Detail already in `ROADMAP.md` Phase 3.)

---

## 9. Verification & testing

- **XML/asset validation** (§3.5) in CI on every content change.
- **Server boot smoke test** after each content batch (loads without behavior/loot errors —
  watch for `Xml data not found` / `Object type ... not found`).
- **Headless protocol test** via `nrelay` (TS bot client) — log in, enter dungeon, confirm
  packets, without the GUI.
- **In-client visual** via the Flash Projector for sprites/UI/feel.
- **Per-system functional tests** (rune effects apply, fuel deducts, essence→skillpoint, legion
  rank math, auction lifecycle).

---

## 10. Risks & open decisions

- **Missing hard stats**: wiki is often descriptive, not exact (boss HP/phases). Mitigate with
  vanilla baselines + footage; accept approximations, tune later.
- **Sprite name mapping**: ripped sheets lost their names; identification is manual-ish (cell
  size + visual match). The contact sheet + importer help.
- **Custom-systems complexity**: §6 is the bulk of the engineering risk (Galactic Zones +
  Skilltree especially). Consider an MVP subset first (Runes + Legion + one Galactic tier).
- **C# behavior authoring**: each unique boss = real code; budget time per boss.
- **Balance/faithfulness**: decide per-system whether to match ROTF exactly or approximate.
- **Legal/IP**: RotMG/Myriad/Adobe assets are third-party; non-commercial fan project.

**DECISION (locked 2026-05-31): FULL FAITHFUL BUILD.** Implement the entire game to match the
wiki — all content + all custom systems. Execute the phases in order, autonomously, to completion.
Client-visual checks and AWS deploy are the only steps gated on Jesse (Projector on Windows; AWS creds).

---

## 11. Immediate next actions

1. Phase A1 — get the client connecting to the server (walk around vanilla).
2. Build `scripts/wiki/parse_items.py` + run it → `items.json` (proves automation).
3. Pick the Phase-B vertical-slice dungeon and author it end-to-end.
