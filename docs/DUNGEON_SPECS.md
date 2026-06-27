# ROTF Dungeon Reconstruction Specs

From the 16-agent research pass (real client data + 3 wikis + aced.gg + web). Fidelity varies — see each `confidence`. Built from these. See also `docs/DUNGEONS.md`, `assets/rotf-original-data/`.

## Illusion
Illusion is a mid-tier ROTF-exclusive dungeon (difficulty 3) dropped by Epic Beholders in the Godlands. It features The Illusionist as the main boss—a robed wizard (sprite rotfBoss:51) with 25,000 HP and moderate defense. The arena is a simple 44×44 grass field with the boss centered and 4 spawnable Illusion Mirage minions (white ghost sprites, rotfBoss:124-126) positioned around the fight zone. The boss has two phases: normal combat with 5-shot spirals and periodic minion spawns, escalating to an enraged state at 35% HP featuring orange flashing and 8-10 shot patterns. Signature white-bag drops (Blade of the Fallen Sky, Asura, Bel's Decapitator) are provisional loot assigned during vertical slice development and may be re-themed during balance. The dungeon is themed around illusion and deception, with the boss's taunt 'You cannot tell what is real!' reinforcing the mystical aesthetic.

**Map:** DOCUMENTED from Illusion.jm: 44×44 grass arena with perimeter walls (#). Boss (The Illusionist) centered. 6 minion spawn spots (7 tiles away, cardinal + diagonal positions): (-7,-5), (7,-5), (-7,5), (7,5), (-10,0), (10,0). South spawn strip for players (row y=40, x=[21-26]). Theme: Mystical/illusory (grass ground, simple layout). Difficulty: 3 (mid-tier). Portal (type 0x8200, Illusion Portal) referenced in .jw. Non-blocking, Teleport allowed, persistent: false.

### The Illusionist — 25000 HP, Def 15, tex rotfBoss:51
- Projectiles: Blade (Damage: 75, Speed: 100, LifetimeMS: 1400)
- Fight: DOCUMENTED from BehaviorDb.ROTF.cs — Idle state until player within 12 tiles. Fight state: Wander (35%), Shoot 5 projectiles (angle: 18°, cooldown: 800ms), Shoot 1 projectile (cooldown: 1600ms), Spawn Illusion Mirage (max 4, cooldown: 8000ms). Enrage at 35% HP: Orange flash (0xff8800ff, 3s duration × 10 flashes), Taunt, Wander increased (50%), Shoot 8 projectiles (angle: 12°, cooldown: 600ms), Shoot 10 projectiles (angle: 36°, fixed angle 0°, cooldown: 1200ms).
- Taunts: ['You cannot tell what is real!']

**Minions:** Illusion Mirage (HP: 1500, Def: 0, Size: 90, Texture: rotfBoss:124 — Projectile: Blade, Damage: 40, Speed: 90, LifetimeMS: 1200ms; Wander 40%, Shoot cooldown: 1000ms; spawned max 4 during fight)

**Drops:** Blade of the Fallen Sky (UT Sword, 8% drop rate — lofiObj5:0x30), Asura (UT Katana, 8% drop rate), Bel's Decapitator (UT Axe, 6% drop rate), Potions (2× Potion drops, 10% threshold)

**Confidence:** HIGH for documented elements: HP, Defense, Projectile stats, Fight phases (all extracted from real code and XML). Texture indices confirmed from SPRITE_SHEETS.md. Map layout reconstructed from .jm JSON but accurate to schema. Loot thresholds from code. Taunt line exact. Theme and narrative (Godlands drop, Beholder origin) confirmed via wiki + DUNGEONS.md. NOTE: Loot items marked provisional in DUNGEONS.md (may be re-themed during balance) but drop rates and item details are documented in BehaviorDb code.

---

## The Showcase
The Showcase is a ROTF-exclusive dungeon that exists in the game's wiki and asset pipeline, but was intentionally left unspecified in the development documentation. It is designated as a 'showcase/gauntlet boss' dungeon, implying an arena-style boss rush or exhibition fight, but no concrete boss name, HP, attacks, or drops were ever defined. The dungeon appears on the Dungeons page and has a portal asset (TheShowcasePortal.png) but contains no game mechanics details.

**Map:** UNKNOWN - map theme/size/structure not documented. No `.jw`/`.jm` map file or DungeonGen template found. Status marked 'todo' in development plan (DUNGEONS.md).

### Unnamed (showcase/gauntlet boss)
- Fight: RECONSTRUCTED: No documented fight pattern. Designated as a 'showcase/gauntlet boss' suggesting possible boss-rush or arena format, but mechanics entirely unspecified. Design placeholder: 'design a showcase/gauntlet boss' per DOTF DUNGEONS.md line 21.

**Confidence:** MINIMAL DOCUMENTATION. The Showcase exists in: (1) dungeon roster wiki stubs (revengeofthefallengame/The Showcase.wiki contains only 'asdf' placeholder); (2) Dungeons.wiki index listing with portal asset name (TheShowcasePortal.png); (3) DUNGEONS.md implementation plan marking it 'todo' with note '(unnamed — design a showcase/gauntlet boss)' and '(tbd)' drops. Zero boss stats, drops, minions, taunts, or fight mechanics are documented anywhere in: rotf_objects_FULL.xml, items.json, dungeon_roster.json, or aced.gg blog. The aced.gg Runes article cryptically references 'the boss of the legacy rotf Showcase dungeon' in a puzzle hint (first letter only, actual name redacted), but no identity revealed. Status: UNBUILT; intended for Phase B vertical-slice or later per implementation plan."

---

## Asgard
Asgard is a Norse mythology-themed dungeon in Revenge of the Fallen (ROTF), a RotMG private server. The dungeon features multiple high-tier bosses (Heimdallr, Thor, Loki, Hela, Thusala, Odin) and includes a signature Bifrost bridge-crossing mechanic that separates casual players from completionists. The dungeon is notable for advanced visual effects including 3D terrain, lighting tricks, and environmental storytelling. All six bosses drop legendary unique items, making Asgard a high-value endgame dungeon. Development was showcased in dev blogs (Jan-Feb 2021) where designers discussed level design iterations, visual polish, and mechanical refinement based on closed testing feedback.

**Map:** PARTIALLY DOCUMENTED: Norse mythology-inspired dungeon entrance features a spiraling mountain climb demonstrating engine verticality. Central mechanic is the Bifrost (Rainbow Bridge) challenge that players must traverse to prove 'worthiness' and reach Heimdallr/Observatory. Failed players are beamed back to the Observatory and cannot complete the dungeon. Multiple arena segments documented for different bosses (Thor, Heimdallr, Loki/Hela in Helheim segment with advanced lighting). RECONSTRUCTED: The dungeon appears to have distinct regions - Mountain Entry, Bifrost Challenge Area, Thor Arena, Heimdallr Observatory, Loki Arena, Helheim (Hela's domain). Exact dimensions and complete room structures unknown."

### Heimdallr
- Fight: RECONSTRUCTED: Likely appears early in Asgard dungeon, guards the Bifrost access. Documented as a boss encounter in dev blogs but detailed fight mechanics not documented. Heimdallr's magic is required to open the Bifrost (context from Enchanted Uru Sword description).

### Thor
- Fight: RECONSTRUCTED: Featured in Asgard dungeon preview (aced.gg Jan 31 2021). Developer notes mention camera occlusion issues during Thor encounter with 3D terrain. Specific attack patterns not documented.

### Loki
- Fight: RECONSTRUCTED: Boss fight in Asgard. Dev notes (Feb 6 2021) document 'unintended safe spots' in Loki's arena that allowed players to escape combat. Specific phases and shot patterns not documented. Drops Loki's Dagger (legendary dagger with boomerang effect, poison theme).

### Hela
- Fight: RECONSTRUCTED: Boss fight in Helheim segment of Asgard. Dev notes highlight advanced lighting tricks and visual enhancements in her arena. Specific attack patterns and phases not documented. Drops Hela's power (staff with HP +80 bonus, multi-hit pierce effect).

### Thusala
- Fight: RECONSTRUCTED: Boss appears in Asgard. Item description mentions 'guarded the Necropolis for thousands of years' but this may be flavor text. Drops Thusala's Slasher (legendary sword with 50-1675 damage, primal abilities: 4% bleed chance, swift speed boost). Fight pattern completely undocumented.

### Odin
- Fight: RECONSTRUCTED: Likely final or major boss in Asgard dungeon (Allfather in Norse mythology). Drops Gungnir (legendary spear/sword with pierce and multihit). Specific fight mechanics completely undocumented.

### Asgard Guardian
- Fight: RECONSTRUCTED: Appears to be a minion/realm spawning entity rather than a traditional boss. Item description: 'This bow has harvested souls of many dark elves in its lifetime.' Drops Heavy Bow of the Asgard Guardian (very high damage bow: 1350-1650 minDmg with 11 RoF). May be a special encounter or guardian encounter.

**Drops:** Enchanted Uru Sword, Gungnir, Heavy Bow of the Asgard Guardian, Hela's power, Loki's Dagger, Reaper's Bow, Thusala's Slasher

**Confidence:** Boss names & drops: DOCUMENTED - sourced directly from items.json and dungeon_roster.json with exact drop correlations. Map layout: PARTIALLY DOCUMENTED - General structure and Bifrost mechanic confirmed via aced.gg dev blogs; detailed layout RECONSTRUCTED from thematic context. Fight patterns: RECONSTRUCTED - Only Bifrost challenge explicitly documented; individual boss phases, shot patterns, and attack sequences not found in local data or publicly available sources. Projectiles/Stats: NOT FOUND - Boss HP, Defense, and projectile definitions absent from rotf_objects_FULL.xml and other available local files. Taunts: NOT FOUND - No chat lines or boss dialogue documented in any source. Overall: ~40% documented (drops, names, general theme), ~60% reconstructed or missing (mechanics, stats, dialogue)."

---

## Tomb of Decaying Death
The Tomb of Decaying Death is a graveyard-themed dungeon exclusive to Revenge of the Fallen. It features an undead/necromantic aesthetic with moss, bones, tombstones, and soul cracks. The dungeon is populated by zombie-like creatures in a decorative, anti-aliased environment. Theme is death/decay/undead, intended as a medium to high difficulty encounter for players.

**Map:** RECONSTRUCTED: Graveyard biome theme with scattered tombstones, soul cracks in ground, bones, and moss throughout. Layout is likely arena-like or maze-based similar to other ROTF dungeons. Size and room structure unknown from available documentation.

### Unknown
- Fight: RECONSTRUCTED: Dungeon design article mentions theming but no fight mechanics documented. Graveyard aesthetic with undead enemies suggests moderate to high difficulty encounter. No boss name or specific pattern found in XML or wiki.

**Minions:** Rotten Corpse, Brain Craver, Buried Hand, Revealed Remains, Spoilt Flesh, Tombstone Carrier

**Drops:** Rotting Arm

**Confidence:** PARTIALLY DOCUMENTED: Dungeon theme, biome aesthetic, and minion roster (6 enemy types) are documented from the May 2021 aced.gg design article. The sole known drop (Rotting Arm) is confirmed in items.json. However, NO BOSS STATS, FIGHT PATTERNS, TAUNTS, or detailed MAP LAYOUT found in local client data or wiki mirrors. The XML files (rotf_objects_FULL.xml) do not contain object definitions for these specific minion types, suggesting they may have been removed, not yet implemented, or named differently in the actual game build. The dungeon_roster.json lists empty bosses/minions arrays despite the wiki naming 6 minion types, indicating a gap between design documentation and actual implementation data."

---

## Twilight Necropolis
Twilight Necropolis is a graveyard-themed dungeon exclusive to Revenge of the Fallen featuring undead/necromantic enemies. Theme combines Halloween aesthetics (gravestone decorations) with dark fantasy necromancy. Home to three distinct bosses: Diagon (bone mage), Necro Doggo/Werewolf (melee), and Gravedigger (soul mage). Players encounter decorated graveyard walls and tombstones as environmental obstacles while collecting powerful necromancer-themed loot.

**Map:** RECONSTRUCTED: Graveyard biome theme based on minion types (Holloween Gravestone, Graveyard Wall). Likely features: tombstone/gravestone decorations, walled sections, undead-themed environmental design. Size/structure: unknown - arena/biome layout not documented. Entrance: typical dungeon portal entrance (presumed standard ROTF format).

### Diagon
- Projectiles: TN Diagon Proj4 (Speed: 100, Damage: 350, Size: 75, LifetimeMS: 1200, MultiHit)
- Fight: RECONSTRUCTED: Diagon appears to use bone/necrotic-themed projectiles based on item descriptions. Fight pattern unknown but likely involves undead/skeletal magic given the Wand of Bone drop.

### Necro Doggo (Werewolf)
- Fight: RECONSTRUCTED: Werewolf-themed melee boss based on item description 'Claw of the Beast, freshly plucked from the forepaw of the Necropolis werewolf'. Likely uses claw/bite attacks.

### Gravedigger
- Fight: RECONSTRUCTED: Staff-wielding boss (Gravedigger's Shovel drop). Likely uses soul/necrotic magic based on shovel description containing 'limitless power of thousands of souls'. Possible buff/debuff mechanics.

**Minions:** Holloween Gravestone, Holloween Graveyard Wall

**Drops:** Claw of the Beast, Gravedigger's Shovel, Undead's Gross Bow, Wand of Bone, Wand of Retribution

**Confidence:** LOW TO MODERATE. Boss stats (HP/Defense), exact fight patterns, and taunts are ENTIRELY RECONSTRUCTED from item flavor text. Only documented data: minion names/textures from XML, drop table from roster, boss names from DUNGEONS.md implementation notes, and projectile stats for one Diagon projectile. Dungeon theme is reliably documented as graveyard/necropolis. Three bosses confirmed by name only (no stats). Minion environment objects have exact texture/model data. This dungeon is marked in DUNGEONS.md as 'NOT YET IMPLEMENTED' - all encounter specifics require reconstruction from thematic context.

---

## Icy Peaks
Icy Peaks is a ROTF-exclusive dungeon themed around an ice-covered mountain with a frozen queen boss. It features Yeti minions and drops legendary katanas and wands. Located in Phase D2 build plan per DUNGEONS.md (row 7), currently status TODO.

**Map:** RECONSTRUCTED: Dungeon theme is ice-covered peaks/mountain. No .jm map file exists yet; layout/rooms/biome size unknown. Per DUNGEONS.md Phase D2, will reuse Illusion recipe with BehaviorDb per dungeon. Access via portal (0x82xx Portal object type).

### Ice Queen / Frozen Queen
- Fight: RECONSTRUCTED: No boss entity currently defined in rotf_objects_FULL.xml. Based on drops and dungeon_roster.json, the Ice Queen/Frozen Queen is named but not yet implemented. Fight pattern unknown.

**Minions:** Big Yeti (HP: 7000, DEF: 0, Texture: chars16x16rEncounters:123, Projectile: Ice Burst 60dmg 60spd 2000ms), Mini Yeti (HP: 2890, DEF: 0, Texture: chars8x8rEncounters:118, Projectile: Ice Burst 60dmg 40spd 3000ms), Snow Bat (HP: 1200, DEF: 5, Texture: chars8x8rEncounters:119, Flying, Contact damage 77), Snow Bat Mama (HP: 4240, DEF: 10, Texture: chars16x16rEncounters:124)

**Drops:** Asura, Ice Shatter, Wand of the Frozen Queen, Fang of Frost

**Confidence:** Minions fully documented from rotf_objects_FULL.xml with exact MaxHitPoints, Defense, AnimatedTexture File+Index, and Projectile stats (Ice Burst: 60dmg, 40-60spd, 2000-3000ms, ArmorPiercing). Drops documented via dungeon_roster.json and items.json (3/4 items verified; Fang of Frost listed as Sprite World drop in items.json). UNDOCUMENTED: Ice Queen/Frozen Queen boss entity does not exist in client XML. No boss HP, Defense, texture, projectiles, or fight pattern. Map layout not implemented (.jm file missing). Boss design marked TODO in DUNGEONS.md. Overall: 40% documented (minions+drops), 60% reconstructed/missing (boss fight, map layout)."

---

## "Scorched Plains (alias: Fiery Planes)"
"Scorched Plains (Fiery Planes) is a ROTF-exclusive dungeon centered on the Firebreather boss encounter. The dungeon features a fire/lava-themed landscape with Native Fire Sprite minions. The primary boss, Firebreather, is a flame-wielding entity dropping three Searing Artifact (ST) items and one Legendary (UT) staff. A secondary boss or phase form, Blazetalon (embodied as a legendary UT katana), suggests multi-phase or dual-boss mechanics. The dungeon tier is moderate-to-high based on loot (ST + UT). No original client data, map, or boss behavior AI exist in the codebase — all stats/mechanics are reconstructed from wiki drops and thematic inference."

**Map:** RECONSTRUCTED - Desert/volcanic plains theme with lava flows. No static map exists in repo yet (marked TODO in DUNGEONS.md). Dungeon is exclusive to ROTF with no vanilla RotMG counterpart. Likely arena-style or multi-room pyrolytic environment with fire/lava environmental hazards. Size/structure unknown — design from theme + drop tier framework per IMPLEMENTATION_PLAN.md Phase C3."

### Firebreather
- Projectiles: Red Fire (inferred from minion)
- Fight: RECONSTRUCTED - Boss fires flame projectiles in patterns; theme suggests multiple fire-based attack phases. Drops high-tier ST/UT items (Firebreather's Tail, Head of the Firebreather, The Searing Blood) indicating moderately challenging encounter. Second boss or variant likely 'Blazetalon' (UT katana minion/add or second phase).

### Blazetalon
- Fight: RECONSTRUCTED - Legendary (rarity=l) weapon drop suggests this is either a second boss, a minion wave, or a Firebreather second-phase form. Magmarock/flame theme aligns with dungeon pyrolytic aesthetic.

**Minions:** Native Fire Sprite

**Drops:** Firebreather's Tail (ST Dagger: 85-190 dmg, 1 shot, 10 proj-speed, 550ms lifetime, piercing), Head of the Firebreather (ST Staff: 40-70 dmg, 4 shots, rof 120, 9 range, piercing, fame bonus 5), The Searing Blood (ST Poison: 1300 total dmg, 450 impact, radius 5.6, 850 poison damage, 4s duration, cost 130 mana, fame bonus 5), Flameblast Staff (UT Staff: 400-500 dmg, 2 shots, rof 35, range 4.7, legendary rarity, fame bonus 7), Blazetalon (UT Katana: 140-160 dmg, 2 shots, rof 150, range 5.5, piercing, legendary rarity, fame bonus 7)

**Confidence:** "DOCUMENTED: Native Fire Sprite minion (exact XML stats extracted). All drop item names, descriptions, stats (dmg ranges, rarity tiers, slots). Boss names, drop relations (dropsFrom field). RECONSTRUCTED: Firebreather & Blazetalon boss HP/defense/texture (no XML entries exist; both are red-links on wiki with only prose/item-drop evidence). Fight pattern, map layout, taunt lines (none documented). All boss mechanics inferred from loot tier (ST + UT = moderate-high difficulty) and thematic fire/lava aesthetic. No original client footage, map files, or AI behavior definitions recovered."

---

## Deadwater Docks
Deadwater Docks (Ocean Trench internally) is a mid-to-high difficulty water-themed dungeon centered on Thessal the Mermaid Goddess. Features multi-phase boss fight with dangerous spawnable minions and ocean-themed enemies throughout. Theme: coastal underwater arena with coral, rocks, and seaweed.

**Map:** Theme: Underwater/Oceanic. Structure: Primary boss arena with Thessal centerpiece. Environmental decoration: OceanRock (lofiEnvironment2 indices 0x6e/0x6f/0x74), Seaweed (0x75/0x76), Coral (0x77-0x7a) - all randomized Static objects. Walls: Ocean Wall (lofiEnvironment2 0x7d with top 0x7c) and Ocean Wall2 (0x7b with top 0x7c) - both full-occupy, sight-blocking walls. Scope/Size: RECONSTRUCTED - appears single large arena based on spawning mechanics. Entrance: RECONSTRUCTED as typical dungeon portal from Nexus/hub world.

### Thessal the Mermaid Goddess — 138000 HP, Def 85, tex chars16x16dEncounters2:16
- Projectiles: Thunder Swirl (DMG 135, Speed 55, Size 110, 1600ms, Armor Broken 4s, MultiHit); Trident (DMG 100, Speed 70, Size 100, 3000ms, MultiHit); Super Trident (DMG 145, Speed 60, Size 110, 3000ms, MultiHit); Yellow Wall (DMG 155, Speed 60, Size 160, 3000ms, Weak 6s, MultiHit)
- Fight: DOCUMENTED - PHASE 1 (HP>50%): Wanders/Follows (speed 0.3). Spawns Deep Sea Beast when none nearby. Cycles Attack1 randomly (3s cooldown): Thunder Swirl 8-burst x3 (500ms apart); Super Trident 2x2 at 4 angles x2 phases (250ms); Yellow Wall 30-shot x3 waves (500ms apart, yellow flash); Spawning Bomb tosses 4 Coral Bomb Big (45/135/225/315 deg); Spawning Deep tosses 4 Deep Sea Beast (cardinal angles). PHASE 2 (HP<50%): Attack2 - Thunder Swirl 16-burst; Super Trident 4-phase extended; same bomb/deep spawning. Death: 10% transforms to Thessal Wounded (invincible dialogue form), 90% spawns Thessal Dropper (loot entity).

### Thessal the Mermaid Goddess Wounded — 960000 HP, Def 60, tex chars16x16dEncounters2:18
- Fight: DOCUMENTED - Post-boss invincible form. Spawns with texture cycling (indices 18<->19, 250ms flicker). Taunts 'Is King Alexander alive?' with 12s timeout. If correct answer (Prize state): says 'Thank you kind sailor.', tosses 3 Coral Gift at angles 45/135/235, suicides (rewards player). If timeout (Fail): says 'You speak LIES!!', suicides immediately.
- Taunts: ['Is King Alexander alive?', 'Thank you kind sailor.', 'You speak LIES!!']

### Thessal Dropper — 960000 HP, Def 0, tex lofiObjBig:0xb6
- Fight: DOCUMENTED - Invisible loot dropper (Size 0, no minimap). Spawned on Thessal death. Invincible. Despawns if Thessal ceases to exist 100+ ticks. Transforms to Ocean Vent on death.

**Minions:** Deep Sea Beast (HP 15000, DEF 60, spawnable by Thessal, grows on spawn, shoots 4 projectile types 1.8-4.2 range, MultiHit, StasisImmune, PerRealmMax 1), Deep Sea Beast Spawner (HP 8000, DEF 50, invisible utility spawner), Coral Bomb Big (HP 1000, DEF 200, spawned by Thessal, tosses 6 Coral Bomb Small then shoots 5-spread Coral Spike, suicides), Coral Bomb Small (HP 1000, DEF 200, tossed by Big, shoots 5-spread Coral Spike then suicides), Coral Gift (HP 1200, DEF 5, loot entity tossed by Thessal Wounded, cycles textures, non-combat), Fishman (HP 3000, DEF 40, common minion, follows, shoots Boomerang+Spear x3 with Confused effect), Fishman Warrior (HP 10000, DEF 52, stronger variant, orbits+follows, shoots Spear x3 and Coral Shot x6, range-shoot state), Sea Horse (HP 1300, DEF 20, flying, orbits Sea Mare, shoots Bubble x1/2/3 cascades), Sea Mare (HP 7000, DEF 60, charges, cycles Shoot1 (3x Spinner) and Shoot2 (8x Squid repeats at angles), Grey Sea Slurp (HP 1300, DEF 20, swarming, stays near spawn, cycles Shoot+Move and Wall Shoot, max 8 from spawner), Giant Squid (HP 6000, DEF 40, NOT in roster but in code, tosses Ink Bubble then attacks, shoots Ink Blot), Ink Bubble (HP 2000, DEF 40, flying spawned by Squid, continuous Ink Blot with Blind effect), Sea Slurp Home (spawner entity, generates max 8 Grey Sea Slurp)

**Drops:** Cloak of the 1000 Oceans (UT Cloak, Thessal drop), Dagger of Goddess of Goddesses (UT Dagger, Thessal drop), Radiant Wand of Royalism (UT Wand Legendary, Thessal drop), Sword of the Aquatic God (UT Sword, Thessal drop), Potion of Mana (guaranteed 32% threshold Thessal Phase 2), Coral Juice (0.3 rate Thessal/Coral Gift), Sea Slurp Egg (0.25 rate Coral Gift), Coral Bow (0.01 rate Coral Gift), Coral Venom Trap (0.03 rate Coral Gift), Wine Cellar Incantation (0.02 rate Coral Gift), Coral Silk Armor (0.04 rate Coral Gift), Coral Ring (0.04 rate Coral Gift)

**Confidence:** DOCUMENTED: All boss stat blocks (HP/DEF/texture/projectiles) from XML. All fight mechanics (phases, attacks, spawns) from BehaviorDb.cs with exact timing/counts. All taunts/dialogue exact from code. All minion behaviors/projectiles from code + XML. Map layout theme (ocean/coral/walls) DOCUMENTED from EmbeddedData XML objects. Entrance/room layout RECONSTRUCTED - no explicit map generator found, inferred from environmental objects and dungeon pattern conventions. Giant Squid in code but NOT in parsed roster (possible unused or alternate spawn). Confidence HIGH for combat mechanics, MEDIUM for spatial layout details.">

---

## Limon's Lair
Limon's Lair is an elemental sprite-themed dungeon in ROTF featuring four elemental entities (Limon Elements 1-4) and a central boss (Limon the Sprite God). The dungeon also spawns six types of minion sprites with elemental affinities (Fire, Ice, Magic, Nature, Darkness variants plus a Nature Sprite God). All enemies are Flying, sprite-class beings. The four Elements are heavily armored (Defense 1000) but share identical attack patterns. The central boss is lower defense but higher HP and deals rapid fire damage. The minions are weak but numerous.",
    "sources": [
  "/home/jesse/rotf/assets/rotf-original-data/rotf_objects_FULL.xml (Objects 0x0d00-0x0d0a, all sprite entities with exact HP, Defense, Textures, Projectiles documented)",
  "/home/jesse/rotf/docs/rotf-wiki/parsed/dungeon_roster.json (Dungeon roster showing boss/minion/drop lists for Limon's Lair)",
  "/home/jesse/rotf/docs/rotf-wiki/revengeofthefallengame/Dungeons.wiki (Portal image reference: LimonsLairPortal.png)",
  "/home/jesse/rotf/docs/rotf-wiki/revengeofthefallengame/Limon_s Lair.wiki (Wiki page exists but is a stub containing only 'asdf')"
]

**Map:** RECONSTRUCTED: Theme is Sprite-elemental (5 sprite types + 4 elements). No documented room structure, entrance type, or terrain layout in available sources. Wiki page is a stub (only contains 'asdf'). Dungeon portal appears in main dungeon list (LimonsLairPortal.png in INDEX). Likely arena or arena-like structure given 4 Elements + 1 central Sprite God design pattern.

### Limon Element 1 — 100000 HP, Def 1000, tex chars8x8dEncounters:0
- Projectiles: White Flame
- Fight: RECONSTRUCTED: Four Elements (1-4) spawn symmetrically around arena. Each Element fires White Flame projectiles (Damage 50, Speed 90, LifetimeMS 1940, MultiHit, PassesCover). StasisImmune. No documented phases or special patterns in source XML or wiki.

### Limon Element 2 — 100000 HP, Def 1000, tex chars8x8dEncounters:0
- Projectiles: White Flame
- Fight: RECONSTRUCTED: Part of 4-Element configuration. Fires identical White Flame pattern as Element 1. StasisImmune.

### Limon Element 3 — 100000 HP, Def 1000, tex chars8x8dEncounters:0
- Projectiles: White Flame
- Fight: RECONSTRUCTED: Part of 4-Element configuration. Fires identical White Flame pattern as Elements 1-2. StasisImmune.

### Limon Element 4 — 100000 HP, Def 1000, tex chars8x8dEncounters:0
- Projectiles: White Flame
- Fight: RECONSTRUCTED: Part of 4-Element configuration. Fires identical White Flame pattern as Elements 1-3. StasisImmune.

### Limon the Sprite God — 30000 HP, Def 30, tex lofiChar216x16:0x36
- Projectiles: Red Fire
- Fight: DOCUMENTED (XML): Central boss. Fires Red Fire projectiles (Damage 80, Speed 80, LifetimeMS 2300, MultiHit). Flying, God-class, StasisImmune, GivePotions. XpGain 3000. Sounds: monster/sprite_god_hit and monster/sprite_god_death.

**Minions:** Native Fire Sprite (Type 0x0d00, HP 100, Def 0, AnimatedTexture chars8x8rMid:0x00, Projectile: Red Fire Damage 17 Speed 50 LifetimeMS 4000), Native Ice Sprite (Type 0x0d01, HP 100, Def 0, AnimatedTexture chars8x8rMid:0x01, Projectile: Ice Spinner Damage 14 Speed 80 LifetimeMS 1800 Slowed), Native Magic Sprite (Type 0x0d02, HP 100, Def 0, AnimatedTexture chars8x8rMid:0x02, Projectile: Green Magic Damage 18 Speed 50 LifetimeMS 4000), Native Nature Sprite (Type 0x0d03, HP 100, Def 0, Texture lofiChar28x8:0x30, Projectile: Green Bolt Damage 16 Speed 100 LifetimeMS 2000), Native Darkness Sprite (Type 0x0d04, HP 100, Def 0, Texture lofiChar28x8:0x31, Projectile: Darkness Bolt Damage 16 Speed 50 LifetimeMS 3000), Native Sprite God (Type 0x0d05, HP 2000, Def 12, AnimatedTexture chars16x16dMountains1:0x01, Projectiles: Purple Magic Damage 100 Speed 70 LifetimeMS 2000 AND Grey Boomerang Damage 0 Speed 60 LifetimeMS 2400 Quiet effect MultiHit ParticleTrail)

**Confidence:** MIXED: All boss/minion stats, textures, projectiles, and sound effects are DOCUMENTED from rotf_objects_FULL.xml (exact values). Dungeon roster confirms boss names and minion types. However, fight PHASES, ATTACK PATTERNS, ARENA LAYOUT, and CHAT TAUNTS are NOT documented anywhere — all reconstructed from game mechanics (Flying status, enemy groupings, projectile properties). No YouTube gameplay footage, dev blog posts, or wiki articles with detailed descriptions were found. The dungeon's structure and combat dynamics must be inferred from object type groupings and defensive/offensive stats."

---

## Esben's Lair
Esben's Lair is a Gothic-themed single-arena dungeon (44x44, Difficulty 4) featuring two bosses: Esben the Unwilling (90k HP, primary threats via 4 projectile types including armor-piercing and debuff shots) and ic Loot Balloon (120k HP, loot vessel). The arena is decorated with Gothic architecture (pillars, arches, windows, bookshelves, portraits). Combat phase: Esben wanders while unleashing multi-projectile spreads and armor-piercing bursts; loot drops at 5% HP. Signature drops: Handcannon, Starbuster Blade. All boss stats and mechanics are documented from the 2019 ROTF client data; map layout is documented in the server world files.

**Map:** Theme: Gothic/Undead fortress. Size: 44x44 tile arena on Grass ground. Difficulty: 4/10. Entrance region: marked 'Spawn'. Boss arena populated with Esben, Loot Balloon (separate entity), and decorative Gothic architecture (Pillars, Arches, Windows, Bookshelves, Portraits, Landscape pieces). Also contains: Epic Quest Chest (5000 HP, 140 size), references to Canopic Jars and Classic Ghost (may be placeholder/legacy objects in map data).

### ic Esben the Unwilling — 90000 HP, Def 20, tex chars16x16rEncounters:121
- Projectiles: Sound Wave (Damage: 70, Speed: 60, Lifetime: 8000ms, Effect: Unstable 7s); Armor Pierce Bullet (Damage: 75, Speed: 60, Lifetime: 8000ms, ArmorPiercing, MultiHit); Ice Burst (Damage: 70, Speed: 60, Lifetime: 8000ms, ArmorPiercing, MultiHit); Ice Bullet (Damage: 45, Speed: 55, Lifetime: 8000ms, Effects: Quiet 3.5s + Dazed 3.5s)
- Fight: DOCUMENTED: Primary phase: Wander (speed 0.45) while shooting 8 projectiles in fan pattern (3-count, 15° spread, 900ms cooldown) and secondary armor-piercing shots (1600ms cooldown). At 5% HP triggers loot threshold. Texture alts include invisible state and alternate forms.

### ic Loot Balloon — 120000 HP, Def 0, tex lofiObj3:0x466
- Fight: RECONSTRUCTED: Loot vessel — minimal combat AI expected. Likely stationary or slow-moving target with no documented projectile pattern. Primary mechanic appears to be loot generation on defeat.

**Minions:** Epic Quest Chest (documented: 5000 HP, Quest loot chest), Gothic Arch (environment/wall), Gothic Bookshelf (environment/wall), Gothic Bookshelf Env (environment variant), Gothic Bricks (environment/wall), Gothic Landscape Lf (environment decoration), Gothic Landscape Rt (environment decoration), Gothic Pillar (environment/wall), Gothic Portrait (environment decoration), Gothic Window 1 (environment decoration)

**Drops:** Handcannon (documented from dungeon_roster.json), Starbuster Blade (documented from dungeon_roster.json)

**Confidence:** HIGH confidence on core boss stats/mechanics: Esben's exact HP/Defense/texture/4 projectile types with all damage/speed/effects are documented in rotf_objects_FULL.xml. Loot Balloon HP/texture documented. Fight AI (Wander + Shoot patterns + loot threshold) documented in BehaviorDb.cs. Map structure (44x44, Grass, Spawn region) documented in .jm/.jw files. Drops (2 items) documented in dungeon_roster.json. MODERATE confidence on minion classification: Gothic objects are listed in roster but confirmed as environment/wall Class in XML (not combat enemies). Epic Quest Chest is confirmed combat entity. References to Canopic Jars and Classic Ghost in map data are RECONSTRUCTED as likely legacy/placeholder objects not present in current object XML — they may be removed or renamed.

---

## Slime God Den
Sewer-themed dungeon featuring Gulpord the Slime God, a multi-phase boss who shoots poisonous and slowing projectiles. The dungeon contains various sewage-themed minions including brown and yellow slimes, goblins, and aquatic creatures like alligators and turtles. Central boss arena surrounded by grass terrain.

**Map:** DOCUMENTED from SlimeGodDen.jm/.jw: 44x44 grass arena with single spawn region. Difficulty:4, Background:0, Blocking:0, no TP restriction, shows displays, non-persistent. Portal ID 0x8205 (33285) leads back to Nexus. RECONSTRUCTED theme: Sewer-themed dungeon with grass ground - likely features central boss arena with surrounding spawn areas for minions. The entity dictionary shows Gulpord the Slime God placed as primary boss with Boss Minion (Gulpord Slime) variants, surrounded by various slime types, goblins, and aquatic creatures distributed throughout the arena.

### Gulpord the Slime God — 45000 HP, Def 35, tex chars16x16rEncounters:126
- Projectiles: Poison Fireball (Damage 85, Speed 85, LifetimeMS 1700); Red Star w/ Armor Broken (Damage 45, Speed 50, LifetimeMS 1750, Duration 2.5s); Green Star w/ Slowed (Damage 45, Speed 50, LifetimeMS 2000, Duration 3s); Green Star w/ Slowed (Damage 45, Speed 50, LifetimeMS 2400, Duration 1.5s)
- Fight: DOCUMENTED: Single phase documented in XML with 4 projectile patterns. HP:45000, Defense:35, Size:120, StasisImmune, GivePotions=true. Fires Poison Fireball + Red Star (Armor Break) + 2x Green Star (Slowed). RECONSTRUCTED: Likely has phase mechanics based on existence of Medium (9500 HP, Defense 30, Size 70) and Small (6000 HP, Defense 25, Size 50) variants in XML - probable Phase 2 split similar to vanilla Gulpord where boss becomes invulnerable and splits into smaller clones

**Minions:** DS Brown Slime (Sewer Brown Slime, HP:2750, Def:15, Projectile: Brown Magic Damage 40), DS Yellow Slime (Sewer Yellow Slime, HP:2500, Def:5, Projectile: Yellow Magic w/ArmorPiercing Damage 30), DS Boss Minion (Gulpord Slime, HP:1500, Def:0), DS Alligator (Sewer Alligator, HP:1700, Def:25, Projectile: Z Blast w/ArmorPiercing Damage 40), DS Goblin Knight (HP:2450, Def:18, Projectile: White Bolt Damage 140), DS Goblin Brute (HP:1750, Def:12, Projectile: Green Bolt Damage 90), DS Goblin Peon (HP:900, Def:10, Projectile: Green Magic w/ArmorPiercing Damage 20), DS Bat (Sewer Bat, HP:125, Def:25, Flying, Projectile: Invisible w/Confused Damage 40), DS Blue Turtle (HP:4200, Def:10, StasisImmune), DS Brown Slime Trail (lofiObj3 RandomTexture, decorative), DS Yellow Slime Trail (lofiObj3 RandomTexture, HP:1000, decorative), DS Fly (Sewer Fly, HP:100, Flying, lofiObj3 animated), DS Natural Slime God (HP:1000, Def:12, chars16x16dMountains1, Projectiles: Red Fire & Green Star Slowed)

**Confidence:** High confidence on all stat numbers (MaxHitPoints, Defense, projectile damage/speed/lifetime) - these are DOCUMENTED directly from rotf_objects_FULL.xml with exact values. High confidence on minion roster from dungeon_roster.json. Moderate confidence on fight pattern Phase 1 (projectiles documented, stationary stance inferred from XML structure). Low-to-moderate confidence on Phase 2/3 phases - the existence of Medium and Small Gulpord variants in the XML suggests a split mechanic similar to vanilla RotMG, but no explicit fight script found in available docs. High confidence on map layout dimensions (44x44, grass theme) from SlimeGodDen.jm. No documented drops found in items.json or dungeon_roster.json. No chat/taunt lines found in XML. All boss/minion textures and animations documented from XML AnimatedTexture/Texture fields."

---

## Bunny Hollow
Bunny Hollow is an Easter-themed arena dungeon featuring Biff the Buffed Bunny as the main boss, surrounded by egg-themed minions. The dungeon has a cheerful, pastoral aesthetic and appears designed as a casual/thematic encounter.

**Map:** Theme: Spring/Easter-themed grassy meadow. Size: 44x44 tile grid. Structure: Open arena with single spawn region and one boss placement point. No multi-room dungeon structure documented. Difficulty: 4/10. Portal ID: 0x8209 (from Nexus spawning code).

### Biff the Buffed Bunny — 208000 HP, Def 0, tex buffedBunnyChars16x16:1
- Projectiles: BB Egg Cannon 1 (Damage 150, Speed 60, Lifetime 1800ms, Confused 2.5s); BB Bunny Blaster (Damage 130, Speed 70, Lifetime 1400ms); BB Bunny Blaster (Damage 130, Speed 100, Lifetime 1400ms); BB Egg Cannon 1 (Damage 150, Speed 60, Lifetime 2000ms, Confused 2.5s); BB Egg Cannon 1 (Damage 150, Speed 60, Lifetime 2200ms, Confused 2.5s); BB Bunny Blaster (Damage 130, Speed 70, Lifetime 1800ms)
- Fight: DOCUMENTED - Wanders at 0.5 speed. Phase 1: Shoots 5 projectiles in 20-degree fan (cooldown 700ms). Phase 2: Shoots 3 projectiles in 12-degree fan (cooldown 1400ms). Repeating pattern. Immune to Stasis, Stun, Paralyze.

### Mysterious Egg (BB Biff Summoner) — 300000 HP, Def 0, tex buffedBunnyObjects16x16:0x18
- Fight: RECONSTRUCTED - No behavior or projectiles documented. Appears as secondary boss in roster but role unclear. May summon Biff or serve as pre-phase trigger.

**Minions:** Easter Egg (BB God Egg): 10000 HP, 0 DEF, Size 60, Easter Egg (BB High Egg): 10000 HP, 0 DEF, Size 60, Easter Egg (BB Mid Egg): 10000 HP, 0 DEF, Size 60, Easter Egg (BB Low Egg): 10000 HP, 0 DEF, Size 60

**Drops:** Tier 6 Potions (2x required, 10% drop rate), Tier 11 Weapons (1x required, 2% drop rate), Tier 12 Armor (1x required, 2% drop rate)

**Confidence:** STATS/TEXTURES/PROJECTILES: Documented from rotf_objects_FULL.xml. FIGHT PATTERN: Partially documented (behavior wander/shoot patterns in BehaviorDb.cs, projectiles fully detailed). PHASES/TAUNTS: Not found. LAYOUT: Basic grass arena confirmed (BunnyHollow.jm shows 44x44 grid); no complex room structure in data. BIFF SUMMONER: HP documented but zero behavior definition—role speculative.

---

## Gate to the Underworld
Gate to the Underworld is a ROTF-exclusive planned dungeon featuring Craig the Mad Intern (a chaotic overworked intern boss with 3 combat phases) alongside three Oryx trap obstacles (Darkness, Daze, Confuse) and a Thunder Helper support unit. The dungeon hosts a mix of minions from ocean/slime/forest themes. It serves as a mid-tier challenge with mixed elemental and debuff mechanics. Status: Exists in dungeon roster but NOT implemented in server/map files as of June 2026 — conceptual design phase only.

**Map:** RECONSTRUCTED (not yet implemented). Theme: Eclectic arena mixing Underworld/Oryx/Elemental elements. Likely structure: central arena containing Craig the Mad Intern as primary boss, surrounded or interspersed with three stationary Oryx trap obstacles (Darkness/Daze/Confuse) as environmental hazards. Thunder Helper positioned as aerial support unit. Minions (sea creatures, rats, slimes) scattered throughout arena as secondary threats. Entrance/exit via portal. Size: medium arena (8x8 rooms estimated based on other ROTF dungeons like Illusion/Deadwater). Blocking/environmental: Candy Straw Columns (broken/whole) provide destructible cover. Ocean coral reefs (Coral Gifts, Bombs) add secondary arena features.

### Craig the Mad Intern — 161000 HP, Def 50, tex chars8x8rMid:0x02
- Projectiles: Paperwork shotgun (5 pellets); Spiral blast (12 pellets); Predictive fire (6 pellets)
- Fight: DOCUMENTED (from BehaviorDb.Craig.cs): Phase 1 'deadlines' at 100% HP — stays near spawn, fires 5-pellet paperwork shotgun + summons Stressed Interns (max 6). Phase 2 'overtime' at 50% HP — taunt 'Do you have ANY idea how many TPS reports I have?!' — fires 8-pellet spread + 12-pellet spiral, spawns more interns (max 10). Phase 3 'meltdown' at 20% HP — taunt 'THAT'S IT. I QUIT. AND I'M TAKING YOU WITH ME.' — manic fire: 10-pellet spreads (450ms cooldown) + 6-pellet predictive shots (800ms cooldown). XpMult: 1.6.
- Taunts: ['Do you have ANY idea how many TPS reports I have?!', "THAT'S IT. I QUIT. AND I'M TAKING YOU WITH ME."]

### Oryx Darkness Trap — 100000 HP, Def 35, tex oryxHordeChars8x8:2
- Projectiles: Oryx Darkness Trap Proj (Damage 30, Speed 80, LifetimeMS 160, Size 80)
- Fight: DOCUMENTED (XML): Flying, stasis/paralyze immune, XpMult 0.0. Static arena trap. Fires Oryx Darkness Trap Proj — applies Armor Broken + Darkness (3s duration).

### Oryx Daze Trap — 100000 HP, Def 35, tex oryxHordeChars8x8:3
- Projectiles: Oryx Daze Trap Proj (Damage 30, Speed 80, LifetimeMS 160, Size 80)
- Fight: DOCUMENTED (XML): Flying, stasis/paralyze immune, XpMult 0.0. Static arena trap. Fires Oryx Daze Trap Proj — applies Armor Broken + Dazed (3s duration).

### Oryx Confuse Trap — 100000 HP, Def 35, tex oryxHordeChars8x8:4
- Projectiles: Oryx Confuse Trap Proj (Damage 30, Speed 80, LifetimeMS 160, Size 80)
- Fight: DOCUMENTED (XML): Flying, stasis/paralyze immune, XpMult 0.0. Static arena trap. Fires Oryx Confuse Trap Proj — applies Armor Broken + Confused (3s duration).

### Thunder Helper — 100000 HP, Def 35, tex oryxHordeObjects16x16:0x00
- Projectiles: Thunderbolt (Damage 95, Speed 120, LifetimeMS 240, Size 80, applies Paralyzed 1.2s)
- Fight: DOCUMENTED (XML): Flying, stasis/paralyze immune, XpMult 0.0. Animated (2-frame). Fires Thunderbolt projectiles at 120 speed, 95 damage, applies Paralyzed (1.2s). Secondary support unit.

**Minions:** Candy Straw Column Broken (static destructible, lofiObjBig:0x72), Candy Straw Column Whole (static destructible, lofiObjBig:0x73), Coral Bomb Big (HP 1000 Def 200, chars8x8dEncounters:15, Size 120, fires Coral Spike), Coral Bomb Small (HP 1000 Def 200, chars8x8dEncounters:17, Size 60, fires Coral Spike), Coral Gift (HP 1200 Def 5, lofiEnvironment3:0x22-24, Size 90, stasis-immune), DS Brown Slime Trail (HP 1000 Def 10, lofiObj3:0x57c-7f random, Size 90), DS Golden Rat (HP 5500 Def 10, chars8x8rEncounters:132, stasis/stun/paralyze-immune), DS Master Rat (HP 10000 Def 60, chars8x8rEncounters:142, Size 140, fires Wereblast Dmg 50), DS Natural Slime God (HP 1000 Def 12, chars16x16dMountains1:0x07, Size 100, god-type, fires Red Fire Dmg 80), DS Orange Turtle (HP 4200 Def 10, chars8x8rEncounters:135, Size 90, stasis-immune)

**Drops:** Deepspace Veil, Flask of Stardust, Gravedigger's Shovel, Stem of the Brain, Thusala's Slasher, Undead's Gross Bow, Wand of Retribution, Windcutter

**Confidence:** HIGH for boss stats, projectiles, textures, HP, defense (all extracted from real 2019 client XML or active server code). PARTIAL for Craig fight pattern detail (documented in active code; 3 phases/taunts verified). RECONSTRUCTED for map layout (dungeon exists in roster but has NO server .jm/.jw files; arena structure inferred from boss/minion mix and ROTF dungeon conventions). Fight patterns for the three Oryx traps and Thunder Helper are minimal (static/arena obstacles with single projectile types); no documented AI phases. Dropped items listed in roster (verified against items.json); no quantity/rarity data in sources. Overall: This dungeon is PLANNED but NOT IMPLEMENTED in server code as of June 2026."

---

## Broken Forest
A referenced-only forest dungeon in ROTF with wood/candy hybrid theme. No map, bosses, or mechanics are implemented; only metadata and item drops exist.

**Map:** RECONSTRUCTED: Theme suggests broken/decaying forest with candy-themed columns. No map file exists (no .jm/.jw). Likely small to medium dungeon arena with candy pillar obstacles (Candy Choc/Straw Columns). Expected entrance via portal, single arena or multi-room layout.

### Unknown Boss 1
- Fight: RECONSTRUCTED: No real data. Theme suggests wood/nature with candy hybrid. ST tier loot (Broken Branch, Wooden Elegance) indicates mid-tier difficulty. Likely 1-2 bosses. No chat lines, phases, or attack patterns documented.

### Unknown Boss 2
- Fight: RECONSTRUCTED: Possible secondary boss or minion. No implementation exists.

**Minions:** Candy Choc Column Broken (type:0x18dd, lofiObjBig:0x70) - Static environment piece, Candy Straw Column Broken (type:0x18e0, lofiObjBig:0x72) - Static environment piece

**Drops:** Broken Branch (Katana, ST, Rarity A) - 300-450 dmg, +6 ATT +4 DEX, Wooden Elegance (Bow, ST, Rarity A) - 65-95 dmg, +4 ATT +6 DEX

**Confidence:** ALMOST ENTIRELY RECONSTRUCTED. Documented: dungeon entry in dungeon_roster.json, two item drops with descriptions mentioning 'Broken Forest', two minion object IDs from rotf_objects_FULL.xml. NOT documented: boss stats, HP, defense, projectiles, fight phases, chat/taunts, map layout, portal type, BehaviorDb implementation. No wiki page, no map file, no real gameplay data exists. Only metadata + theming from item descriptions (wood/candy hybrid forest).

---

