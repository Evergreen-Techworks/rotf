# ROTF Dungeon & Boss Master Plan (from rotfserver.fandom.com)

Source: `docs/rotf-wiki/` (Dungeons, Galactic Zones, Enemies, Realm Power pages + the
`dropsFrom` field on 70 items in `docs/rotf-wiki/parsed/items.json`). The wiki NAMES the
dungeons and (via item drops) most bosses, but gives almost NO per-boss stats — so boss
HP/phases/shot patterns are designed from theme + drop tier + the Epic-enemy framework.

Build pipeline per dungeon: `.jm` map (`scripts/maps/make_jm.py`) + `.jw` + Portal object
(0x82xx) + `BehaviorDb.<Name>.cs` boss AI + loot `Threshold`. See `docs/SPRITE_SHEETS.md`
for art and the content-pipeline memory for the recipe.

Legend: ✅ built · 🟡 partial · ⬜ todo

---

## A. ROTF-EXCLUSIVE DUNGEONS (8)

| # | Dungeon | Bosses (from wiki/drops) | Signature loot | Status |
|---|---------|--------------------------|----------------|--------|
| 1 | **Illusion** | The Illusionist (+ Illusion Mirages) | Blade of the Fallen Sky, Asura, Bel's Decapitator* | ✅ map+portal+boss+loot |
| 2 | **The Showcase** | (unnamed — design a showcase/gauntlet boss) | (tbd) | ⬜ |
| 3 | **Galaxy / Galactic Zones** | 4 tiers × 5 rotating bosses: Arcanica, Ortar, L. Bandito, Cracked Core, The Zucc, + GL bosses | Galactic Essence, Starbuster Blade, Handcannon, Wand Full of Mana, Hornshot, Deepspace Veil, Flask of Stardust, Greeni, Celestial Caduceus, Starlight Crook | ⬜ (Phase D3 — needs Spaceship/Fuel/tiers) |
| 4 | **Asgard** | Odin, Loki, Hela, Heimdallr, Thusala | Gungnir, Loki's Dagger, Hela's Power, Enchanted Uru Sword, Thusala's Slasher | ⬜ |
| 5 | **Tomb of Decaying Death** ("Todd's") | Todd + decay minions | Rotting Arm | ⬜ |
| 6 | **Twilight Necropolis** | Necropolis bosses, "Necro Doggo", Diagon | Wand of Retribution, Claw of the Beast, Wand of Bone | ⬜ |
| 7 | **Icy Peaks** | Ice Queen / Frozen Queen | Ice Shatter, Wand of the Frozen Queen, Asura, Fang of Frost | ⬜ |
| 8 | **Scorched Plains** ("Fiery Planes") | Firebreather + 2nd boss + Blazetalon | Firebreather's Tail, Head of the Firebreather, The Searing Blood, Flameblast Staff, Blazetalon | ⬜ |

\* Illusion's loot is provisional (assigned during the vertical slice); re-theme during balance.

### A2. Referenced-only dungeons (real, but NOT on the master Dungeons page — found via item drops)
The wiki is sparse: these have no detail page at all, only a passing mention. Confirmed real and
to be built like the rest (design from the drop + name).
| Dungeon | Evidence | Signature loot | Status |
|---|---|---|---|
| **Craig's Castle** | Shields.wiki: "drops in the Craig's Castle dungeon" | Shield of Vendettas | ⬜ |
| **Broken Forest** | item prose: "found in the Broken Forest" | (tbd) | ⬜ |

> NOTE: the mirror is COMPLETE (142/142 live pages fetched). Most ROTF **bosses are red-links**
> on the wiki (Archdemon Malphas, Cracked Core, Firebreather, L. Bandito, Pokerface, Shadowscale,
> Thessal, The Zucc, Odin/Thor via armor sets, Nikao the Defiler, Uvuvwevwe, Diagon, Elithor,
> Novus, Riverborn, Thusala…) — named but never statted. Encounters are reconstructed, not copied.

## B. REALM-EVENT BOSSES (spawn in the Realm, not a dungeon)

| Boss | Signature loot | Status |
|------|----------------|--------|
| **Realm Giant** (Realm Power system) | Realm Power (UT wand) | ⬜ (Phase D7) |
| **Shadowscale** | Shadow Scales, Shadowscale's Horn, Shadowbone Sword | ⬜ |
| **Riverborn** | The Winged Wand, The Seaweed Sword | ⬜ |
| **Thessal the Mermaid Goddess** | Cloak of the 1000 Oceans, Dagger of Goddess of Goddesses, Radiant Wand of Royalism, Sword of the Aquatic God | ⬜ |
| **Asgard Guardian** | Heavy Bow of the Asgard Guardian | ⬜ |
| **God of Reptilians** | Staff of Nature | ⬜ |
| **Staff of Annihilation event** (4 pieces across Realm) | Staff of Annihilation | ⬜ |

## C. EPIC ENEMY TIERS (the modifier system — Phase D1, gates the Rune economy)

- **Epic Gods** (buffed Godlands: Epic Brain/Medusa/Ent/Septavius/White Demon/Beholder/Djinn) — extra dmg + debuffs (stun/paralyze); **drop Illusions**.
- **Epic Elite Gods** ("eEgods": Medusa/Brain/Ent/Beholder) — bigger, more dmg; **drop Nodestones + Special Crates**.
- **Epic Bosses** — more shots/dmg/better loot than normal counterparts.
- **Epic midland/lowland** — buffed low mobs; chance to **drop Special Crates**.
- Rule of thumb (wiki): *almost any enemy can roll epic.*

## D. VANILLA DUNGEONS carrying ROTF loot (already in engine — just add drop-table entries)

Abyss of Demons → Sword of the Demon Lord, Destructor Cloak (Archdemon Malphas → Destructor
Dagger) · Undead Lair → Shattered War Axe · Sprite World → Dagger of the Endless Void, Fang of
Frost, Wand of the Rising Star · Cave of a Thousand Treasures → Sword of the Fallen · Tomb of
the Ancients → Blade of the Fallen Sky · The Shatters → Omega Ancient Sword · Oryx 3 → Sword of
Oryx · Candy Land (Cookie Monster) → Sweet Sugar Rush Staff + cookie traps.

## E. GALACTIC ZONES detail (Phase D3)

Access: Spaceship in Vault (or `/summon`), 8/8 char required (or party-summoned). Tiers & Fuel:
**T1 Sungravel** 500 · **T2 Splinterspire** 2000 · **T3 Flamefeather** 5000 · **T4 Frostwood**
10000. Each tier = 5 bosses spawning in random order, infinitely, each kill spawns the next.
Higher tier = more HP/dmg + better Galactic-item/Essence rates. **Galactic Essence**: consumable,
8/8 to use, grants 1 Skilltree point (max 82/char).

---

### Build order (dependency-sequenced)
1. ✅ Illusion (vertical slice — done)
2. Epic Enemies (D1) — unlocks Illusions/Nodestones drops → Runes economy
3. Runes + Nodestones (D2)
4. The 5 remaining static dungeons (Asgard, Tomb of Decaying Death, Twilight Necropolis, Icy
   Peaks, Scorched Plains) + The Showcase — reuse the Illusion recipe; one BehaviorDb per dungeon
5. Realm-event bosses (incl. Realm Giant / Realm Power)
6. Galactic Zones (D3) + Skilltree/Essence (D4) + Legion (D5) + Auction House (D6)
7. ROTF-loot drop-table entries into the vanilla dungeons (cheap, do alongside)
