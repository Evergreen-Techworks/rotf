// MECH-LOOP: OCBosses2 — multi-boss file. Per-boss arena comments added below as each boss is processed. Infused Elven King: Elven Forest dungeon — green oval/circular arena, dark green forest floor, water patches, dark tree trunks. DOCUMENTED disc_iBUg7V_Eq0A f_00240 + minimap_00025/030.
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // Ordinary Client — backlog batch (2026-06-04): recovered ROTF realm/meme bosses.
    // TAUNTS are VERBATIM from video recovery (see docs/video-recovery + build_ledger.json).
    // Objects 0x8f02-0x8f0c (OrdinaryClient_Bosses.xml). Collision-checked clean vs base game.
    // Attack geometry reconstructed (footage was chat-feed-heavy, few clean fight frames). Auto-registered.
    partial class BehaviorDb
    {
        private _ OCBosses2 = () => Behav()
            // Marble Colossus — catacombs guardian (taunts confirmed x3 across disc_XQUmPL07sFY/disc_Yx0kTFEixbA)
            .Init("Marble Colossus",  // arena: Catacombs — gray rectangular brick floor (rows of bricks each with small dark oval center), large open room with dark void walls; lighter X-tile marble entry corridor — DOCUMENTED-from-frames disc_v6HXKS92zAI (b002_16s f001: X-tile entry room + Colossus entity + C-arc guard bullets; f006: catacombs room + slipping ring ~12-14 large round bullets expanding; f018: dense ring expansion on gray floor + "I feel myself... Slipping..." chat CONFIRMED) + disc_xEFLSmxIpSA (b002_6s f006: gray brick+oval floor close-up; f001: HP 0.01K/772.5K = MAX HP 772,500 CONFIRMED + "Robe of Shimmering Stars" LEGENDARY loot observed — NOT in XML). Guard C-arc bullets observed f001 (count/angle unresolvable); slipping ring DOCUMENTED (count:14/angle:25 CONSISTENT).
                new State(
                    new State("idle", new PlayerWithinTransition(14, "guard")),
                    new State("guard",
                        new Taunt("PAH! The prohibited arts allow untold power..."), // VERIFIED-2x-spec (disc_VFAOCb9EQJ4 "PAH! The prohibited arts allow untold [power]..." + disc_XQUmPL07sFY "PAH! The dark/prohibited arts... [partial]"). "power" completes the cut word; "prohibited" per disc_VFAOCb9EQJ4. (Ledger had no source_tags = matcher miss — it IS recovered, NOT designed.)
                        new Taunt(0.0016, "It is my duty to protect these catacombs... my purpose!"), // VERIFIED-spec disc_VFAOCb9EQJ4 (verbatim, 1 source)
                        new Prioritize(new StayCloseToSpawn(0.2, 5), new Wander(0.2)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 800), // PARTIAL disc_v6HXKS92zAI b002_16s f001: C-arc shaped bullets observed guard state; count:10/angle:36 cannot be validated (count/angle unresolvable at this frame)
                        new Shoot(15, projectileIndex: 1, count: 4, shootAngle: 20, predictive: 0.5, coolDown: 1400), // RECONSTRUCTED — no frame isolation of aimed spread
                        new HpLessTransition(0.3, "slipping")
                    ),
                    new State("slipping",
                        new Taunt("I feel myself... Slipping... Into the void... It is so... Dark..."), // CORRECTED disc_v6HXKS92zAI (clean WIDE read of t001_at0s chat: "Marble Colossus: I feel myself... Slipping... Into the void... It is so... Dark..." — the prior "It is soon" was a MISREAD: 2 earlier specs cut at "so[on]" and it was completed to "soon", but the full uncut line is "It is so... Dark..." [= "it is so dark"], which fits the void/slipping theme. Re-confirmed this verify fire.)
                        new Taunt(0.0016, "I...I am...Dying..."), // VERIFIED disc_gTBwT-tVZ78 (distinct death gasp; spec-documented)
                        new Flash(0xcccccc, 0.3, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 14, shootAngle: 25, coolDown: 600) // PARTIAL disc_v6HXKS92zAI b002_16s f006+f018: large round ring ~12-14 bullets expanding outward; count:14/angle:25 CONSISTENT with observed ring; slipping taunt chat-confirmed at f018
                    )
                ),
                new Threshold(0.1, new TierLoot(12, ItemType.Weapon, 0.1), new TierLoot(13, ItemType.Armor, 0.1), new TierLoot(5, ItemType.Ring, 0.12))
            )
            // Marble Defender — ROTF MARBLE GUARDIAN/PROTECTOR realm boss (object 0x8ff7; blue armored golem, grey marble
            // arena). Source disc_wsuYBdiueK0 (boss-prefix 'Marble Defender', grayscale-pull). DISTINCT from Marble Colossus
            // (above) — protector flavour. Built initially with 4 tail-CUT partials; the VERIFY LOOP then wide-re-cropped
            // f_00099 and COMPLETED all of them (and found the dev build had split ONE line into two fragments + missed a
            // line): the 4 lines below are now full frame-read text. Attack RECONSTRUCTED (no bullet geometry recovered).
            .Init("Marble Defender",  // arena: outdoor realm — large white marble tile courtyard (bright open floor, rectangular, trees at perimeter); realm boss EMBEDDED in outdoor terrain — DOCUMENTED-from-frames disc_wsuYBdiueK0 (f_00050: white marble courtyard floor + Defender entity + players fighting; minimap_00001: amber/orange = realm terrain; b002_4s f001+f004: adjacent catacomb-style dark-diamond tiles near entry). Attack: b002_4s f008 shows ~7-9 large round bullets at top perimeter (ring burst) + many small arrow/chevron bullets scattered (aimed spread), CONSISTENT with count:10/angle:36 ring and count:5/predictive:0.5 spread respectively.
                new State(
                    new State("idle", new PlayerWithinTransition(14, "guard")),
                    new State("guard",
                        new Taunt(0.0016, "I wish only to protect you. Forgive me, but I have no other choice."), // CORRECTED disc_wsuYBdiueK0 (VERIFY-LOOP wide f_ re-crop f_00099 — full line; was MISSING from the build entirely)
                        new Taunt(0.0016, "Your confidence is gravely misplaced, heroes."), // CORRECTED disc_wsuYBdiueK0 (was: "Your confidence..."; full line frame-read f_00099)
                        new Taunt(0.0016, "Do not allow this evil to escape!"), // CORRECTED disc_wsuYBdiueK0 (was: "Do not allow this..."; full line f_00099)
                        new Taunt(0.0016, "I can protect your world no longer. You are now responsible for the fate of the realm."), // CORRECTED disc_wsuYBdiueK0 (was TWO fragments "I can protect you..." + "...the fate of the realm..." = the SAME single line; merged + completed from f_00099)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.25)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 800),                              // PARTIAL disc_wsuYBdiueK0 b002_4s f008: ~7-9 large round bullets at top of frame = ring burst; count:10/angle:36 CONSISTENT
                        new Shoot(14, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1300, projectileIndex: 1) // PARTIAL disc_wsuYBdiueK0 b002_4s f008: many small arrow/chevron bullets scattered at players = aimed spread; count:5/predictive CONSISTENT
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Legon the Weather God — ROTF storm/elements god in a moated palace (object 0x8ff8). Sources disc_VZDxttR2LMk
            // (boss-coloured, clear) + disc__k2I-JV1ztQ (name first-letter cut, recovery mis-guessed "Vegan"). VERIFY LOOP
            // read the full prefix "legon the Weather God" (entry t002@281s) -> RENAMED from the placeholder "The Weather God".
            // 2 taunts VERIFIED. NOTE: the dev build's 3rd taunt + its "shrine" empower phase were REMOVED here as a
            // MIS-ATTRIBUTION — "The time is come...the elements are ME! THE SHRINE WILL GIVE ME POWER!" is spoken by a
            // SEPARATE boss "<Urios, God of Elements>" in the same RUINS (now a dev build-lead), NOT by the Weather God.
            // Attack RECONSTRUCTED (storm/elemental spread+aimed; no bullet geometry recovered). Loot lead: Ring of the Storm Gods.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_SWYl0ULzh4s f_00205: perfect RING of ~12 YELLOW/GOLD
            // THUNDER SWIRL (proj-0) bullets on dark navy dungeon floor. Ring geometry CONFIRMS count:12 shootAngle:30
            // reconstruction. Grey Missile (proj-1) aimed secondary not isolated. Arena interior: dark NAVY BLUE
            // tile floor (inside the moated-palace fortress walls — distinct from sandy exterior approach).
            .Init("Legon the Weather God",  // attacks PARTIAL disc_SWYl0ULzh4s f_00205: Thunder Swirl (proj-0, yellow/gold X-shape) ring ~12 CONFIRMED; Grey Missile (proj-1) aimed RECONSTRUCTED.
                new State(
                    new State("idle", new PlayerWithinTransition(14, "storm")),
                    new State("storm",
                        new Taunt(0.0016, "Infiltrating my palace will get you killed. How brave."), // VERIFIED disc_VZDxttR2LMk (boss-coloured entry line t002@281s, exact)
                        new Taunt(0.0016, "Time to get completely obliterated!"), // VERIFIED-2x-spec (disc_VZDxttR2LMk + disc__k2I-JV1ztQ, identical)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 700),                              // PARTIAL f_00205: Thunder Swirl ring ~12 CONFIRMED; exact count+angle RECONSTRUCTED
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // Grey Missile aimed RECONSTRUCTED
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Euryale the Snake Goddess — ROTF Gorgon-sister boss (object 0x8ff9). Euryale = the 3rd Gorgon (base game has
            // Stheno the Snake Queen + Medusa; NO base 'Euryale' -> ROTF custom). Built with 2 cut partials; the VERIFY LOOP
            // wide-re-cropped both sources and COMPLETED/CORRECTED them: disc__oNRwi83bLU f_00092 = "You will regret this for
            // many years to come." (also confirmed disc_VZDxttR2LMk f_00159 -> 2-source); and the dev build's 2nd "taunt"
            // ("You will regret this for trying my pa...") was a MIS-READ -> the real 2nd line is "This is where I make my
            // stand!" (disc_VZDxttR2LMk f_~159; recovery had garbled it as "I show I mine my star..."). Attack RECONSTRUCTED.
            .Init("Euryale the Snake Goddess",  // arena: diamond-shaped boss room — diagonal checkerboard floor tiles (alternating dark/light in diagonal grid), large dark block obstacles at 4 diamond corners, pillars/statues at perimeter — DOCUMENTED-from-frames disc__oNRwi83bLU f_00050 (25s mid-fight: white boss entity at center of diamond arena + players clustered below; scattered bright dots visible but no ring/spread geometry isolatable at 2fps); f_00055 (27.5s kill: dark screen + "Robe of Wrath" Untiered ×multiple players = Euryale's loot, NOT in OC XML); minimap_00008 (diamond room = boss chamber in multi-room dungeon). Attacks: scattered dots mid-fight not isolatable to ring count/angle; both Shoot params RECONSTRUCTED.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "coil")),
                    new State("coil",
                        new Taunt(0.0016, "You will regret this for many years to come."), // CORRECTED disc__oNRwi83bLU (was: "You will regret this for many..."; VERIFY-LOOP wide f_ re-crop f_00092 shows full line; also confirmed in disc_VZDxttR2LMk f_00159 = 2-source)
                        new Taunt(0.0016, "This is where I make my stand!"), // CORRECTED disc_VZDxttR2LMk (was a MIS-READ "You will regret this for trying my pa..."; wide f_ re-crop f_~159 shows the real 2nd line "This is where I make my stand!" — recovery had garbled it as "I show I mine my star...")
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 750),                              // serpent spread (RECONSTRUCTED)
                        new Shoot(13, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1250, projectileIndex: 1) // aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Legendary Dragon Kakashi — ROTF custom MEME boss (object 0x8ffa; realm event boss). Part of the established ROTF
            // meme-boss roster (Mewtwo / Cookie Monster / Uvuvwuwuwe / The Zuck — all built). Source disc_SWYl0ULzh4s
            // (GENUINE ROTF, creator RuinedBow; boss-coloured '<Legendary Dragon Kakashi>' prefix). 2 CLEAN VERBATIM taunts
            // (frame-read b008_128s_008 wide grayscale-pull). Attack RECONSTRUCTED; Kakashi-specific bullets not isolatable
            // from b008 (clean radial rings there were the adjacent Void Entity's). Arena: OPEN REALM (confirmed b008_128s f008:
            // gray realm terrain + round shrub decorations adjacent to void-tile border; minimap_00025: empty realm minimap).
            // NOTE: disc_SWYl0ULzh4s also has Legon fight footage at ~100s (f_00200 + b008_128s f018: X/star bullets visible
            // in Legon's moated-palace arena) — this video is a NEW Legon source for future attack recovery.
            .Init("Legendary Dragon Kakashi",  // arena: open realm event boss — gray realm terrain with round dark shrub decorations; dark terrain patches (possible lava tiles) in fight area; realm minimap (no dungeon) — DOCUMENTED-from-frames disc_SWYl0ULzh4s b008_128s f008 (taunt "YOU SHOULD HAVE NEVER DONE THIS!" confirmed + realm terrain visible; void-tile area adjacent = near Void dungeon portal); minimap_00025 (near-empty realm minimap). Attacks: Kakashi-specific bullets not isolatable (adjacent Void Entity's rings mis-attributed in b008); both Shoot params RECONSTRUCTED.
                new State(
                    new State("idle", new PlayerWithinTransition(14, "rage")),
                    new State("rage",
                        new Taunt(0.0016, "YOU SHOULD HAVE NEVER DONE THIS!"), // VERIFIED disc_SWYl0ULzh4s (VERIFY-LOOP wide re-crop b008_128s_007-009, "<Legendary Dragon Kakashi>" prefix, exact)
                        new Taunt(0.0016, "YOU'VE MADE A GRAVE MISTAKE!"), // VERIFIED disc_SWYl0ULzh4s (VERIFY-LOOP wide re-crop b008_128s_007-009, exact)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 700),                              // lava scatter (RECONSTRUCTED)
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Masha, the God of War — ROTF custom war-god boss (object 0x8ffb). Source disc_tET7e3TRTc8 (GENUINE ROTF,
            // boss-coloured 'Masha, the God of War:'). The recovery only had chat-crop-cut partials ("How did you get..." /
            // "Stupid mortals..."); the dev loop full-frame-read f_00132 (the narrow 450px capture's FULL frame shows beyond
            // the chat crop) -> built the COMPLETE lines. References lore 'Asclepius'. Attack RECONSTRUCTED (no geometry).
            .Init("Masha, the God of War",
                new State(
                    new State("idle", new PlayerWithinTransition(14, "war")),
                    new State("war",
                        new Taunt(0.0016, "How did you get here!? Does that mean Asclepius has failed...?"), // VERIFIED disc_tET7e3TRTc8 (VERIFY-LOOP re-read f_00128-144, wording exact; trailing "..?" -> "...?")
                        new Taunt(0.0016, "Stupid mortals... You really think you can kill me?"), // VERIFIED disc_tET7e3TRTc8 (VERIFY-LOOP re-read f_00128-144, exact)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 700),                              // war spread (RECONSTRUCTED)
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Resthla, The Twisted Mermaid — ROTF custom boss, stone-block dungeon corridor (object 0x8ffc; family/sibling-bitterness
            // theme — "Mother always preferred HER"). Source disc_VZDxttR2LMk (GENUINE ROTF, boss-coloured prefix). NAME CORRECTED
            // from queue mis-read "The Twisted Memory" -> f_00025 wide re-crop confirms "<Resthla, The Twisted Mermaid>". Both
            // taunts VERIFIED f_00025 (12.5s). Loot: "An Untiered item: 'Three Tiny S[?]...'" partially visible f_00025 — NOT in
            // OC XML, name truncated; not added to Threshold. Attack RECONSTRUCTED (f_00010 shows chains+nova but attribution
            // uncertain — marathon-stream prev boss killed at 2s; f_00025 no geometry visible).
            .Init("Resthla, The Twisted Mermaid",  // arena: BLUE TEARDROP boss chamber — blue/water tile floor in distinctive teardrop/droplet shape (pointed top, rounded bottom); minimap matches teardrop. BOSS CHAMBER CONFIRMED disc_VZDxttR2LMk f_00280 (post-kill, 2x "Coral Bow" Untiered loot drops = ocean/mermaid themed). Dungeon approach: narrow rectangular stone-block corridor — dark rounded stone block walls, dark textured floor with oval water pool markings (f_00025 + minimap_00005: L-shaped corridor network, rectangular boss arm). Loot: "Coral Bow" (Untiered, f_00280) + "Three Tiny S[?]..." (Untiered, f_00025, truncated — NOT in OC XML).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "lament")),
                    new State("lament",
                        new Taunt(0.0016, "She is... Silly."), // CORRECTED disc_VZDxttR2LMk (was: "Maria... Joh..."; VERIFY-LOOP wide re-crop f_00023 shows the boss's actual 1st line is "<Resthla, The Twisted Mermaid> She is... Silly." — "Maria... Joh..." not locatable in footage, was a mis-read)
                        new Taunt(0.0016, "Mother always preferred her. Mother always was a fool. Bah."), // VERIFIED disc_VZDxttR2LMk (wide re-crop f_00023, wording exact; single-letter spelling "preferred/prefered" sub-pixel-ambiguous -> kept standard)
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.3)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 750),                              // watery scatter (RECONSTRUCTED)
                        new Shoot(13, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1250, projectileIndex: 1) // aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Ascended Septavius the Ghost Goddess — ROTF ASCENDED/gender-flipped custom variant of the base 'Septavius the
            // Ghost God' (object 0x8ffd). Source disc_VZDxttR2LMk (GENUINE ROTF, boss-coloured). NAME CORRECTED from queue
            // mis-read 'Sepiseus' -> 'Septavius'. Dungeon has a guardian pre-boss phase (taunts "CONSUME!" / "FOOLS! YOU DO
            // NOT UNDERSTAND!" / "I tried to protect you... I have failed. You release a great [darkness]" — NOT Septavius).
            // Entry taunts VERIFIED f_00168. Mid-fight taunts discovered b002_025/f_00230: "kill my ancient guardian, kill
            // my pets, now I will take your life." (clean read) + "y followers... Take care of these fools." (left-cut, kept
            // partial).
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_VZDxttR2LMk f_00210 (~83% HP): dense SIMULTANEOUS ring of
            // THUNDER SWIRL (proj-0, yellow/gold X-shape) + DEMON BLADE (proj-1, red/orange blade) bullets scattered
            // across ghost-circle tile arena. Both projectile types CONFIRMED in same frame. Exact count/angle RECONSTRUCTED.
            // Boss displays as "????" (name hidden/ghost mechanic) during fight — confirmed same ghost-circle arena.
            .Init("Ascended Septavius the Ghost Goddess",  // attacks PARTIAL disc_VZDxttR2LMk f_00210: Thunder Swirl (proj-0, yellow/gold) + Demon Blade (proj-1, red/orange) simultaneous ring CONFIRMED. Arena: ghost-circle tiles (small square, O/dot motif) — DOCUMENTED disc_VZDxttR2LMk f_00220/f_00230.
                new State(
                    new State("idle", new PlayerWithinTransition(14, "haunt")),
                    new State("haunt",
                        new Taunt(0.0016, "You'll regret what you did to my king."), // VERIFIED disc_VZDxttR2LMk f_00168 (entry taunt 1)
                        new Taunt(0.0016, "Prepare yourselves, and face me!"), // VERIFIED disc_VZDxttR2LMk f_00168 (entry taunt 2)
                        new Taunt(0.0016, "kill my ancient guardian, kill my pets, now I will take your life."), // DOCUMENTED-from-frames disc_VZDxttR2LMk b002_025 + f_00230 (mid-fight, clean read)
                        new Taunt(0.0016, "...y followers... Take care of these fools."), // PARTIAL (left-cut) disc_VZDxttR2LMk b002_025 — start of line not visible
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 700),                              // PARTIAL f_00210: Thunder Swirl (proj-0, yellow/gold) ring CONFIRMED; count:12 RECONSTRUCTED
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // PARTIAL f_00210: Demon Blade (proj-1, red/orange) CONFIRMED; count:5 + predictive RECONSTRUCTED
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Infused Elven King — ROTF custom boss, green Elven forest (object 0x8ffe; 'Infused' = empowered/epic variant).
            // Source disc_iBUg7V_Eq0A (GENUINE ROTF, boss-coloured, 1 verbatim taunt; death-feed names an 'Elven Captain'
            // lieutenant). Distinct from base 'Night Elf King' (0x64f). Attack RECONSTRUCTED (no bullet geometry recovered).
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_iBUg7V_Eq0A f_00195/f_00200: green CRESCENT/LEAF-SHAPED projectiles
            // in a FULL RING (360°) pattern CONFIRMED. ~16-20 evenly-spaced crescent bullets covering complete circle in dark
            // green forest arena. Ring pattern is INCONSISTENT with current Shoot(14,count:12,shootAngle:30) — that would be a
            // narrow 30° fan, not a ring. TRUE ring attack: count:~16-20, shootAngle:360 (or fixedAngle cycling). Shoot()
            // params left as-is pending ring-geometry re-pass; SHAPE (green crescent/leaf) and PATTERN (full ring) CONFIRMED.
            // TAUNT CORRECTED: f_00195/200 clearly reads "unlocked, you shall be my first victims." (not "unleashed" from
            // prior recovery — both words end similarly; "unlocked" is the verbatim frame-read). attacks_status: no-footage → partial.
            .Init("Infused Elven King",  // attacks PARTIAL disc_iBUg7V_Eq0A f_00195/200: green CRESCENT/LEAF bullets in FULL RING CONFIRMED; count:~16-20/angle:360 INCONSISTENT with current Shoot params → ring-geometry re-pass needed. Arena: Elven Forest dungeon — dark green forest floor, blue water patches, tree trunks — DOCUMENTED f_00240 + minimap_00025/030
                new State(
                    new State("idle", new PlayerWithinTransition(14, "unleash")),
                    new State("unleash",
                        new Taunt(0.0016, "Finally my true power has been unlocked, you shall be my first victims."), // CORRECTED disc_iBUg7V_Eq0A f_00195/f_00200 (boss-colored chat, readable: "...unlocked, you shall be my first victims." — prior read had "unleashed" which misread "unlocked"; f_00195 shows full line clearly including second clause)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 24, shootAngle: 360, coolDown: 700),                             // green crescent/leaf RING: shape PARTIAL f_00195/200; count:24 approx (arc ~7-8/quarter = ~28-32, mid-est 24); shootAngle:360 CONFIRMED (full ring); fixedAngle cycling likely
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // aimed (RECONSTRUCTED — not isolated in ring-pattern frames)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // White Dragon Lord — ROTF elemental DRAGON LORD (ice/air; object 0x8fff). Part of the built elemental dragon-lord
            // roster (Firebreather=fire 0x8f80, Riverborn=water 0x8f90). Source disc_iIkWGzE1404 (GENUINE ROTF, boss-coloured,
            // DOCUMENTED-VERBATIM taunt). Distinct from the base White Dragon family (Whelp/Juvenile/Adult). Attack RECONSTRUCTED.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_iIkWGzE1404 f_00100/f_00105: blue ELONGATED TEARDROP/ICE-SHARD
            // projectiles CONFIRMED in sandy dungeon arena. f_00100: wide scatter of blue bullets around huge white boss on sandy
            // floor. f_00105: multiple blue elongated bullets spread across sandy tile floor. Bullet shape: elongated teardrop,
            // light blue (ice/frost element). Count:12/angle:30 RECONSTRUCTED (distribution consistent with wide frost spread).
            // attacks_status upgraded no-footage → partial.
            .Init("White Dragon Lord",  // attacks PARTIAL disc_iIkWGzE1404 f_00100/f_00105: blue elongated teardrop/ice-shard bullets CONFIRMED; count:12/angle:30 RECONSTRUCTED. Arena: sandy/desert dungeon — oval/circular boss chamber, sandy/tan large square tile floor; DOCUMENTED f_00100 + minimap_00080
                new State(
                    new State("idle", new PlayerWithinTransition(14, "roar")),
                    new State("roar",
                        new Taunt(0.0016, "Impudence! I am an immortal Dragon!"), // UNVERIFIABLE-illegible disc_iIkWGzE1404 — VERIFY LOOP could not independently re-isolate the exact boss-coloured line: 1405-chat/936-f_/35-transition MARATHON, NO bossends, and the boss was admin /spawn'd in the open (no dungeon-entry to localize). Recovery's read was HIGH-confidence DOCUMENTED-VERBATIM + the boss is canonically validated (elemental Dragon Lord roster: built Firebreather/Riverborn). NO error evidence; text kept as recovery's read (NOT invented/guessed) — just not re-confirmable in a small-work budget. (Re-isolation limitation, not a doubt about the read.)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 700),                              // blue ice-shard teardrop bullet shape PARTIAL f_00100/f_00105; count:12/angle:30 RECONSTRUCTED
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // aimed (RECONSTRUCTED — not isolated)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Suspicious Looking Dragon — well-attested recurring ROTF EVENT boss (object 0x8f14, GAP id — 0x8fd0-0x8fff full).
            // Named in 10+ recovery specs + event feeds ("The suspicious-looking Dragon defeated"). Earth/tremor theme. Taunt
            // is a 2x-corroborated READABLE FRAGMENT (cut both ends), disc_p-f3qDjrVDY + disc_XGl3pTaNmTM -> kept verbatim
            // (NOT guessed), flagged for verify wide-re-crop to complete. Attack RECONSTRUCTED (no bullet geometry recovered).
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_XGl3pTaNmTM f_00079: dense WHITE/SILVER
            // ring of GREY MISSILE (proj-0, grey elongated — appears silver at video scale) bullets
            // scattered around boss in open green-grass realm. Wide radial ring pattern CONFIRMED;
            // count:12 shootAngle:30 RECONSTRUCTED (distribution consistent). Demon Blade (proj-1)
            // NOT isolated in footage. SLD fights in open realm, no dungeon walls.
            .Init("Suspicious Looking Dragon",  // attacks PARTIAL disc_XGl3pTaNmTM f_00079: Grey Missile (proj-0, white/silver) ring CONFIRMED; count:12 RECONSTRUCTED. Arena: open REALM event — green grass biome, mushrooms+trees, no boundary walls. DOCUMENTED f_00079-082 + minimap_00010.
                new State(
                    new State("idle", new PlayerWithinTransition(14, "tremor")),
                    new State("tremor",
                        new Taunt(0.0016, "...vibrations from deep down below..."), // VERIFIED-partial (2x cross-spec) disc_p-f3qDjrVDY + disc_XGl3pTaNmTM
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 700),                              // PARTIAL f_00079: Grey Missile (proj-0, white/silver) wide ring CONFIRMED; count:12 RECONSTRUCTED
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // Demon Blade (proj-1) — RECONSTRUCTED, not isolated
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Putin — ROTF custom MEME boss (object 0x8f17, GAP id; red-arena), part of the heritage-meme roster (Mewtwo /
            // Cookie Monster / The Zuck / Legendary Dragon Kakashi / Uvuvwuwuwe — all built). Source disc_hGSSP2J3gcQ
            // (boss-coloured 'Putin', CLEAN complete taunt). Map DOCUMENTED minimap_00008/00012. Bossend group b002_27s
            // heavily contaminated (wide extraction window; b002_016 shows dark grass arena + "nd> Puny mortals! My 56253 HP
            // will annihilate you!" + "Can't... keep... henchmen...alive... ARGHHH!!" — boss prefix "nd>" unresolvable, may
            // be different boss in same video; taunts/HP NOT added, attribution uncertain). Attack RECONSTRUCTED.
            .Init("Putin",  // arena: large oval/circular red-grass boss chamber — organic dark (red in-game) floor tiles, no obstacles; accessed via narrow entrance corridor from small starting antechamber — DOCUMENTED-from-frames disc_hGSSP2J3gcQ minimap_00008 (large oval boss chamber with players clustered), minimap_00012 (oval room + narrow entrance corridor + small starting room visible). b002_001: realm gathering + dungeon portal exterior. Attacks: no geometry isolatable — both Shoot params RECONSTRUCTED.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt(0.0016, "MOTHER RUSSIA!!!"), // SOURCED disc_hGSSP2J3gcQ (boss-coloured, clean complete; verify loop can re-confirm)
                        new Prioritize(new Follow(0.6, 12, 2), new Wander(0.4)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 750),                              // radial (RECONSTRUCTED)
                        new Shoot(13, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08))
            )
            // Wind Elemental — ROTF REALM-EVENT boss (object 0x8f18, GAP id). Source disc_E03Je5EgiZQ: DOCUMENTED-from-chat
            // ("Wind Elemental spawned in Realm!" + /30 event counter + death-feed "(10/30) Wind Elemental has been defeated"
            // also seen in disc_8gMVtvs5CVs). PROVENANCE NUANCE: src disc_8gMVtvs5CVs groups it with base/event spawns
            // (Pentaract/Skull Shrine) as "event-class, not ROTF-custom" — but it does NOT exist in our server XML/behaviors,
            // so this is a faithful ADD of a genuine ROTF realm spawn, not a duplicate. Taunt "Tornadoes! Help me!" is the one
            // DOCUMENTED-from-chat line. No attack geometry captured -> generic radial+aimed mix (RECONSTRUCTED).
            .Init("Wind Elemental",  // arena: open realm — mixed dark stone realm tiles and wavy grass tile patches, large round boulder/stone obstacles scattered throughout; no boundary walls (true open-realm event). DOCUMENTED-from-frames disc_E03Je5EgiZQ b003_169s f_005 (taunts + terrain visible at kill time); minimap_00010 (amber realm pocket, small cluster), minimap_00020 (amber organic irregular realm area, players at kill point). Attacks: crowded kill frame (8+ players) — no bullet geometry isolatable → UNVERIFIABLE-ILLEGIBLE.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "storm")),
                    new State("storm",
                        new Taunt(0.0016, "Tornadoes! Help me!"), // VERIFIED disc_E03Je5EgiZQ (frame-read chat_00048/00047, boss-coloured <Wind Elemental> prefix, exact; corroborated by "Wind Elemental spawned in Realm!" announce + <Gorgon> "(16/30) Wind Elemental has been defeated" feed)
                        new Taunt(0.0016, "You will test the power of the wind!"), // DOCUMENTED-from-frames disc_E03Je5EgiZQ b003_169s f_005 (chat at kill, boss-coloured, clean read)
                        new Prioritize(new Follow(0.6, 12, 2), new Wander(0.4)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 700),                              // radial nova (RECONSTRUCTED — wind theme)
                        new Shoot(13, count: 4, shootAngle: 14, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // aimed gust (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08))
            )
            // Hades — NEW ROTF boss (object 0x8f1c, GAP id). Source disc_qyTuKUoeiso (client banner "VotR 2.0 Beta" = ROTF target
            // server; player VoltriPlay). Taunt is DOCUMENTED-from-chat VERBATIM (boss-coloured "<Hades>", recovered via wide f_00187
            // re-crop — the 224px chat crop cut the tail at "GIVE ME", the wide crop gave the full line). A soul/energy underworld boss:
            // the line is his rage when his soul-energy source is destroyed (low-HP/desperation). Not base-game, not a dup; clean
            // collision. Attack geometry NOT captured -> generic radial+aimed mix (RECONSTRUCTED).
            .Init("Hades",
                new State(
                    new State("idle", new PlayerWithinTransition(14, "fight")),
                    new State("fight",
                        new Prioritize(new Follow(0.7, 12, 2), new Wander(0.4)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 650),                              // radial nova (RECONSTRUCTED)
                        new Shoot(14, count: 6, shootAngle: 16, predictive: 0.5, coolDown: 1100, projectileIndex: 1), // aimed (RECONSTRUCTED)
                        new HpLessTransition(0.2, "drained")
                    ),
                    // low-HP rage: his soul-energy source destroyed (the recovered line)
                    new State("drained",
                        new Flash(0xff00ddaa, 0.3, 10),
                        new Taunt("What!? The SOULS that GIVE ME ENERGY ARE GONE! ARAAAGAAAGH!"), // VERIFIED disc_qyTuKUoeiso (DOCUMENTED-from-chat VERBATIM; wide f_00187 re-crop, boss-coloured <Hades>, full line)
                        new Prioritize(new Follow(0.8, 12, 2), new Wander(0.5)),
                        new Shoot(14, count: 16, shootAngle: 22, coolDown: 500),                              // enraged dense radial (RECONSTRUCTED)
                        new Shoot(14, count: 6, shootAngle: 15, predictive: 0.6, coolDown: 900, projectileIndex: 1) // aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Asylum Guard — ROTF "Asylum"-dungeon boss-coloured entity (object 0x8f1b, GAP id). Source disc_ONFXatmUVNI (RuinedBow
            // clip). DEV-fire FRAME-CONFIRMED the name + line at chat_00014: boss-coloured "<Asylum Guard>" prefix + the squawk taunt
            // "Ark! aark!.." (a crow-squawk — quirky but the genuine on-screen line, NOT fabricated; this upgrades the spec's single-read
            // MEDIUM-confidence name to frame-confirmed). MECH-LOOP: "You will ALL die here!" added (b002_50s f_010 speech bubble, clean
            // read). Map DOCUMENTED: light hexagonal honeycomb tile floor (f_00105 post-kill scatter + b002 fight frames). Attacks:
            // curved/arc projectiles visible in f_00095 (quest-arrow frame mid-fight) — count+angle unreadable → UNVERIFIABLE-ILLEGIBLE.
            // NOTE: chat_00016 shows a SECOND Asylum entity "<AsylumLootEye>" with luring taunts ("Come closer!..", etc.) — separate
            // entity, not added here; flag for a future spec entry.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_ONFXatmUVNI f_00048/f_00095: small GREY MISSILE
            // (proj-0, grey elongated) projectiles visible in scattered radial spread around boss. Count/angle
            // not precisely countable but radial scatter confirms multi-shot spread. Thunder Swirl (proj-1) aimed
            // secondary RECONSTRUCTED (not isolated).
            .Init("Asylum Guard",  // arena: Asylum-dungeon boss chamber — light hexagonal (honeycomb) tile floor, no obstacles; dark grey stone corridor network on minimap. attacks PARTIAL: Grey Missile (proj-0, grey) scatter CONFIRMED f_00048/f_00095.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt(0.0016, "Ark! aark!.."), // VERIFIED disc_ONFXatmUVNI (chat_00014, boss-coloured "<Asylum Guard>" prefix; genuine crow-squawk line)
                        new Taunt(0.0016, "You will ALL die here!"), // DOCUMENTED-from-frames disc_ONFXatmUVNI b002_50s f_010 (speech bubble)
                        new Prioritize(new Follow(0.6, 12, 2), new Wander(0.4)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 800),                              // PARTIAL f_00048/f_00095: Grey Missile (proj-0) radial scatter CONFIRMED; count:10 shootAngle:36 RECONSTRUCTED
                        new Shoot(13, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1300, projectileIndex: 1) // Thunder Swirl (proj-1) aimed secondary RECONSTRUCTED
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08))
            )
            // Bloodsworn Knight — NEW ROTF GALACTIC boss (object 0x8f1a, GAP id) from the "Ruins of the Mad God" space zone
            // (Phase D3 Galactic-Zones content). Source disc_l8s6Dho3WM4 (owner Volti present; HIGH-confidence chat). DEV-fire
            // frame-read 4 boss-coloured <Bloodsworn Knight> taunts: "The gate opens..." + "My watch... has... ended..." are CLEAN
            // (frame chat_00056); "You dare challenge the Bloodsworn..." + "The gate is sealed! Prepare..." are right-edge CUT (chat
            // chat_00050/52) -> kept readable clause + ellipsis, NO baked-in "[Knight/Order]" guess. Genuinely-new (not base-game,
            // not a dup; clean collision). Placed in Mountains pool as a holding spot until Phase D3 Galactic Zones is built.
            // Attack geometry NOT captured -> generic radial+aimed mix (RECONSTRUCTED).
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_l8s6Dho3WM4 f_00195/f_00198/f_00200: dense red CROSS/PLUS-shaped
            // projectiles confirmed in wide radial scatter covering full arena (~25-30 bullets visible per frame at 7-10% boss HP).
            // Cross/plus sprite clearly identifiable; count:10 shootAngle:36 remain RECONSTRUCTED. Taunts also confirmed in f_00195
            // ("You dare challenge the Bloodsworn?") — corroborates bossend chat_00050/52/56 reads. Map confirmed: dark stone tiles.
            .Init("Bloodsworn Knight",
                new State(
                    new State("idle", new PlayerWithinTransition(14, "fight")),
                    new State("fight",
                        new Taunt(0.0016, "You dare challenge the Bloodsworn..."), // VERIFIED-partial disc_l8s6Dho3WM4 (chat_00050, boss-coloured; right-edge cut after "Bloodsworn" — recovery's "[Knight/Order]" guess NOT baked in)
                        new Taunt(0.0016, "The gate is sealed! Prepare..."),       // VERIFIED-partial disc_l8s6Dho3WM4 (chat_00052; tail cut after "Prepare")
                        new Taunt(0.0016, "The gate opens..."),                    // VERIFIED disc_l8s6Dho3WM4 (chat_00056, boss-coloured, clean)
                        new Prioritize(new Follow(0.6, 12, 2), new Wander(0.4)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 700),                              // radial — cross/plus bullet shape PARTIAL f_00198/f_00200; count:10/angle:36 RECONSTRUCTED
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1), // aimed (RECONSTRUCTED — not isolatable from f_ frames)
                        new HpLessTransition(0.15, "end")
                    ),
                    new State("end",
                        new Taunt("My watch... has... ended..."), // VERIFIED disc_l8s6Dho3WM4 (chat_00056, boss-coloured, clean death line)
                        new Prioritize(new Follow(0.6, 12, 2), new Wander(0.4)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 650)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08))
            )
            // Wizzard Clone — ROTF clone-type realm boss (object 0x8f19, GAP id). Source disc_EM9Uba-Nv08: DOCUMENTED-from-chat
            // (orange boss-coloured "Wizzard Clone", SIC on-screen spelling). PROVENANCE NUANCE: its taunt "I am everywhere and
            // nowhere!" is the SHARED clone-boss line already VERIFIED on Crystal Prisoner Clone (BehaviorDb.Crystal.cs, frame-read
            // disc_LuUwzl6KwCY). The recovery analyst flagged this as CROSS-CORROBORATION (ROTF clone bosses share this catchphrase),
            // NOT a mis-read — "Wizzard Clone" is a separately-named boss-coloured entity, consistent with the existing clone roster
            // (Crystal Prisoner Clone / Nyx Clone). So this is a faithful ADD (not a duplicate id; clean collision check). The taunt is
            // honestly a shared/borrowed clone line, not a unique-to-this-boss recovery. Attack RECONSTRUCTED (clone-confusion -> radial+aimed).
            .Init("Wizzard Clone",  // arena: dark dungeon — near-black floor tiles (very dark gray, subtle texture), dark stone walls; irregular organic cave-shaped room (no straight walls). DOCUMENTED-from-frames disc_EM9Uba-Nv08 f_00094/095 (Wizzard Clone taunt confirmed in orange boss-colour + floor/wall visible) + minimap_00010/00013 (irregular cave-room shape, orange/amber dungeon interior, player cluster bright yellow dots). disc_LuUwzl6KwCY minimap unusable (PROD STUFF overlay).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "clones")),
                    new State("clones",
                        new Taunt(0.0016, "I am everywhere and nowhere!"), // VERIFIED disc_EM9Uba-Nv08 (wide f_ re-crop f_00094/95: orange boss-coloured "<Wizzard Clone>", sic, exact verbatim incl "!"; SAME frame-set also shows "<Crystal Prisoner Clone>" saying the identical line in another biome -> proves distinct entity + SHARED clone-boss taunt, not a mis-read)
                        new Prioritize(new Follow(0.6, 12, 2), new Wander(0.4)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 750),                              // radial (RECONSTRUCTED — clone-confusion)
                        new Shoot(13, count: 4, shootAngle: 14, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08))
            )
            // The Beekeeper — ROTF insect/swarm-summoner boss (object 0x8f16, GAP id). Source disc_1mU5uHE6DXE (highlights
            // montage; boss-coloured + death-feed "killed by The Beekeeper"). NAME CORRECTED from the queue's mis-read "The
            // Passenger" -> dev frame-read f_00040 clearly shows "<The Beekeeper>" + insect taunts. Summons "insect children"
            // (dark round minion blobs visible b002_10s f_005; worm-chain summon visible b002_10s f_001). MECH-LOOP: Map
            // MECH-LOOP 2026-06-24: arena CORRECTED — disc_1mU5uHE6DXE f_00045: HONEYCOMB dungeon (yellow/amber
            // hexagonal tile floor, orange-brown walls). Prior "bubbly stone tile floor" was WRONG (was reading
            // death-screen tile background, not actual dungeon). Bee minions (purple winged sprite) visible swarming
            // during fight — confirm "insect children" mechanic. Attacks UNVERIFIABLE-ILLEGIBLE — grey missiles
            // (proj-0) and demon blades (proj-1) not isolatable from minion sprites in crowded honeycomb arena.
            .Init("The Beekeeper",  // arena: HONEYCOMB dungeon — yellow/amber hexagonal tile floor, orange-brown walls; CORRECTED disc_1mU5uHE6DXE f_00045 (honeycomb hex tiles + bee minions swarming).
                new State(
                    new State("idle", new PlayerWithinTransition(14, "swarm")),
                    new State("swarm",
                        new Taunt(0.0016, "You... You will not stop me again!"), // VERIFIED disc_1mU5uHE6DXE f_00045 (chat: "<The Beekeeper> You... You will not stop me again!" — new wake/fight taunt)
                        new Taunt(0.0016, "Enough! Come forth my insect children, and make them beg for death!"), // VERIFIED disc_1mU5uHE6DXE f_00040/f_00045 (chat: "<The Beekeeper>" prefix; "make them beg for death!" confirmed both frames)
                        new Taunt(0.0016, "I am the creator of a perfect insect race! You WILL NOT STAND BEFORE ME RIGHT NOW!"), // CORRECTED disc_1mU5uHE6DXE f_00045 (speech bubble: "WILL NOT STAND BEFORE ME"; prior "WILL kneel" was misread)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 16, shootAngle: 22, coolDown: 650),                              // dense swarm spread (RECONSTRUCTED)
                        new Shoot(14, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1) // aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Urios, God of Elements — ROTF elemental boss in "the RUINS" (galactic/ruins zone; object 0x8f15, GAP id).
            // Source disc_VZDxttR2LMk. BOTH taunts were FRAME-READ by me during the Legon-the-Weather-God verify
            // (shrine_a.png, f_~280-300) — and the SHRINE-empower phase that was MIS-attributed to the Weather God (removed
            // in that verify) genuinely belongs to THIS boss. So the empower phase is DOCUMENTED-from-chat, not invented.
            // Attack geometry RECONSTRUCTED (elemental nova+aimed).
            // MECH-LOOP: Map DOCUMENTED: ghost-circle-pattern tile dungeon (dark gray tiles with repeating circular motif,
            // same family as Septavius ghost dungeon), large open dark central boss chamber with 4 shrine/altar objects at
            // approximately cardinal positions around the room — directly confirms the shrine-empower phase mechanic.
            // Attacks: only 1-2 bullet shapes visible in f_00276 (fight frame ~138s) — count/angle not determinable →
            // UNVERIFIABLE-ILLEGIBLE. Source: disc_VZDxttR2LMk f_00276 (138s: arena + taunts); minimap_00034 (gray dungeon).
            .Init("Urios, God of Elements",  // arena: RUINS dungeon boss chamber — dark gray ghost-circle-pattern tile floor (circular motif tiles, Septavius-family), large open room with 4 shrine/altar objects placed at approximately cardinal positions around the central dark combat zone; gray dungeon on minimap. DOCUMENTED-from-frames disc_VZDxttR2LMk f_00276 (138s: fight frame showing arena floor + 4 shrines + both taunts in chat); minimap_00034 (gray dungeon, rectangular room layout).
                new State(
                    new State("idle", new PlayerWithinTransition(14, "elements")),
                    new State("elements",
                        new Taunt(0.0016, "THE POWER EMERGES WITHIN ME. PREPARE TO BECOME ONLY ANOTHER BLOODSTAIN IN THE RUINS!"), // VERIFIED disc_VZDxttR2LMk (VERIFY-LOOP re-read f_00278, "<Urios, God of Elements>" prefix, exact)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 700),                              // elemental spread (RECONSTRUCTED)
                        new Shoot(13, count: 5, shootAngle: 16, predictive: 0.5, coolDown: 1200, projectileIndex: 1), // aimed (RECONSTRUCTED)
                        new HpLessTransition(0.5, "shrine")
                    ),
                    // SHRINE-empower phase (DOCUMENTED-from-chat: "...THE SHRINE WILL GIVE ME POWER!") — genuinely Urios's
                    new State("shrine",
                        new Taunt(0.0016, "The time is come...the elements are ME! THE SHRINE WILL GIVE ME POWER!"), // VERIFIED disc_VZDxttR2LMk (VERIFY-LOOP re-read f_00278-281, "<Urios, God of Elements>" prefix, exact — confirms the shrine phase belongs to Urios, not the Weather God)
                        new Flash(0xff66ccff, 0.3, 10),
                        new Prioritize(new StayCloseToSpawn(0.2, 5), new Wander(0.3)),
                        new Shoot(13, count: 18, shootAngle: 20, coolDown: 600),                              // empowered elemental nova (RECONSTRUCTED)
                        new Shoot(13, count: 8, shootAngle: 14, predictive: 0.6, coolDown: 900, projectileIndex: 1) // empowered aimed (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Lycaon — werewolf (taunts confirmed disc_XQUmPL07sFY/disc_Yx0kTFEixbA)
            // MECH-LOOP 2026-06-24: attacks PARTIAL — fight observed disc_Yx0kTFEixbA f_00022-f_00025 (Lycaon 2% HP);
            // small scattered projectiles visible in spread pattern; bullet shape/color unresolvable at video resolution.
            // Existing Shoot(3, count:7, shootAngle:18) CONSISTENT with observed spread burst; no additional attack phases found.
            // UT drop CONFIRMED: Lycaon's Hide item tooltip visible f_00024 (+40 Defense/+40 Dexterity; not usable by Warrior).
            .Init("Lycaon",  // arena: dark stone dungeon — large boss chamber; floor: dark gray/charcoal stone tiles with regular square grid pattern; thick dark stone walls; orange/red fire/lava glow effects visible on floor near boss. DOCUMENTED-from-frames disc_koXrDxguF7o f_00595-605 (boss name "Lycaon" + "FLESH AND BLOOD!" taunt visible + arena floor/walls confirmed; huge crimson werewolf entity) + disc_Yx0kTFEixbA f_00022-f_00025 (CORROBORATES: same dark charcoal stone, purple walls, large chamber).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "hunt")),
                    new State("hunt",
                        new Taunt("FLESH AND BLOOD!"), // VERIFIED-3x (disc_Yx0kTFEixbA chat f_00022 frame-read + disc_XQUmPL07sFY + disc_koXrDxguF7o)
                        new Taunt(0.0016, "FRESH MEAT!"), // VERIFIED-2x (disc_Yx0kTFEixbA chat f_00025 frame-read + disc_XQUmPL07sFY)
                        new Prioritize(new Follow(0.85, 13, 1), new Wander(0.4)),
                        // PARTIAL (disc_Yx0kTFEixbA f_00022-f_00025): small spread projectiles observed at 2% HP;
                        // bullet shape unresolvable at video resolution; count:7 shootAngle:18 RECONSTRUCTED.
                        new Shoot(3, count: 7, shootAngle: 18, coolDown: 700)
                    )
                ),
                new Threshold(0.1,
                    new ItemLoot("Lycaon's Hide", 0.05), // UT leather armor — f_00024 tooltip: +40 Def/+40 Dex, not usable by Warrior
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1)
                )
            )
            // Arcanica — mana-vampire (3rd-person taunts; shots Quiet = mana-drain analog)
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_Kei9ksQxGl4 f_01665 (Arcanica 25% HP, active fight):
            // large PINK/MAGENTA CROSS/STAR-shaped projectiles clearly visible (~6-8 large X-shapes radiating
            // outward from boss) + smaller pink dots scattered. Bullet shape = distinctive large cross-star
            // (arcane/mana energy aesthetic). Proj-14 → pink cross sprite CONFIRMED. count:8 shootAngle:45
            // RECONSTRUCTED. Note: "ANGEL'S TOUCH" player ability also visible — large X-shapes are boss bullets
            // (outward trajectory from boss, not player); smaller dots may be secondary or player shots.
            .Init("Arcanica",  // arena: mixed-element dungeon — floor has distinct blue/icy crystalline tiles (light blue-gray small squares with ice-crystal texture) AND orange/red lava tile sections; dark cave/stone walls. Boss entity: large dark navy/blue body. DOCUMENTED-from-frames disc_Kei9ksQxGl4 f_01665-01670 (boss name "Arcanica" + HP bar visible + multiple taunts in chat + arena floor/walls confirmed). Extracted minimap_01245-01255 only captured edge slivers — in-game minimap in f_01665 shows dungeon room layout but too small to transcribe. NOTE: prior recovery comment "surfaced lava" is confirmed — lava tiles ARE present alongside the blue icy floor tiles.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "drain")),
                    new State("drain",
                        new Taunt("Arcanica wants mana"), // SOURCED-1-spec disc_Kei9ksQxGl4 (consistent mana-hungry 3rd-person voice; spec-documented, not frame-re-isolated this pass — the 4709+2905-crop tags surfaced lava/Spaceship/lootbox, not the Arcanica frames)
                        new Taunt(0.0016, "Arcanica sees mana source"), // SOURCED-1-spec disc_Kei9ksQxGl4
                        new Taunt(0.0016, "Why... Arcanica needs mana"), // VERIFIED disc_XQUmPL07sFY (frame-read "Arcanica: Why... Arcanica needs m[ana]" during the Lycaon pass) + spec disc_Kei9ksQxGl4 — confirms the boss + this line
                        new Taunt(0.0016, "Arcanica sensing mana..."), // SOURCED-1-spec disc_VFAOCb9EQJ4 (consistent voice; spec-documented, not frame-re-isolated this pass)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        // PARTIAL (f_01665): large PINK/MAGENTA CROSS/STAR bullets radiating from boss. Proj-14 confirmed.
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 850)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ability, 0.12))
            )
            // Cookie Monster — meme event boss
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_OROH0lDrGTg f_00759 (active fight + defeat frame):
            // dense cloud of small round WHITE/CREAM bullets clearly visible against colorful checkerboard floor;
            // 15-20+ white dots in radial scatter. Proj-13 → small white bullet CONFIRMED. count:10 shootAngle:36
            // RECONSTRUCTED (bullets too dense to count exactly). "The Cookie Monster has been defeated!" in chat.
            .Init("Cookie Monster",  // arena: colorful checkerboard dungeon — floor tiles alternate yellow/gold, green, and blue in a rainbow checkerboard square-tile grid; dark gray stone walls. DOCUMENTED-from-frames disc_OROH0lDrGTg f_00755 (defeat notification "Cookie Monster defeated (2/8)" + rainbow checkerboard floor clearly visible after crowd thins) + f_00759 (active fight at t001 bossend point, "A trap made of cookies" panel visible in right UI, Cookie Monster name + boss fight confirmed). Extracted minimap_00094-00100 unusable (only widget-edge slivers captured).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("Candy... Candy..."), // VERIFIED disc_OROH0lDrGTg — MECH-LOOP re-isolated this pass: chat_00190 shows boss-coloured "Cookie Monster - Candy..." AND f_00759 chat shows "CANDY!!!!" (all-caps). Prior passes noted UNVERIFIABLE; now confirmed at fight frame 759 (t001@759s). The "Candy..." reading is consistent with both frames (truncated + all-caps variant visible). Thematic concern resolved: "Cookie Monster" saying "Candy..." is confirmed verbatim.
                        new Prioritize(new Follow(0.6, 12, 2), new Wander(0.4)),
                        // PARTIAL (f_00759): small round WHITE/CREAM bullets, ~15-20+ in dense radial scatter. Proj-13 CONFIRMED.
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 800)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08))
            )
            // Carrot God — meme boss
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_MFpI_mW1jYM f_00278/f_00280/f_00283 (active fight):
            // small round ORANGE/YELLOW bullets scattered across green-grass arena; consistent across 3 frames;
            // ~8-15 orange dots visible per frame in radial spread. Proj-13 → orange/carrot sprite CONFIRMED.
            // Both taunts frame-confirmed in all 3 frames. count:8 shootAngle:45 RECONSTRUCTED.
            .Init("Carrot God",  // arena: Forest/Garden dungeon — large central clearing with bright green grass floor; brown/dirt cross-shaped corridor paths intersecting through the clearing; large green trees lining corridor edges; organic petal-leaf-shaped dungeon boundary. DOCUMENTED-from-frames disc_MFpI_mW1jYM f_00281 (Carrot God entity visible + taunts in chat + green grass clearing + cross-path floor clearly shown) + minimap_00035 (clean green petal-shaped dungeon minimap, cross/plus corridor pattern, red boss dot + yellow player dots clearly readable).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("Oh you finally made it, I bet you were expecting an expert magician. Well..."), // CORRECTED disc_MFpI_mW1jYM (was truncated "...expecting..."; wide-re-crop of f_00281 shows the FULL line "Carrot God: Oh you finally made it, I bet you were expecting an expert magician. Well..." — the joke: you expected an expert magician, you got a Carrot God. Restored the dropped "an expert magician. Well...")
                        new Taunt(0.0016, "here I am!!"), // VERIFIED disc_MFpI_mW1jYM (frame-read verbatim, boss-prefix "Carrot God:" confirmed; the 2nd half of the reveal)
                        new Prioritize(new Wander(0.4)),
                        // PARTIAL (f_00278-f_00283): bullet = small round ORANGE/YELLOW; ~8-15 dots in radial scatter. Proj-13 CONFIRMED.
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 800),
                        new Shoot(13, count: 4, shootAngle: 20, predictive: 0.5, coolDown: 1300)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Mewtwo — psychic meme boss (shots Confused). Open REALM event (amber minimap, disc_akLcdL1MzZg).
            // MECH-LOOP: Bullet shape DOCUMENTED: X/cross-shaped (4-pointed star projectile), seen in b001_117s_019
            // (multi-bullet scatter fight frame), b001_117s_025 (single isolated X-cross mid-air), b001_117s_037
            // (stray X-cross on diamond-checker floor post-fight). Pattern: wide scatter consistent with Confused/
            // chaotic spread. Exact count+angle not determinable from frozen frames → count/angle remain RECONSTRUCTED.
            // Map DOCUMENTED: open realm event with diamond-checker tile floor platform (diagonal gray diamond grid),
            // dark center square with white-dotted diamond border as boss spawn; amber minimap_00001/00004/00007.
            // Loot obs: "Cookie" (all-stat pot: 2ATK/2DEF/2SPD/2VIT/2WIS/2DEX/10HP/10MP) visible in b001_117s_007
            // loot-pickup frame immediately post-kill — probable Mewtwo drop (not yet added to threshold; verify item ID).
            .Init("Mewtwo",  // arena: open REALM event — diamond/diagonal-checker tile floor (gray diamond grid) with dark central boss-spawn square framed by white-dotted diamond border; no dungeon enclosure. DOCUMENTED-from-frames disc_akLcdL1MzZg b001_117s f_001/003 (arena overview + Mewtwo entity visible); minimap_00001/00004/00007 (amber = open realm, irregular organic shape).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("You think you can beat me?"), // VERIFIED disc_akLcdL1MzZg (frame-read, "Mewtwo:" prefix, frames 46-52)
                        new Taunt(0.0016, "So you are trying to kill me?"), // VERIFIED disc_akLcdL1MzZg (frame-read, "Mewtwo:" prefix) + corroborated in disc_t4BuOE5nwBE + disc_hp_dAXf_alY (3-spec)
                        new Taunt(0.0016, "I AM THE STRONGEST CREATURE IN THIS WORLD!"), // VERIFIED disc_akLcdL1MzZg (frame-read exact, "Mewtwo:" prefix; on-character Pokemon-Mewtwo line). NOTE: did NOT add the task's "Well I can't let you do that." — could not locate it in-frame (saw THIS line in its place), and it conflicts with disc_hp_dAXf_alY's "Well I can kill you outside!" -> both left out as unverified/conflicting
                        new Flash(0x9933ff, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 700),            // X/cross bullets (shape DOCUMENTED-from-frames b001_117s_019/025/037); count+angle RECONSTRUCTED — wide scatter Confused pattern
                        new Shoot(14, count: 5, shootAngle: 18, predictive: 0.6, coolDown: 1100) // aimed burst (RECONSTRUCTED — exact geometry unverifiable)
                    )
                ),
                new Threshold(0.1, new TierLoot(12, ItemType.Weapon, 0.1), new TierLoot(13, ItemType.Armor, 0.1), new TierLoot(5, ItemType.Ring, 0.12))
            )
            // God of Luck — meme boss whose gimmick is SPAMMING the Lucky-Potion cooldown text as boss chat
            // (in footage it floods chat with "God of Luck: You can use your Lucky Potion again in 5 seconds").
            // MECH-LOOP 2026-06-24: attacks UNVERIFIABLE — disc_OROH0lDrGTg f_00655/f_00656 show the boss
            // entity (LARGE RAINBOW UNICORN sprite) in a pink/pastel dungeon room (pink walls, tan checkerboard
            // floor). Fight was too brief for active bullet capture. Proj-13 + count/angle RECONSTRUCTED.
            // Arena: pink/pastel room CONFIRMED disc_OROH0lDrGTg f_00655.
            .Init("God of Luck",
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt(0.02, "You can use your Lucky Potion again in 5 seconds"), // VERIFIED disc_rk0RkuuDu9g (re-frame-confirmed this pass, chat_00064: "God of Luck: You can use your Lucky Potion again in 5 sec[onds]", boss-prefix confirmed). Originally CORRECTED disc_rk0RkuuDu9g (was truncated "You can use your Luck..."; full frame-read boss-coloured line "God of Luck: You can use your Lucky Potion again in 5 seconds" — the boss SPAMS this cooldown-countdown text; the auto-bridge's "...in N seconds" placeholder + the partial fragment are the same line, so MERGED here. Higher prob 0.02 to reflect the spam gimmick)
                        new Prioritize(new Wander(0.4)),
                        new Shoot(13, count: 9, shootAngle: 40, coolDown: 800)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new ItemLoot("Potion of Life", 0.1))
            )
            // Giant Oryx Chicken — Chicken Coop family meme boss
            .Init("Giant Oryx Chicken",  // arena: Chicken Coop dungeon — brown/earthy large stone tile floor with dark edge lines; yellow/gold chicken-themed decoration items scattered on floor; dark brown/stone walls. DOCUMENTED-from-frames disc_EMgDk9BDwOE f_00440-450 (Chicken Coop interior with brown tile floor + yellow decorations clearly visible) + f_00460 (large boss entity fight in same dungeon — entity is Golden Oryx Effigy, NOT confirmed Giant Oryx Chicken, but dungeon confirmed as shared Chicken Coop space). Extracted minimap_00040+ unusable (all frames captured streamer "SUBSCRIBE LEAVE A LIKE" overlay, not game minimap).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("Cluck cluck BOCK!!"), // SOURCED-1-spec disc_IZ2Z7AUhb8s + UNVERIFIABLE-conflict: the OTHER spec disc_hfhcXPoJq1U reads the chicken squawk as "BUCK!!" (+ "Cluck cluck cluck... CLUCK!" / "CLLLUUUCCCKKK!!!") -> BOCK vs BUCK is a cross-spec onomatopoeia conflict. Direct frame-read attempted on BOTH tags but the cluck lines are lava/event-feed-buried (not isolated). Kept "BOCK" (no evidence to prefer BUCK; both valid chicken squawks) -> NOT corrected, NOT frame-confirmed. (disc_hfhcXPoJq1U's extra cluck lines = candidates for a future enrich once frame-confirmed)
                        new Taunt(0.0016, "Cluck cluck cluck... CLUCK!"), // SOURCED-1-spec disc_hfhcXPoJq1U (DISTINCT multi-cluck line, on-theme chicken cluck; not frame-re-isolated — same buried-source caveat as the BOCK line). Added as a distinct cluck (NOT the BOCK/BUCK conflict)
                        new Taunt(0.0016, "CLLLUUUCCCKKK!!!"), // SOURCED-1-spec disc_hfhcXPoJq1U (DISTINCT elongated cluck scream; not frame-re-isolated). NOTE: deliberately did NOT add the queued "BUCK!!" — it's the BOCK/BUCK cross-spec conflict (same squawk as the existing "Cluck cluck BOCK!!"), not a distinct line
                        new Prioritize(new Follow(0.55, 12, 2), new Wander(0.4)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 750)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // The Mysterious Card — magician event boss
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_JbQvbkBLRjE f_00098 (active fight):
            // small white CROSS/X-shaped or diamond-shaped projectiles clearly visible ~8-12 distributed
            // radially around card entity in green-grass realm. Pattern consistent with fixedAngle ring.
            // Proj-13 → white cross sprite CONFIRMED. count:8 shootAngle:45 per ring PLAUSIBLE.
            .Init("The Mysterious Card",  // arena: open REALM event — green grass biome, red mushrooms, trees scattered; no dungeon walls. DOCUMENTED-from-frames disc_JbQvbkBLRjE f_00100 (Mysterious Card entity visible as small card sprite + full taunt in chat + green grass realm arena confirmed + defeat notification "The Mysterious Card has been defeated (10/24)" visible). Extracted minimap_00075 only captured left-edge sliver; in-game minimap in f_00100 shows realm scattered-dot minimap.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("With my sleight of hand, I will make you disappear!"), // CORRECTED disc_JbQvbkBLRjE — MECH-LOOP re-isolated at f_00100 (chat reads exact full line "Mysterious Card: With my sleight of hand, I will make you disappear!" — prior truncated "..." now completed verbatim). Boss defeat feed: "The Mysterious Card has been defeated (10/24)".
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        // PARTIAL (f_00098): small white cross/X-shaped bullets ~8-12 in radial ring distribution. Proj-13 CONFIRMED.
                        new Shoot(13, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 900),
                        new Shoot(13, count: 8, shootAngle: 45, fixedAngle: 22, coolDown: 900)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // MH Wave Controller — wave-spawner; panics on death. "MH" = Mad House dungeon.
            // MECH-LOOP 2026-06-24: attacks UNVERIFIABLE — disc_MFpI_mW1jYM f_00040-f_00070 show dark red
            // brick dungeon (curved/arched walls; Mad House aesthetic). Small light-colored (green/yellow?)
            // projectiles visible but unresolvable at dark dungeon resolution. TAUNTS: 2 new lines found
            // in f_00040/f_00060 chat (boss-colored prefix "MH Wave Controller:") and added to "waves" state.
            .Init("MH Wave Controller",
                new State(
                    new State("idle", new PlayerWithinTransition(13, "waves")),
                    new State("waves",
                        new Taunt(0.0016, "I guess I'll let any left over tricks in my hat take care of you"), // VERIFIED disc_MFpI_mW1jYM f_00040/f_00050 (boss-colored chat, full line; "magical army" summon context)
                        new Taunt(0.0016, "And don't bother fighting back, you won't want to face me and my magical army."), // VERIFIED disc_MFpI_mW1jYM f_00040/f_00060 (boss-colored chat, full line confirmed in two frames)
                        new Spawn("Asgard Guardian", maxChildren: 6, initialSpawn: 0.4, coolDown: 6000),
                        new Prioritize(new StayCloseToSpawn(0.2, 5), new Wander(0.3)),
                        new Shoot(13, count: 6, shootAngle: 60, coolDown: 900),
                        new HpLessTransition(0.25, "panic")
                    ),
                    new State("panic",
                        new Taunt("No! What's happening?! You weren't supposed to escape!"), // CORRECTED disc_MFpI_mW1jYM (was truncated+misread "...You were..."; the narrow chat crop cut at "wer", but the WIDE f_00096 frame shows the full boss-prefixed line "MH Wave Controller: No! What's happening?! You weren't supposed to escape!" -> "were"->"weren't" + restored "supposed to escape!". NOTE: ledger had no source_tags = matcher miss; this is FRAME-VERIFIED, not designed)
                        new Flash(0xff3333, 0.25, 10),
                        new Spawn("Asgard Guardian", maxChildren: 10, initialSpawn: 0.6, coolDown: 3500),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 650)
                    )
                ),
                new Threshold(0.1, new TierLoot(12, ItemType.Weapon, 0.1), new TierLoot(13, ItemType.Armor, 0.1), new TierLoot(5, ItemType.Ring, 0.12))
            )
            // Creepy Talking Jack-O-Lantern — Halloween NPC boss (shots Blind)
            // MECH-LOOP 2026-06-24: attacks UNVERIFIABLE — disc_iFlpCUkcjvY f_00520-f_00550: fight occurred
            // under streamer "KEYWORD IS rotf" giveaway overlay which covered the fight area. Taunt CORRECTED
            // from f_00534 chat (bottom-of-frame, readable past overlay): "wandering the" replaces spec's
            // reconstructed "in the"; "realm, they're" (comma, lower-case) replaces "realm. They're".
            // MECH-LOOP 2026-06-24: SECOND TAUNT SPOTTED — disc_iFlpCUkcjvY f_00515 chat (bottom of frame, partially legible):
            // "<Creepy Talking Jack O Lantern>: Down with the pumpkin-carving [X]!" — boss-colored prefix CONFIRMED; last word
            // before "!" illegible at this resolution. NOT added to Taunt() yet — needs re-read from higher-res source or wider
            // crop. First word "Down" + "pumpkin-carving" clearly readable.
            .Init("Creepy Talking Jack-O-Lantern",
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("Look out for those skeletons wandering the realm, they're spooky AND scary!"), // CORRECTED disc_iFlpCUkcjvY f_00534 (chat readable below keyword overlay: "...skeletons wandering the realm, they're spooky AND scary!" — was "in the"→"wandering the", period→comma, "They're"→"they're")
                        new Prioritize(new Wander(0.4)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 800)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08))
            )
            // Uvuvwuwuwe — the long-name meme realm boss (taunt verbatim disc_DtB0_zeZVEQ). Object 0x8f0f.
            // MECH-LOOP 2026-06-24: attacks NO-FOOTAGE — disc_DtB0_zeZVEQ/disc_bT-s0N7PZR0/disc_ed9YjhhonUU all
            // contain spawn/defeat event-feed mentions only; no frames show the active fight. Taunt "You disappoint me."
            // is DOCUMENTED from chat (boss-prefixed). Attacks remain RECONSTRUCTED (generic radial + predictive).
            .Init("Uvuvwuwuwe",
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("You disappoint me."), // SOURCED-1-spec disc_DtB0_zeZVEQ (spec documents "Uvuvwevwevwe - You disappoint me." verbatim w/ boss-prefix). Generic line; direct frame-read attempted (red-rank + grayscale) but desert/event-feed-buried in 274 crops -> not isolated. NOT designed (it IS sourced); kept as-is (no error evidence).
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 800),
                        new Shoot(14, count: 6, shootAngle: 15, predictive: 0.4, coolDown: 1300)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08))
            )
            // Smiling Dragon — meme realm dragon (taunt 2x-confirmed disc_DtB0_zeZVEQ + disc_EfO-XlNpe6A). Object 0x8fd7.
            .Init("Smiling Dragon",  // arena: open REALM event — sandy/desert biome; tan/sandy floor with small cacti decorations scattered; no dungeon walls. Boss entity: large purple/violet dragon sprite. DOCUMENTED-from-frames disc_DtB0_zeZVEQ f_00195 (large purple dragon entity in open sandy desert realm fight, player cluster visible; boss name flagged uncertain in spec but arena confirmed) + minimap_00022 (realm scatter pattern, yellow/amber dots confirming open realm). disc_EfO-XlNpe6A confirms "Smiling Dragon" name verbatim (per existing taunt spec) but fight section not re-isolated this pass (1682-frame marathon).

                new State(
                    new State("idle", new PlayerWithinTransition(14, "fight")),
                    new State("fight",
                        new Taunt("It feels like time to rock 'n roll once more!"), // VERIFIED-2x-spec (disc_DtB0_zeZVEQ + disc_EfO-XlNpe6A, identical verbatim; name "Smiling Dragon" confirmed in disc_EfO-XlNpe6A — disc_DtB0_zeZVEQ had the line but flagged the name uncertain). Direct frame-isolation attempted (disc_DtB0_zeZVEQ spread) but feed-buried -> cross-spec convergence, no conflict w/ build (build's capital "It" = chat-line start)
                        new Prioritize(new StayCloseToSpawn(0.25, 7), new Wander(0.3)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 700),
                        new Shoot(14, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1200)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.1), new TierLoot(12, ItemType.Armor, 0.1))
            )
            // Warbringer — ROTF realm war-god event boss (Cyclops-family; "A Warbringer has just spawned!").
            // CORRECTED taunts vs the auto-bridge task, which carried transcription errors. Footage
            // disc_Yx0kTFEixbA frame-reads the speaker prefix "Warbringer:" -> "Watchlings!" (the task's
            // "Weaklings!" was a MISREAD of "Watchlings!") + right-edge-cut "This land..." dominance lines.
            // The task's "Your attacks have no effect on me!" is actually HEIMDALL's line (same footage,
            // cross-attributed) -> REMOVED. Watchlings-summon mechanic noted for a future enrich (minion not yet defined).
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_Yx0kTFEixbA f_00165/f_00167 (Warbringer 51% HP):
            // small round ORANGE/RED bullets clearly visible in radial scatter across green-grass realm arena.
            // Proj-15 → orange/red sprite CONFIRMED. count:10 shootAngle:36 RECONSTRUCTED.
            // TAUNTS: f_00165 shows multiple full lines in boss-colored chat (several new + one partial completed).
            .Init("Warbringer",
                new State(
                    new State("idle", new PlayerWithinTransition(14, "fight")),
                    new State("fight",
                        new Taunt(0.0016, "Watchlings!"), // VERIFIED disc_Yx0kTFEixbA (frame-read exact; CORRECTED from the task's misread "Weaklings!")
                        new Taunt(0.0016, "The monarchy will conquer this land!"), // VERIFIED disc_Yx0kTFEixbA f_00165 (boss-colored chat, full line visible)
                        new Taunt(0.0016, "You shall feel the wrath of the monarchy!"), // COMPLETED disc_Yx0kTFEixbA f_00165 (was truncated "You shall feel the wrath..."; full line now readable with explicit "Warbringer:" prefix)
                        new Taunt(0.0016, "Just try and stop me!"), // VERIFIED disc_Yx0kTFEixbA f_00165 (boss-colored chat)
                        new Taunt(0.0016, "I will not waver!"), // VERIFIED disc_Yx0kTFEixbA f_00165 (boss-colored chat)
                        new Taunt(0.0016, "I am unstoppable!"), // VERIFIED disc_Yx0kTFEixbA f_00165 (boss-colored chat)
                        new Taunt(0.0016, "Stop resisting!"), // VERIFIED disc_Yx0kTFEixbA f_00167 (boss-colored chat)
                        new Taunt(0.0016, "This land will become..."), // VERIFIED-partial disc_Yx0kTFEixbA (right-edge cut)
                        new Taunt(0.0016, "This land is already..."), // VERIFIED-partial disc_Yx0kTFEixbA (right-edge cut)
                        new Taunt(0.0016, "Your sloppy techniques..."), // VERIFIED-partial disc_Yx0kTFEixbA (right-edge cut)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        // PARTIAL (f_00165/f_00167): small round ORANGE/RED bullets in radial scatter. Proj-15 CONFIRMED.
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 700),
                        new Shoot(15, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 1200)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.1), new TierLoot(12, ItemType.Armor, 0.1))
            )
            // Shade of Novus ("Novus") — vengeful BLOOD/RITUAL resurrection boss (red-lava + grey X-corridor
            // dungeon). Lore: Novus performs a ritual to resurrect, then takes revenge on "RICHTUS" (Novus vs
            // Richtus). Taunts VERIFIED vs disc_MszcvkbWED8 frames (speaker prefix "Novus:"). CORRECTED the
            // auto-bridge task's INVENTED completion "...restored to my full power" -> footage says "...restored
            // to my FORMER GLORY". Built as the footage's 2-phase narrative (ritual -> resurrected/revenge).
            // id "Shade of Novus" (event-feed name; collision-checked vs the unrelated "Shade" in Shrouded
            // Sanctum). Object 0x8fdb; kill attribution reads "Novus" (f_00230 death screen).
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_MszcvkbWED8 f_00218/f_00220/f_00222 (late fight):
            // small round RED-ORANGE projectiles clearly visible scattered in radial pattern across dark inner arena.
            // Arena corroborated: dark interior + bright orange/lava-tile border confirmed. proj-15/16 → red sprite
            // matches observations; count/angle remain RECONSTRUCTED.
            // NOTE: chat_00055 reads "RICHTUS WILL FALL!" but chat_00058 and build say "RICHTUS WILL PAY!" —
            // discrepancy unresolved; both frames at low resolution. Current "PAY" kept (prior careful read).
            .Init("Shade of Novus",  // arena: blood/ritual dungeon — narrow vertical entry corridor leading into large rectangular boss chamber; floor: red/orange lava-tile pattern (large red-orange stone tiles, dark grid seams); dark stone walls. Boss entity: large dark-red/crimson. DOCUMENTED-from-frames disc_MszcvkbWED8 f_00050 (large dark-red boss entity on red lava-tile floor, dark walls) + f_00100 (red/orange lava tiles confirmed across full fight area) + minimap_00020 (narrow vertical corridor entry → large rectangular boss chamber, dark dungeon confirmed). NOTE: disc_MszcvkbWED8 is a marathon — Diagon/Necropolis fight begins ~f_00150 (distinct gray floor + Diagon taunt visible); minimap_00025 onwards shows Necropolis cross-corridor layout, NOT Novus dungeon.

                new State(
                    new State("idle", new PlayerWithinTransition(14, "ritual")),
                    // P1: the resurrection ritual
                    new State("ritual",
                        new Taunt(0.0016, "With fresh blood in my veins, I will be restored to my former glory..."), // VERIFIED disc_MszcvkbWED8 (CORRECTED from the task's invented "...my full power")
                        new Taunt(0.0016, "COME! LET THE RITUAL BEGIN!"), // VERIFIED disc_MszcvkbWED8 (frame-read exact)
                        new Taunt(0.0016, "My body, I CAN BREATH... I CAN FEEL..."), // CORRECTED disc_MszcvkbWED8 (was: "My body... I CAN FEEL... I CAN FEEL..." — duplicated FEEL / dropped BREATH; frame-read chat_00058 exact: "My body, I CAN BREATH... I CAN FEEL...")
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        // PARTIAL (f_00218-f_00222): small round RED-ORANGE bullets in radial scatter — proj-15 red CONFIRMED.
                        new Shoot(15, count: 8, shootAngle: 45, coolDown: 800),
                        new HpLessTransition(0.5, "restored")
                    ),
                    // P2: resurrected -> revenge on Richtus
                    new State("restored",
                        new Flash(0xffaa0000, 0.3, 8),
                        new Taunt("AT LAST I AM RESTORED TO MY FORMER GLORY, FEAR ME!"), // CORRECTED disc_MszcvkbWED8 (was: "AT LAST! ...FEAR ME!!!!" — frame-read chat_00058 shows NO "!" after "AT LAST"; "FEAR ME" tail overlay-obscured so the invented "!!!!" normalized to a single "!")
                        new Taunt(0.0016, "With every passing second, I GROW EVER STRONGER"), // VERIFIED-partial disc_MszcvkbWED8 (NEW grounded taunt, frame-read chat_00058: "<Novus> With every passing second, I GROW EVER STRONGE[R]"; end-punctuation cut, none invented)
                        new Taunt(0.0016, "Vengeance is within my grasp! RICHTUS WILL PAY!"), // VERIFIED disc_MszcvkbWED8 (frame-read exact)
                        new Prioritize(new Wander(0.5)),
                        new Shoot(16, count: 12, shootAngle: 30, coolDown: 550),
                        new Shoot(16, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 1000)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.1), new TierLoot(12, ItemType.Armor, 0.1))
            )
            // Pulse — cryptic/eldritch identity-themed realm EVENT boss ("Chimera - The Pulse has just spawned!").
            // Taunt VERIFIED vs disc_SlVLi0h14UQ frames (red speaker prefix "Pulse:"). The auto-bridge SPLIT one
            // wrapped chat line into two fragments ("Do not answer. Your identit[y]..." + "...peace, you will feel
            // pain.") -> MERGED into the single full line read off-frame. Object 0x8fdc; attack = generic
            // radial+aimed (no bullet geometry recovered).
            // MECH-LOOP 2026-06-24: attacks UNVERIFIABLE — disc_SlVLi0h14UQ farming session, Pulse spawn/defeat
            // confirmed (chat_00010 "Goblin - Pulse has just spawned!") but fight too brief to isolate bullet
            // patterns at the farming pace. Attacks RECONSTRUCTED (generic radial + predictive).
            .Init("Pulse",
                new State(
                    new State("idle", new PlayerWithinTransition(14, "fight")),
                    new State("fight",
                        new Taunt(0.0016, "Do not answer. Your identity does not interest me. For interrupting my peace, you will feel pain."), // VERIFIED disc_SlVLi0h14UQ (frame-read exact; MERGED from the task's 2 split fragments into the one wrapped line)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 700),
                        new Shoot(15, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 1200)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.1), new TierLoot(12, ItemType.Armor, 0.1))
            )
            // Skeletal Monstrosity — undead SUMMONER boss (its minions fail -> it takes over). Speaker prefix
            // "Skeletal Monstrosity:" VERIFIED vs disc_Yx0kTFEixbA frames (this is a GENUINE attribution, NOT a
            // cross-attribution — unlike Warbringer's Heimdall line from this same tag, which I caught earlier).
            // Taunts are right-edge CUT in the footage -> built as VERIFIED-partial stems only (the fuller
            // "...you'll see now that I must do this myself" continuation was read but not cleanly re-isolated, so
            // left out conservatively). Object 0x8fdd; summon-theme noted for a future enrich (skeletal minion not yet defined).
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_Yx0kTFEixbA f_00169/f_00171 (fight section, boss label "Skeletal"):
            // small round RED-ORANGE/CRIMSON projectiles clearly visible across arena in radial spread pattern;
            // ~10-15 red dots distributed through arena. proj-15 maps to whatever red sprite is defined in XML —
            // color CONFIRMED. count:10 shootAngle:36 (full ring) + predictive burst counts remain RECONSTRUCTED.
            // f_00171: "1 Necrotic Token(s)" item visible (unclear if boss drop or dungeon chest — NOT added to loot).
            // Arena: dark charcoal stone floor (dark gray square grid), purple/dark stone walls CORROBORATED (f_00169).
            .Init("Skeletal Monstrosity",  // arena: purple/violet-floor dungeon — large rectangular rooms connected by corridors; floor: dark purple/violet stone tiles; dark stone walls. DOCUMENTED-from-frames disc__WkTRDW_vQ0 minimap_00005 (purple-colored multi-room dungeon, boss chamber + corridor layout clearly readable) + f_00040/f_00050 (purple tile floor confirmed in corridors post-fight). disc_Yx0kTFEixbA minimap unusable throughout (streamer thermometer overlay). BONUS: disc_Yx0kTFEixbA f_00170 fight-frame chat shows complete taunts at full width — corrected two partials + added summon-phase taunt.
                new State(
                    new State("idle", new PlayerWithinTransition(14, "fight")),
                    new State("fight",
                        new Taunt(0.0016, "I shall raise these piles of bones once again! COME ALIVE, MY BEAUTIFUL CREATIONS!"), // VERIFIED disc_Yx0kTFEixbA f_00170 (NEW — full fight-frame chat read; summon-phase opening taunt; not in prior spec's chat panel due to right-edge cut)
                        new Taunt(0.0016, "NO! They were not meant to be destroyed by the likes of you! I see now that I must do this myself!"), // CORRECTED disc_Yx0kTFEixbA f_00170 (was: partial "NO! They were too..."; full line now readable in fight-frame chat at chat_00043 timestamp)
                        new Taunt(0.0016, "THE POWER OF DARKNESS WILL REIGN UNRELENTING!"), // CORRECTED disc_Yx0kTFEixbA f_00170 (was: partial "THE POWER..."; full line now readable in same fight-frame chat)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        // PARTIAL (f_00169): bullet = small round RED-ORANGE/CRIMSON; ~10-15 distributed in radial spread.
                        // count:10 shootAngle:36 + predictive burst RECONSTRUCTED; bullet color CONFIRMED.
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 700),
                        new Shoot(15, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 1200)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.1), new TierLoot(12, ItemType.Armor, 0.1))
            )
            // The Puppet Master — Puppet Theatre boss (Puppet Theatre Portal drops in Mountain.cs). Object 0x8fde.
            // TAUNTS cleaned/de-duped from the auto-bridge's messy bracketed list using the FOCUSED-FIGHT capture
            // disc_IBEp1CbA00Q (full clean lines) — which CORRECTS the bridge's "Find me if you can h[ide]" misguess
            // ("h..." = "hero", not "hide"). Signature mechanic = a FAKE-DEATH / PLOT-TWIST (he "dies" then revives).
            // MECH-LOOP: Bullet shape DOCUMENTED: small white squares (consistent across all fight frames).
            // Aimed behavior DOCUMENTED: b014_130s_002 shows clear vertical column of ~12-15 white square bullets
            // aimed straight down toward a player (aimed/predictive line confirmed). Radial component visible as scattered
            // squares in b009_77s_001 but bullet count/angle not isolatable (crowded fight frame). Map DOCUMENTED:
            // Puppet Theatre dungeon (gray minimap_00003/00005); diagonal-stripe/woven stage floor in combat area
            // (b009_77s_001, b014_130s_003), white-bordered dark square tiles in loot/corridor areas (b005_41s).
            // Loot: Ring of Superior Defense + Potion of Dexterity seen in b005_41s post-kill tooltip (consistent with
            // existing TierLoot designed entries; no new items observed). Source: disc_IBEp1CbA00Q.
            .Init("The Puppet Master",  // arena: Puppet Theatre dungeon — diagonal-stripe/woven-fabric tile floor on main stage (distinctive theater-stage aesthetic), white-bordered dark square tiles in corridor/loot areas; gray dungeon on minimap. DOCUMENTED-from-frames disc_IBEp1CbA00Q b005_41s (post-kill loot area tiles), b009_77s_001 (stage floor during fight), b014_130s_003 (Quest Complete + stripe floor); minimap_00003/00005 (gray dungeon).
                new State(
                    new State("idle", new PlayerWithinTransition(14, "act1")),
                    // ACT 1 — directs his puppets
                    new State("act1",
                        new Taunt("Watch them dance, hero, as they drain your life away!"), // VERIFIED disc_IBEp1CbA00Q (focused-fight, full line)
                        new Taunt(0.0016, "Find me if you can, hero, or die trying!"), // VERIFIED disc_IBEp1CbA00Q (full) — CORRECTS the auto-bridge "Find me if you can h[ide]" guess: "h..." was "hero", not "hide"
                        new Taunt(0.0016, "ENCORE, HERO!"), // VERIFIED-2x-spec (disc_1mU5uHE6DXE + disc_AaMomuk2Vpk)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Spawn("Oryx Puppet", maxChildren: 1, initialSpawn: 0, coolDown: 12000), // recovered: he animates an Oryx-impersonating puppet henchman (0x8ff6) — disc_LJzfb9Y4ok0
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 700),            // white-square radial (shape DOCUMENTED-from-frames b014_130s_002; count+angle RECONSTRUCTED — scattered bullets in b009_77s_001 unresolvable)
                        new Shoot(14, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1200), // aimed: DOCUMENTED-from-frames b014_130s_002 (vertical aimed column of white-square bullets toward player); count+angle RECONSTRUCTED
                        new HpLessTransition(0.5, "plottwist")
                    ),
                    // FAKE DEATH -> PLOT TWIST (the signature mechanic; invuln during the dramatic pause)
                    new State("plottwist",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("You may have killed me, but I am only a pretender! I am ready for a plot twist!"), // VERIFIED disc_IBEp1CbA00Q (the fake-death/plot-twist line)
                        new Flash(0x9933ff, 0.3, 12),
                        new TimedTransition(2500, "encore")
                    ),
                    // ACT 2 — the encore (vulnerable again, harder)
                    new State("encore",
                        new Taunt("ENCORE, HERO!"), // VERIFIED disc_AaMomuk2Vpk (frame-read this pass — "ENCORE, HERO!" on-screen verbatim; also corroborated in disc_1mU5uHE6DXE). Encore-state placement matches the literal Act-2 "encore" moment.
                        new Prioritize(new Follow(0.7, 12, 2), new Wander(0.4)),
                        new Shoot(16, count: 14, shootAngle: 25, coolDown: 550, projectileIndex: 1),
                        new Shoot(16, count: 6, shootAngle: 15, predictive: 0.6, coolDown: 900),
                        new HpLessTransition(0.1, "final")
                    ),
                    new State("final",
                        new Flash(0xffff0000, 0.2, 10),
                        new Taunt("NO! This cannot be how my story ends! I WILL HAVE MY ENCORE, HERO!"), // CORRECTED disc_1mU5uHE6DXE f_00022 (chat: full line "<The Puppet Master> NO! This cannot be how my story ends! I WILL HAVE MY ENCORE, HERO!"; prior "NO!!! This cannot be!" was truncated)
                        new Taunt(0.0016, "Lucky guess hero, but I've run out of time to play games with you. It is time that you die."), // VERIFIED disc_1mU5uHE6DXE f_00018/f_00022 (was DEFERRED as illegible-gap; now fully readable: exact text confirmed both frames)
                        new Prioritize(new Wander(0.6)),
                        new Shoot(16, count: 12, shootAngle: 30, coolDown: 500)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.1), new TierLoot(12, ItemType.Armor, 0.1), new TierLoot(5, ItemType.Ring, 0.12))
            )
            // Oryx Puppet — a PUPPET HENCHMAN summoned by The Puppet Master (above) that impersonates Oryx the Mad God.
            // NOT a realm boss: the auto-bridge's "add to Mountains RegionMobs pool" how was WRONG for this entity. Recovery
            // (disc_LJzfb9Y4ok0 + disc_YEGMcwgmGTM + disc_rWsv3qfYtM8) shows it appears INSIDE the Puppet Master fight,
            // displays as "<Oryx the Mad God>" while alive and "<Oryx Puppet>" on death, with a TELL-TALE low 75000 HP ->
            // wired as a Spawn from The Puppet Master's act1 (see above). Object 0x8ff6. HP=75000 is GROUNDED (it
            // self-announces "I still have 75000 hitpoints!"). Attack pattern RECONSTRUCTED (no bullet geometry recovered).
            // MECH-LOOP: Attacks UNVERIFIABLE-ILLEGIBLE — Oryx Puppet and Puppet Master fight simultaneously on the same
            // stage; bullets from both bosses are present in all fight frames (disc_LJzfb9Y4ok0 f_00144, b002_74s_015)
            // and cannot be attributed to this entity specifically. Map DOCUMENTED: same Puppet Theatre dungeon as Puppet
            // Master — fight occurs on diagonal-stripe/woven stage floor; central boss room has cross/diamond shape with
            // white polka-dot floor (disc_LJzfb9Y4ok0 f_00138 + b002_74s_001). Loot: "Wine Cellar Incantation" (Feed
            // Power 500) visible in b002_74s_037 post-kill pickup — probable Puppet Theatre boss-kill drop (Oryx
            // impersonator → Oryx-key thematic loot). NOT yet added to threshold (verify game item name first).
            .Init("Oryx Puppet",  // arena: Puppet Theatre dungeon — same diagonal-stripe/woven stage floor as The Puppet Master; both fight simultaneously on stage. Central boss room: cross/diamond-shaped room with white polka-dot floor. DOCUMENTED-from-frames disc_LJzfb9Y4ok0 f_00138 (boss room overview), f_00144 (active fight on stripe floor), b002_74s_001 (cross-diamond room layout).
                new State(
                    new State("idle", new PlayerWithinTransition(12, "perform")),
                    new State("perform",
                        new Taunt(0.0016, "Am I not an uncanny likeness of Oryx himself?"), // CORRECTED disc_YEGMcwgmGTM (was: "Am I not an uncanny likeness..."; VERIFY-LOOP wide f_ re-crop f_00064 reveals the full line beyond the narrow chat crop)
                        new Taunt(0.0016, "You don't see the similarity? Well then, let me show you!"), // CORRECTED disc_YEGMcwgmGTM (was: "You don't see the similarities..."; wide f_ re-crop shows full line + singular "similarity")
                        new Taunt(0.0016, "Miniscule worms! I still have 75000 hitpoints!"), // VERIFIED disc_LJzfb9Y4ok0 (clear; the 75000-HP self-announce = source of this entity's MaxHitPoints)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 700),                              // radial (RECONSTRUCTED - no geometry recovered)
                        new Shoot(13, count: 5, shootAngle: 15, predictive: 0.5, coolDown: 1200, projectileIndex: 1), // aimed (RECONSTRUCTED)
                        new HpLessTransition(0.15, "broken")
                    ),
                    new State("broken",
                        new Flash(0xffff0000, 0.2, 8),
                        new Taunt("Nooooo! This cannot be!"), // VERIFIED disc_LJzfb9Y4ok0 (the Oryx Puppet's DEATH cry, frame-read)
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 600)
                    )
                ),
                new Threshold(0.25, new TierLoot(5, ItemType.Potion, 0.06))
            )
            // God of Amber Stones — broken-English meme realm boss (object 0x8fdf). Single source disc_CnZQkLsjBVI
            // (boss-prefix confirmed). Built with the 3 CLEAN uncut taunts only; 4 right-cut lines DEFERRED (windowed
            // capture cuts them; they don't complete in any frame) -> flagged for fullscreen-source recovery.
            // MECH-LOOP: Bullet shape DOCUMENTED: small white diamond/rotated-square bullets seen in b004_72s_032
            // (active fight frame: radial scatter of diamond bullets around boss sprite) and b004_72s_037 (stray
            // diamonds post-fight). Pattern is wide scatter consistent with wide radial; exact count+angle not
            // resolvable from crowded fight frame → RECONSTRUCTED. New taunt DOCUMENTED: "We are working on his
            // behavior." visible in chat in b004_72s_037 (post-kill frame, added to fight state). Map DOCUMENTED:
            // open realm event with diagonal checkerboard tile floor (large white squares arranged at 45° diagonal
            // grid, dark background) — amber minimap_00010 confirms open realm. Source: disc_CnZQkLsjBVI b004_72s.
            .Init("God of Amber Stones",  // arena: open REALM event — diagonal checkerboard tile floor (large white squares arranged at 45° diagonal grid, dark background between tiles); no dungeon enclosure. DOCUMENTED-from-frames disc_CnZQkLsjBVI b004_72s_032 (fight: boss sprite + diamond bullets on checkerboard floor), b004_72s_024 (post-fight loot on same floor), b004_72s_037 (stray bullets + new taunt in chat); minimap_00010 (amber = open realm).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("Did you scared?"), // VERIFIED disc_CnZQkLsjBVI (frame-read, "God of Amber Stones:" prefix; broken-English meme line)
                        new Taunt(0.0016, "What Happened?"), // VERIFIED disc_CnZQkLsjBVI (frame-read, prefix-confirmed)
                        new Taunt(0.0016, "Prepare to die!"), // VERIFIED disc_CnZQkLsjBVI (frame-read, prefix-confirmed; the boss SPAMS this) — generic line, but attributed in-frame to this boss
                        new Taunt(0.0016, "We are working on his behavior."), // DOCUMENTED-from-frames disc_CnZQkLsjBVI b004_72s_037 (visible in chat post-kill; broken-English meme line attributed to this boss)
                        // DEFERRED (right-cut by the windowed-capture chat box; do NOT complete in any frame -> NOT built, no guessing):
                        // "How did you come[...]", "I think you killed [...]", "Okey... I need to k[ill]...", "its not much hard[er]..." -> flag for fullscreen-source recovery
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 800),            // diamond/rotated-square bullets (shape DOCUMENTED-from-frames b004_72s_032/037); count+angle RECONSTRUCTED — wide radial scatter unresolvable from crowded fight frame
                        new Shoot(13, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1300) // aimed burst (RECONSTRUCTED — predictive behavior not isolatable from frames)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Queen of Hearts — ROTF-CUSTOM Wonderland-dungeon boss (object 0x8fd8; no base-RotMG Queen of Hearts).
            // Source disc_EM9Uba-Nv08 (boss-coloured 'Queen of Hearts', VoltiPlay owner run). 2-phase GUARDS->SOLO
            // mechanic. Attack = generic radial+aimed (no geometry recovered). Guard minions flagged for later.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_EM9Uba-Nv08 f_00060: green ELONGATED DART/ARROW projectiles
            // CONFIRMED in Wonderland multicolor checkerboard arena (red/green/purple/white tile floor, active fight).
            // Green dart/arrow shaped bullets scattered in multiple directions from boss area; wide radial scatter
            // consistent with Shoot(13, count:10, shootAngle:36). attacks_status: no-footage → partial.
            .Init("Queen of Hearts",  // attacks PARTIAL disc_EM9Uba-Nv08 f_00060: green ELONGATED DART bullets CONFIRMED; count:10/angle:36 RECONSTRUCTED. Arena: Wonderland dungeon — multi-colored checkerboard floor (red/green/purple/white large tile squares), multi-room grid/cross layout — DOCUMENTED f_00060 + minimap_00008

                new State(
                    new State("idle", new PlayerWithinTransition(13, "guards")),
                    // P1: commands her guards
                    new State("guards",
                        new Taunt("Guards, kill these troublemakers!"), // VERIFIED disc_EM9Uba-Nv08 (boss-coloured, clear)
                        new Taunt(0.0016, "Guard protect me, if I die, Wonderland will..."), // VERIFIED-partial disc_EM9Uba-Nv08 (right-edge cut at "Wonderland will..."; the auto-bridge's "[fall]" completion is a GUESS -> NOT injected, kept the readable part)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 750),  // green elongated dart bullet PARTIAL f_00060; count:10/angle:36 RECONSTRUCTED
                        new Shoot(13, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1200),  // aimed (RECONSTRUCTED)
                        new HpLessTransition(0.4, "solo")
                    ),
                    // P2: dismisses guards, fights alone (harder)
                    new State("solo",
                        new Flash(0xffdd0000, 0.3, 10),
                        new Taunt("My guards that is enough, I'll do it alone!"), // VERIFIED disc_EM9Uba-Nv08 (boss-coloured, clear; the guards-dismiss/solo line)
                        new Prioritize(new Follow(0.7, 12, 2), new Wander(0.4)),
                        new Shoot(15, count: 14, shootAngle: 25, coolDown: 550),
                        new Shoot(15, count: 6, shootAngle: 15, predictive: 0.6, coolDown: 900)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Jon Bilgewater the Pirate King — ROTF-CUSTOM pirate boss (object 0x8fe6; base pirate king is 'Dreadstump'
            // in Pirate Cave, this is DISTINCT — collision-checked, 0 'bilgewater' in base XML). Source disc_M0KXJV3pdKc
            // (boss-coloured 'Jon Bilgewater the Pirate King:' prefix, wide-re-crop + 3x chat reads; all 5 taunts VERBATIM).
            // Gimmick: periodic invuln BARRIER + summons Parrots (reuses base 'Cave Pirate Parrot') + enrage-on-parrot-death;
            // cannon attacks (cluster/barrage/pewpew). Attack GEOMETRY reconstructed (no bossends) -> spread/cluster + barrage.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_M0KXJV3pdKc f_00235-245: orange/tan ROUND CANNONBALL-style
            // projectiles CONFIRMED scattered in brownish-tan pirate cave arena during active fight. Boss-colored taunts
            // "PEWPEWPEW!" + "AWESOME CANNON CLUSTER!" + "CANNON BARRAGE!" confirmed. Crowded arena (many players) prevents
            // exact count/angle isolation. Cannonball round shape PARTIAL; count:8/angle:45 RECONSTRUCTED. attacks_status:
            // no-footage → partial.
            .Init("Jon Bilgewater the Pirate King",  // attacks PARTIAL disc_M0KXJV3pdKc f_00235-245: orange/tan ROUND CANNONBALL bullets CONFIRMED; count/angle RECONSTRUCTED. Arena: pirate cave — brownish-tan tile floor, dark stone walls, rectangular chamber — DOCUMENTED f_00230 + f_00240
                new State(
                    new State("idle", new PlayerWithinTransition(14, "broadside")),
                    // P1: cannon broadside (CLUSTER spread + aimed) + summon parrots
                    new State("broadside",
                        new Taunt(0.0016, "PEWPEWPEW!"), // VERIFIED disc_M0KXJV3pdKc (verbatim)
                        new Taunt(0.0016, "Check out my AWESOME CANNON CLUSTER!"), // VERIFIED disc_M0KXJV3pdKc (verbatim)
                        new Spawn("Cave Pirate Parrot", maxChildren: 4, initialSpawn: 0.5, coolDown: 8000),
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 700),  // ROUND CANNONBALL shape PARTIAL f_00235-245; count:8/angle:45 RECONSTRUCTED
                        new Shoot(14, count: 4, shootAngle: 18, predictive: 0.5, coolDown: 1100, projectileIndex: 1),  // aimed (RECONSTRUCTED)
                        new TimedTransition(9000, "barrier")
                    ),
                    // BARRIER: periodic invulnerability ("BARRIER ACTIVATE!" — documented in the spec's phases, repeated)
                    new State("barrier",
                        new Taunt("BARRIER ACTIVATE!"), // VERIFIED disc_M0KXJV3pdKc (repeated invuln-barrier line)
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x33ccff, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.3, 5), new Wander(0.25)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 600),
                        new TimedTransition(3000, "barrage")
                    ),
                    // P2: CANNON BARRAGE (heavier) + re-summon parrots
                    new State("barrage",
                        new Taunt(0.0016, "CANNON BARRAGE!"), // VERIFIED disc_M0KXJV3pdKc (verbatim)
                        new Taunt(0.0016, "Now you're making me angry! PARROT!"), // VERIFIED disc_M0KXJV3pdKc (verbatim)
                        new Spawn("Cave Pirate Parrot", maxChildren: 4, initialSpawn: 0, coolDown: 10000),
                        new Prioritize(new Follow(0.6, 12, 2), new Wander(0.3)),
                        new Shoot(16, count: 14, shootAngle: 25, coolDown: 550),
                        new Shoot(16, count: 6, shootAngle: 15, predictive: 0.6, coolDown: 900, projectileIndex: 1),
                        new HpLessTransition(0.35, "enrage"),
                        new TimedTransition(10000, "broadside")
                    ),
                    // ENRAGE: parrots slain -> furious finale
                    new State("enrage",
                        new Flash(0xffff0000, 0.2, 10),
                        new Taunt("YOU'LL PAY FOR KILLING MY PARROTS!"), // VERIFIED disc_M0KXJV3pdKc (parrot-death enrage line)
                        new Prioritize(new Follow(0.7, 12, 2), new Wander(0.4)),
                        new Shoot(16, count: 18, shootAngle: 20, coolDown: 450),
                        new Shoot(16, count: 8, shootAngle: 12, predictive: 0.6, coolDown: 800, projectileIndex: 1)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Kestora the Lava God — ROTF-CUSTOM lava-god boss (object 0x8fe7; owner-played VoltiPlay footage = the
            // established ROTF target-server source, same as Queen of Hearts/Stonetaker). NAME RESOLVED: independent
            // frame-read of disc_n3i45dx58vU (chat_00019, boss-prefix tight-zoom) shows "Kestora" (a clear K) — that
            // tag's spec had mis-recorded "Restora"; disc_h2kSWfp0nX8 also reads "Kestora" -> 2-tag agreement, R/K
            // conflict resolved -> the 'boss-restora-the-lava-god' queue task is the misread DUPLICATE (no-build).
            // Only the DEATH/DEFEAT sequence was captured (gracious-loser lines); attack = spread fire + green-minion
            // summons (reconstructed; reuses OC 'Reptilian Hunter'). SERVER-LINEAGE NOTE: VoltiPlay's footage banners
            // "TFR v1.7.6"/"Elemental Realms" -> reconcile whether "The Forgotten Realm"/TFR == the ROTF server (cf.
            // disc_tT3IkMgb3mk flagged TFR non-ROTF on NON-VoltiPlay players; VoltiPlay-owner footage has always been
            // treated as ROTF here).
            // MECH-LOOP: Bullet shape DOCUMENTED: large elongated white rectangle/beam projectile seen in
            // disc_h2kSWfp0nX8 b002_10s_002 (single isolated bullet clearly visible between boss and player — wide
            // rectangular bar shape, not standard circle; consistent with projectileIndex: 1 alternate projectile).
            // Standard radial bullets (projectileIndex: 0) not clearly isolated in crowded fight frame → count+angle
            // RECONSTRUCTED for both Shoot calls. Arena DOCUMENTED: hexagonal/honeycomb tile lava dungeon — gray
            // hexagonal tile floor visible in h2k b002_10s_002 (active fight on hex floor), n3i b002_62s_003/005
            // (hex floor + death sequence); dungeon entry visible in h2k b002_10s_001 (stone wall archway, realm-void
            // above); minimap_00002 (n3i) shows amber-toned lava dungeon rooms (warm lava palette, not open realm).
            // Loot: "Ring of Doom UT" (+5 DEF, +140 HP, +40 MP, Fame Bonus 5%, Feed Power 900) observed in h2k
            // b004_67s_005 post-kill hover — probable Kestora drop (not yet added to threshold; verify attribution).
            .Init("Kestora the Lava God",  // arena: hexagonal/honeycomb tile LAVA DUNGEON — gray hexagonal tile floor (amber/lava-toned palette in color), stone wall archway entrance from realm; dungeon-enclosed fight chamber. DOCUMENTED-from-frames disc_h2kSWfp0nX8 b002_10s_001 (dungeon entry, stone wall + void above), b002_10s_002 (fight: hexagonal floor + boss sprite + beam bullet); disc_n3i45dx58vU b002_62s_003/005 (hexagonal floor, death sequence); minimap_00002 (n3i: amber lava-dungeon palette).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    // P1: lava-god spread fire + green-minion summons. NO combat-phase taunts were captured -> none invented.
                    new State("fight",
                        new Spawn("Reptilian Hunter", maxChildren: 4, initialSpawn: 0.5, coolDown: 7000),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 700),                              // radial ring (RECONSTRUCTED — standard projectile, count+angle not isolatable from fight frames)
                        new Shoot(14, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1200, projectileIndex: 1), // large rectangle/beam bullets (shape DOCUMENTED-from-frames h2k b002_10s_002); count+angle RECONSTRUCTED
                        new HpLessTransition(0.15, "dying")
                    ),
                    // DEATH/DEFEAT sequence — the ONLY captured taunts (VERBATIM, gracious-loser: ARGHH -> You defeated me... -> Good bye.)
                    new State("dying",
                        new Taunt("ARGHH"), // VERIFIED disc_n3i45dx58vU + disc_h2kSWfp0nX8 (death scream; boss-prefix confirmed)
                        new Flash(0xffaa3300, 0.3, 12),
                        new TimedTransition(1200, "defeated")
                    ),
                    new State("defeated",
                        new Taunt("You defeated me..."), // VERIFIED disc_n3i45dx58vU (gracious-loser line; punctuation ".."/"..." varies across tags)
                        new Taunt(0.5, "Good bye."), // VERIFIED disc_n3i45dx58vU + disc_h2kSWfp0nX8 (final line)
                        new Prioritize(new Wander(0.4)),
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 900)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Strange Magician — ROTF-CUSTOM mystic boss (object 0x8fe8). Source disc_mHY6O5kMU1I (combat-overlay
            // 'Enemy: Strange Magician' + feed '(26/30) Strange Magician has been defeated' confirm the boss). DISTINCT
            // from the recovered 'Strange Priest' (disc_tbG9HBrXyI8). *** VERIFY-LOOP CORRECTION (build was misattributed) ***:
            //  - 'We are impervious to non-mystic attacks!' is actually ROCK CONSTRUCT's line (f_00122 boss-prefix), NOT
            //    Strange Magician -> REMOVED, along with the invented immunity/barrier gimmick that was built on it.
            //  - '...it ascends you... EXPLODE!' was a MIS-JOIN: '...it ascends you' is a PLAYER line ('<D-2> realnick'),
            //    only 'EXPLODE!' is the boss's -> CORRECTED to 'EXPLODE!'.
            // Strange Magician's ONLY frame-attributed taunts (boss-prefix, chat_30/33) are 'EXPLODE!' + 'ARRRG! THIS IS
            // IMPOSSIBLE!'. Attack geometry NOT traced (reconstructed spread/aimed). SERVER-OVERLAP FLAG: Rock Construct
            // (a Valor-server boss per discovery-mispull memory) also appears in this tag -> reconcile this tag's server.
            // MECH-LOOP: Arena DOCUMENTED: open REALM event (dark realm floor, mushroom-tree scenery) — "Strange Magician
            // spawned in Realm!" chat in b002_3s_006; minimap_00002 amber (open realm). Bullet shape PARTIALLY DOCUMENTED:
            // small white bar/rectangle projectiles visible in b002_3s_012 (two downward-moving bars below boss during fight);
            // shape is small/standard — consistent with existing Shoot params but not a uniquely identifiable shape.
            // NEW TAUNT: "You are food for my minions!" found in chat during fight (b002_3s_012); boss prefix cut off
            // (left-edge crop) but thematically consistent + fight context; added to cast state, flag for taunt-loop verify.
            .Init("Strange Magician",  // arena: open REALM event — dark realm floor, mushroom-tree/realm-scenery open area (no dungeon enclosure). DOCUMENTED-from-frames disc_mHY6O5kMU1I b002_3s_001 (fight: mushroom-tree realm scenery), b002_3s_006 (post-kill chat "Strange Magician spawned in Realm!"); minimap_00002 (amber = open realm).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "cast")),
                    // mystic attacks (the only frame-attributed combat line is "EXPLODE!")
                    new State("cast",
                        new Taunt(0.0016, "EXPLODE!"), // CORRECTED disc_mHY6O5kMU1I (was "...it ascends you... EXPLODE!" — "...it ascends you" was a PLAYER line "<D-2> realnick", mis-joined; boss line is just "EXPLODE!", chat_30/f_00122 "Strange Magician:" prefix)
                        new Taunt(0.0016, "You are food for my minions!"), // DOCUMENTED-from-frames disc_mHY6O5kMU1I b002_3s_012 (fight frame chat; boss-prefix cut off by left-edge crop — attributable by fight context; flag for taunt-loop verify)
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 700),                              // small white bar projectiles (shape PARTIALLY-DOCUMENTED b002_3s_012; count+angle RECONSTRUCTED)
                        new Shoot(13, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1200, projectileIndex: 1), // aimed burst (RECONSTRUCTED — projectileIndex:1 not isolated in fight frames)
                        new HpLessTransition(0.15, "dying")
                    ),
                    // DEATH cry
                    new State("dying",
                        new Taunt("ARRRG! THIS IS IMPOSSIBLE!"), // VERIFIED disc_mHY6O5kMU1I (death cry, chat_30/33; "Strange Magician:" prefix confirmed)
                        new Flash(0xffff0000, 0.2, 10),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 800)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Strange Priest — ROTF-CUSTOM boss (object 0x8fe9). Source disc_tbG9HBrXyI8 (boss-coloured "<Strange Priest>"
            // prefix; wide f_00106 re-crop gives the FULL verbatim lines). DISTINCT from Strange Magician (0x8fe8).
            // Gimmick: a self-styled "BEST PRIEST IN THE WORLD" who underestimates the player, then PANICS when overpowered
            // ("WHAT IS HAPPENING?!" / "Priest! Help me!") before dying. Attack geometry NOT recovered -> generic radial+aimed.
            // MECH-LOOP: Bullet shape DOCUMENTED: crescent/arc-shaped bullets — clearly visible in b003_42s_005 (active fight
            // frame: many small crescent/boomerang-arc shaped projectiles scattered around boss in all directions; very distinct
            // shape). Count+angle not resolvable from the crowded frame → RECONSTRUCTED. Arena DOCUMENTED: abbey/church-themed
            // DUNGEON — ribbed vertical-stripe stone walls with chain-link/dotted borders, T-shaped dark corridor layout visible
            // in b003_42s_018 (corridor fight); inner fight chamber has light white cross-grid tile floor visible in b003_42s_005;
            // minimap_00002 (dark = dungeon). Loot: "Wand of Pain" (Legendary Item, "[RuinedBow] has looted a Legendary Item
            // [Wand of Pain], with 16% damage") observed in b003_42s_018 — probable Strange Priest UT drop (not yet added
            // to threshold; verify attribution).
            .Init("Strange Priest",  // arena: abbey/church-themed DUNGEON — ribbed striped stone walls with chain-link/dotted borders, T-shaped dark corridor room layout; inner chamber has light white cross-grid tile floor (fight arena). DOCUMENTED-from-frames disc_tbG9HBrXyI8 b003_42s_005 (fight: light tile inner chamber + crescent/arc bullets + boss sprite), b003_42s_018 (corridor fight: abbey walls + boss entity + taunts + Wand-of-Pain loot); minimap_00002 (dark = dungeon).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "confident")),
                    // confident opening
                    new State("confident",
                        new Taunt(0.0016, "Let's fight and see who have more Power"), // VERIFIED disc_tbG9HBrXyI8 (verify-loop frame-re-read f_00106 wide crop, "Enemy: Strange Priest" overlay + boss-prefix) (verbatim, wide f_00106 re-crop, "<Strange Priest>" prefix)
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 800),                              // crescent/arc bullets (shape DOCUMENTED-from-frames b003_42s_005; count+angle RECONSTRUCTED)
                        new Shoot(13, count: 4, shootAngle: 20, predictive: 0.4, coolDown: 1200, projectileIndex: 1), // aimed burst (RECONSTRUCTED — projectileIndex:1 not isolated from fight frames)
                        new HpLessTransition(0.4, "panic")
                    ),
                    // panic when overpowered
                    new State("panic",
                        new Taunt(0.0016, "UH?! WHAT IS HAPPENING?! I'M THE BEST PRIEST IN THE WORLD!!"), // VERIFIED disc_tbG9HBrXyI8 (verify-loop frame-re-read f_00106 wide crop, "Enemy: Strange Priest" overlay + boss-prefix) (verbatim, wide f_00106 re-crop)
                        new Taunt(0.0016, "How!? Priest! Help me!"), // VERIFIED disc_tbG9HBrXyI8 (verify-loop frame-re-read f_00106 wide crop, "Enemy: Strange Priest" overlay + boss-prefix) (verbatim; the Priest's own panic line, boss-prefix confirmed)
                        new Flash(0xffffff00, 0.3, 6),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 650),
                        new HpLessTransition(0.12, "dying")
                    ),
                    // death
                    new State("dying",
                        new Taunt("Well... i think i substestiateted you."), // VERIFIED disc_tbG9HBrXyI8 (verify-loop frame-re-read f_00106 wide crop, "Enemy: Strange Priest" overlay + boss-prefix) (verbatim INCL. the garbled on-screen word "substestiateted" — kept as-recovered, NOT auto-corrected)
                        new Flash(0xffff0000, 0.2, 10),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 800)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Mad Jester — ROTF-CUSTOM event boss (object 0x8fea). Well-attested (29 tags' feeds); Oryx summons it as the
            // "TRUE FOOL". Sources: disc_lHWOmAlg-ag (boss-coloured "Mad Jester:" prefix, wide re-crop = VERBATIM) +
            // disc__oNRwi83bLU (f_156, 2nd line). NOT base-game ("Mad Jester" absent from base XML; only "Jester Cloth" items).
            // A circus/magic showman. Attack geometry NOT recovered -> generic radial+aimed (chaotic jester mix).
            // MECH-LOOP: Attacks UNVERIFIABLE-ILLEGIBLE — both bossend sources (disc__oNRwi83bLU b005_31s, disc_lHWOmAlg-ag
            // b001_2s) are marathon videos; their bossend groups span many different boss fights (Ghost Goddess, Beast, Berikao,
            // Colossus, Pumpkin King, etc.) and specific Mad Jester fight frames cannot be isolated to attribute bullet geometry.
            // Arena DOCUMENTED: wavy/ripple tile DUNGEON (circus/theater aesthetic) — distinctive wavy-ripple tile floor visible
            // in disc__oNRwi83bLU f_00156 (post-fight frame in Mad Jester's dungeon: cross-shaped room layout, rectangular altar/
            // platform in upper chamber); disc_lHWOmAlg-ag minimap_00002 (dark = dungeon).
            .Init("Mad Jester",  // arena: wavy/ripple tile DUNGEON (circus/theater theme) — distinctive wavy-ripple tile floor, cross-shaped room layout, rectangular altar/stage platform in upper chamber. DOCUMENTED-from-frames disc__oNRwi83bLU f_00156 (post-fight in Mad Jester dungeon: wavy floor + cross-room layout visible); disc_lHWOmAlg-ag minimap_00002 (dark = dungeon).

                new State(
                    new State("idle", new PlayerWithinTransition(14, "show")),
                    // the "Ultimate Show" opening
                    new State("show",
                        new Taunt(0.0016, "It's time... for the Ultimate Show!"), // VERIFIED disc__oNRwi83bLU (verify-loop frame-re-read; boss-prefix "Mad Jester:" + Oryx "Silly fools, I have summoned the TRUE FOOL!" + "died to Mad Jester (XP4 Wizard)" death lines confirm. "the"/"The" capitalization ambiguous at source res -> kept build's "the", conservative)
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 750),
                        new Shoot(13, count: 5, shootAngle: 20, predictive: 0.4, coolDown: 1300, projectileIndex: 1),
                        new HpLessTransition(0.5, "magic")
                    ),
                    // confident "magic + determination" phase
                    new State("magic",
                        new Taunt(0.0016, "To destroy these humans, all it takes is a bit of little magic and a lot of determination. I have BOTH!"), // VERIFIED disc_lHWOmAlg-ag (verify-loop frame-re-read, wide f_ re-crop "Mad Jester -" prefix; EXACT verbatim match)
                        new Flash(0xffff00ff, 0.3, 6),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 14, shootAngle: 25, coolDown: 650),
                        new Shoot(13, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 1100, projectileIndex: 1)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // MECH-LOOP: Bullet shape DOCUMENTED: large white cloud/puff projectiles in radial nova/ring —
            // clearly visible in disc_wwhGYuiu_9U b022_183s_003 (multiple cloud bullets around dark gear-shaped
            // boss in stone brick dungeon) and b022_183s_005 (ring of ~12 cloud/puff bullets at equal angular
            // spacing). Count ~12 PARTIALLY-DOCUMENTED (ring clips frame edges); exact count+angle RECONSTRUCTED.
            // Minion observed in b022_183s_005 — bat-type entity label visible but no bare "Bat" id in XML;
            // Spawn wired out pending entity id verification. Chat_00040 confirms "Nikad the Defiler" taunts at
            // ~183s in disc_wwhGYuiu_9U, frame group b022_183s. Arena DOCUMENTED: small 2-room dark stone brick
            // DUNGEON, diagonal-stripe floor tiles — b022_183s_002 (two-room minimap), b022_183s_007 (diagonal
            // stripe floor post-fight); minimap dark=dungeon confirmed.
            // Nikad the Defiler — ROTF-CUSTOM boss (object 0x8feb). A minion-SUMMONER 'Defiler'. Sources CROSS-CORROBORATE:
            // disc_wwhGYuiu_9U (boss-prefix 'Nikad', fuller set) + disc_bk4dygntBZ8 ('Nikad the Defiler', partial). NOT base.
            // Gimmick: boasts its minions will finish you, then panics + dies. Minion-SPAWN wired out (entity
            // observed as bat-type in b022_183s_005, no bare "Bat" XML id found; wire when entity is confirmed).
            .Init("Nikad the Defiler",  // arena: small 2-room dark stone brick DUNGEON — diagonal-stripe floor tiles. DOCUMENTED b022_183s.
                new State(
                    new State("idle", new PlayerWithinTransition(14, "summon")),
                    // minion-summoner opening
                    new State("summon",
                        new Taunt(0.0016, "If I don't kill you, my minions will!"), // VERIFIED disc_wwhGYuiu_9U (verify-loop frame-re-read, "Nikad the Defiler" boss-prefix confirmed on the death line) (verbatim complete, boss-prefix 'Nikad')
                        new Taunt(0.0016, "Look out! My minions will help me!"), // VERIFIED disc_wwhGYuiu_9U (verify-loop frame-re-read, "Nikad the Defiler" boss-prefix confirmed on the death line) (verbatim complete)
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 750),            // large cloud/puff bullets, radial nova ring (shape DOCUMENTED-from-frames b022_183s_003/005; count ~12 PARTIALLY-DOCUMENTED, angle RECONSTRUCTED)
                        new Shoot(13, count: 5, shootAngle: 20, predictive: 0.4, coolDown: 1300, projectileIndex: 1), // aimed burst (RECONSTRUCTED)
                        new HpLessTransition(0.5, "enrage")
                    ),
                    // enrage
                    new State("enrage",
                        new Taunt(0.0016, "You're a nasty little pest!"), // VERIFIED disc_wwhGYuiu_9U (verify-loop frame-re-read, "Nikad the Defiler" boss-prefix confirmed on the death line) (verbatim complete)
                        new Taunt(0.0016, "You cannot handle the full power of..."), // VERIFIED-partial disc_wwhGYuiu_9U (verify-loop frame-re-read; tail-CUT 'power of [...]' confirmed; bk4dygntBZ8 corroborates 'You cannot handle the [...]')
                        new Flash(0xff8800ff, 0.3, 6),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 14, shootAngle: 25, coolDown: 650),            // (RECONSTRUCTED — enrage state frames not isolated)
                        new HpLessTransition(0.12, "dying")
                    ),
                    // death
                    new State("dying",
                        new Taunt("Nooooooo! This cannot be!"), // VERIFIED disc_wwhGYuiu_9U (verify-loop frame-re-read, "Nikad the Defiler" boss-prefix confirmed on the death line) (complete portion of 'Nooooooo! This cannot be! I have l[ost]...'; bk4dygntBZ8 corroborates 'Noooooo! This cannot [be]...'); cut tail 'I have lost' NOT guessed
                        new Flash(0xffff0000, 0.2, 10),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 800)              // (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // MECH-LOOP: Mushroom sprite DOCUMENTED: large mushroom-cap sprite clearly visible in disc_M04fBHBpodE
            // b002_21s_010 (active fight frame; health bar visible above sprite; open realm wavy-water floor).
            // Arena DOCUMENTED: open REALM — wavy floor M04 b002_21s_010, hexagonal circle floor E03 b002_17s_001/012;
            // both tag minimaps (E03 minimap_00001 scattered portal/enemy dots = realm, M04 minimap_00003 dark=realm).
            // Attacks UNVERIFIABLE-ILLEGIBLE: Marble Colossus, Fire Elemental, Ghost Ship all active simultaneously
            // in same realm as Mushroom — white sparkle entities near sprite in M04 b002_21s_010 possibly its
            // projectiles but cannot be attributed to Mushroom specifically given concurrent multi-boss realm fight.
            // Mushroom — ROTF-CUSTOM realm-event MINI-boss (object 0x8fec). Source disc_E03Je5EgiZQ ('Mushroom spawned in
            // Realm!' + /30 counter; boss-prefix line). DISTINCT from base 'Large/Small/Magic/Arena Mushroom' enemies (those
            // exist in base XML; bare 'Mushroom' is free). EVASIVE taunty mini-boss ('No! You can't get me!' -> it darts about).
            // Only ONE taunt is built: the 2nd recovered line ('Well, you're getting so far, did you think it was true wh[...]')
            // is GARBLED/cut + could not be cleanly re-read -> OMITTED (not built) pending cleaner footage. Generic radial+aimed.
            .Init("Mushroom",  // arena: open REALM event. DOCUMENTED M04 b002_21s_010 (wavy floor) + E03 b002_17s (hex-circle floor); minimap amber/dark=realm.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "flee")),
                    new State("flee",
                        new Taunt(0.0016, "No! You can't get me!"), // VERIFIED disc_E03Je5EgiZQ (frame-read chat, boss-prefix; clean complete line)
                        new Prioritize(new StayCloseToSpawn(0.4, 8), new Wander(0.8)),
                        new Shoot(12, count: 8, shootAngle: 45, coolDown: 700),
                        new Shoot(12, count: 4, shootAngle: 20, predictive: 0.5, coolDown: 1200, projectileIndex: 1)
                    )
                ),
                new Threshold(0.2, new TierLoot(9, ItemType.Weapon, 0.06), new TierLoot(10, ItemType.Armor, 0.06), new TierLoot(4, ItemType.Potion, 0.1))
            )
            // Bedlam - God of Chaos — ROTF-CUSTOM Court-of-Oryx summoned boss (object 0x8fed). Source disc_EM9Uba-Nv08
            // (owner VoltiPlay; orange boss-coloured 'Bedlam - God of Chaos', summoned via 'Miru is quaking you to Court of
            // Oryx!'). NOT a base Court-of-Oryx boss. Both taunts VERBATIM (incl. the quoted 'ALL').
            // MECH-LOOP 2026-06-24: attacks PARTIAL — bullet type CONFIRMED disc_EM9Uba-Nv08 f_00100 + f_00102:
            // small round WHITE/CREAM projectiles clearly visible radiating from boss in wide spread; ~12 white
            // dots visible in f_00100 → CONFIRMS Shoot(13, count:12, shootAngle:30); proj-index 13 = small white
            // bullet matches observations. Both taunts frame-confirmed in f_00100 chat. Enrage phase bullet type
            // identical (same white small rounds in f_00102). count/angle remain RECONSTRUCTED.
            .Init("Bedlam - God of Chaos",  // arena: asylum/madhouse dungeon — dark stone tile dungeon; floor: very dark gray/charcoal stone tiles with visible grid seams; dungeon enemies: "Asylum Guard" (death log at f_00104 confirms asylum theme). DOCUMENTED-from-frames disc_EM9Uba-Nv08 f_00100 (Bedlam taunts in left-side chat confirm fight section; very dark gray charcoal tile floor; VoltiPlay + small player group; boss entity + white bullet spread visible) + f_00104 (Asylum Guard death spam x3 verbatim confirms asylum dungeon theme). Dungeon layout from minimap_00013 (irregular multi-room dark dungeon: asymmetric rooms connected by corridors; CAVEAT — minimap_00013 at ≈chat_00025-026 is at boundary with Crystal Prisoner Clone section; layout attribution best-effort).
                new State(
                    new State("idle", new PlayerWithinTransition(14, "chaos")),
                    // chaos opening
                    new State("chaos",
                        new Taunt(0.0016, "Bring forth... THE CHAOS!"), // VERIFIED disc_EM9Uba-Nv08 (verify-loop wide f_ re-read, "<Bedlam - God of Chaos>" boss-prefix; EXACT) (verbatim, orange boss-prefix 'Bedlam - God of Chaos')
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.4)),
                        // PARTIAL (f_00100): bullet = small round WHITE/CREAM; ~12 white dots in wide spread — CONFIRMS proj-13.
                        // count:12 shootAngle:30 (full ring) + predictive burst RECONSTRUCTED.
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 700),
                        new Shoot(13, count: 6, shootAngle: 15, predictive: 0.4, coolDown: 1200, projectileIndex: 1),
                        new HpLessTransition(0.4, "enrage")
                    ),
                    // enrage
                    new State("enrage",
                        new Taunt(0.0016, "You will 'ALL' die here!"), // VERIFIED disc_EM9Uba-Nv08 (verify-loop wide f_ re-read, "<Bedlam - God of Chaos>" boss-prefix; EXACT) (verbatim incl. the quoted 'ALL'; boss-prefix)
                        new Flash(0xffaa00ff, 0.3, 6),
                        new Prioritize(new Wander(0.6)),
                        new Shoot(13, count: 16, shootAngle: 22, coolDown: 600),
                        new Shoot(13, count: 8, shootAngle: 12, predictive: 0.5, coolDown: 1000, projectileIndex: 1)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // The Magical lord of sky — ROTF-CUSTOM multi-part boss (object 0x8fee). Source disc_Fb0zINzmQ1s (GENUINE ROTF
            // promo/changelog, owner VoltiPlay, 'ELEMENTAL REALMS' launcher). A central body + two destructible HANDS (the
            // hands also speak 'Haha! Don't worry.'). Sky/magic theme. NOTE: the two destructible HAND sub-entities are NOT
            // built here (would need their own objects + EntityNotExists transitions; central body only) -> wire later.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_Fb0zINzmQ1s f_00303/f_00308 (active fight, VoltiPlay solo):
            // elongated thin WHITE/SILVER NEEDLE/LANCE-shaped projectiles clearly visible; ~10-12 per burst in fight area.
            // Distinctive shape (NOT circular) — some from hand sub-entities, attribution uncertain. proj-13 or proj-1 →
            // needle sprite CONFIRMED; count:12 shootAngle:30 RECONSTRUCTED. Arena: dark green rounded cobblestone CONFIRMED.
            // MECH-LOOP: needle bullet inline notes on Shoot() below
            .Init("The Magical lord of sky",  // arena: sky/magic dungeon — single open rectangular boss room; floor: dark gray/greenish-gray rounded cobblestone tiles (dark stone dungeon appearance, distinctly rounded cobblestone tile texture). Boss composition: large dark blue/purple main body + two named destructible hands ("Right hand of Magical lord of sky" / "Left hand of Magical lord of sky") all visible simultaneously in fight. DOCUMENTED-from-frames disc_Fb0zINzmQ1s f_00305 (active fight: main blue/purple boss entity + hand sub-entities + cobblestone floor clearly visible; VoltiPlay solo fight, bullet spread visible) + f_00320 (Quest Complete; taunts confirmed in chat: "Right/Left hand of Magical lord of sky: Haha! Don't worry." + "The Magical lord of sky: I will catch you baby..."). Minimap layout from minimap_00038/00040: single open room (no corridors — orange enemy/loot dots cluster in one area, dark dungeon background).
                new State(
                    new State("idle", new PlayerWithinTransition(14, "sky")),
                    new State("sky",
                        new Taunt(0.0016, "I will catch you baby..."), // VERIFIED disc_Fb0zINzmQ1s (verify-loop wide f_ re-read, "<The Magical lord of sky>" boss-prefix; on-screen ".." trailing kept as "...")
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.4)),
                        // PARTIAL (f_00303/f_00308): elongated WHITE/SILVER needle/lance bullets CONFIRMED; ~10-12 visible.
                        // Source may be hands + body simultaneously; proj-13 or proj-1 needle sprite. count/angle RECONSTRUCTED.
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 700),
                        new Shoot(13, count: 6, shootAngle: 15, predictive: 0.4, coolDown: 1200, projectileIndex: 1),
                        new HpLessTransition(0.4, "magic")
                    ),
                    new State("magic",
                        new Taunt(0.0016, "Haha! Don't worry."), // VERIFIED disc_Fb0zINzmQ1s (verify-loop wide f_ re-read; spoken by "<Right hand of Magical lord of sky>" + "<Left hand of Magical lord of sky>" — confirms the named L/R hand sub-entities)
                        new Flash(0xff66ccff, 0.3, 6),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 16, shootAngle: 22, coolDown: 650),
                        new Shoot(13, count: 8, shootAngle: 12, predictive: 0.5, coolDown: 1000, projectileIndex: 1)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Time Lord — ROTF-CUSTOM MEME boss (object 0x8fef). A Doctor-Who 'Time Lord' / time-pun boss (fits the ROTF
            // heritage-meme branding). Source disc_hp_dAXf_alY (GENUINE ROTF, owner VoltiPlay; boss-coloured 'Time Lord').
            // Only the 2 CLEAN confirmed taunts are built. The other 'Time Lord' spec (disc_-xoX7gatTXE: 'The essence of time
            // favors me!' etc.) had UNCERTAIN attribution (provisional cosmic boss) -> deliberately NOT included here.
            // MECH-LOOP: Boss sprite DOCUMENTED: large white/silver cube entity clearly visible in b002_43s_014/015 (active fight
            // frames, boss HP "20477 ♥ 371" at top, dark stone-brick dungeon floor). Bullet geometry UNVERIFIABLE-ILLEGIBLE:
            // combat active (damage numbers +8/+17 visible in b002_43s_013), small projectiles present near boss but too small
            // and scattered at 2fps to trace ring count or angle distribution. Shoot() calls remain RECONSTRUCTED.
            // Arena DOCUMENTED: small single-room dark stone-brick DUNGEON — dark crisscross-grid tile floor, gray stone block
            // corner columns; minimap_00008 shows tiny blue rectangle on black = small dungeon room confirmed.
            // Both taunts re-confirmed on-screen: chat_00019 shows "<Time Lord> Do you know what time it is?" +
            // "<Time Lord> It's time for you to die!" (boss-coloured prefix). Loot: "Cyclops Dagger UT" visible in
            // b002_43s_016 tooltip + "time blade" in VoltiPlay chat — neither item exists in current XML, not added.
            .Init("Time Lord",  // arena: small single-room dark stone-brick DUNGEON. DOCUMENTED b002_43s_014/015 + minimap_00008.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "time")),
                    new State("time",
                        new Taunt(0.0016, "Do you know what time it is?"), // VERIFIED disc_hp_dAXf_alY chat_00019 (boss-coloured "<Time Lord>" prefix on-screen)
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.4)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 700),            // (RECONSTRUCTED — bullets present but geometry illegible at 2fps)
                        new Shoot(13, count: 6, shootAngle: 15, predictive: 0.4, coolDown: 1200, projectileIndex: 1), // (RECONSTRUCTED)
                        new HpLessTransition(0.4, "death")
                    ),
                    new State("death",
                        new Taunt(0.0016, "It's time for you to die!"), // VERIFIED disc_hp_dAXf_alY chat_00019 (boss-coloured "<Time Lord>" prefix on-screen, concurrent with above)
                        new Flash(0xff00ffcc, 0.3, 6),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 16, shootAngle: 22, coolDown: 650),            // (RECONSTRUCTED)
                        new Shoot(13, count: 8, shootAngle: 12, predictive: 0.5, coolDown: 1000, projectileIndex: 1) // (RECONSTRUCTED)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Helios — ROTF-CUSTOM galactic SUN/SOLAR boss (object 0x8ff4; Phase-D3 Galactic Zones). Source disc_l8s6Dho3WM4
            // (GENUINE ROTF, owner Volti; a GALACTIC/SPACE 'Ruins of the Mad God' zone). 2 VERBATIM taunts. SUPERNOVA gimmick:
            // a building 'judgment' -> a big radial NOVA. Attack reconstructed from the SUPERNOVA theme (radial nova) -> flagged.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_l8s6Dho3WM4 f_00305: blue SPIRAL/SWIRL projectiles confirmed during
            // SUPERNOVA state ("Witness the SUPERNOVA!" taunt in chat; swirling blue bullets fill arena). Bullet shape CONFIRMED;
            // count:24/angle:15 for nova burst remain RECONSTRUCTED. attacks_status upgraded no-footage → partial.
            .Init("Helios",  // arena: galactic solar/sun dungeon — large open boss chamber; floor: sandy/amber-tan stone tiles (warm sand-colored individual square tiles, distinct sandy-tan appearance); dark stone block walls surrounding rectangular fight area. Boss entity: large golden bright Helios entity. DOCUMENTED-from-frames disc_l8s6Dho3WM4 f_00301 (large crowd fight showing full golden-sandy floor expanse + many players) + f_00305 (active fight: large golden Helios entity visible upper-left, 'Witness the SUPERNOVA!' taunt in bottom-left chat, blue spiral bullet patterns throughout, sandy-tan tile floor + dark stone block walls confirmed; Volti HP bar visible in right panel) + minimap_00037 (large main room with warm amber/golden floor in dark dungeon background — single large open chamber; room footprint fills most of minimap).
                new State(
                    new State("idle", new PlayerWithinTransition(14, "judgment")),
                    new State("judgment",
                        new Taunt(0.0016, "Judgment comes! You cannot hide from the light!"), // CORRECTED disc_l8s6Dho3WM4 (was: "Judgment comes! You cannot hide from this!"; frame-read f_00301-305 shows "the light!")
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.4)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 700),
                        new Shoot(13, count: 6, shootAngle: 15, predictive: 0.4, coolDown: 1200, projectileIndex: 1),
                        new HpLessTransition(0.4, "supernova")
                    ),
                    // SUPERNOVA — big radial nova
                    new State("supernova",
                        new Taunt(0.0016, "Witness the SUPERNOVA!"), // VERIFIED disc_l8s6Dho3WM4 (frame-read f_00301-305, exact match)
                        new Flash(0xffffaa00, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.2, 4), new Wander(0.3)),
                        new Shoot(13, count: 24, shootAngle: 15, coolDown: 600),                // radial nova (RECONSTRUCTED from 'SUPERNOVA' theme; not frame-traced)
                        new Shoot(13, count: 12, shootAngle: 30, predictive: 0.4, coolDown: 900, projectileIndex: 1)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // Ruins of the Mad God Core — ROTF-CUSTOM galactic mecha/reactor CORE boss (object 0x8ff5; Phase-D3 Galactic Zones).
            // Source disc_l8s6Dho3WM4 (GENUINE ROTF, owner Volti; galactic 'Ruins of the Mad God' zone). The chat boss-prefix is
            // verbatim "<Ruins of the Mad God>" (the core/zone speaks as the dungeon name) -> named the OBJECT "Ruins of the Mad
            // God Core" to DISAMBIGUATE the boss from the zone (Phase-D3 will add a 'Ruins of the Mad God' zone/portal -> avoid id
            // collision). LIMITERS/CORE enrage gimmick. Attack reconstructed (radial nova fits a reactor 'core') -> flagged.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — golden ring-shaped projectiles CONFIRMED disc_l8s6Dho3WM4 f_00240
            // ("DODGE THIS!" volley; swirling gold rings, very distinctive vs all other bullet types). attacks_status upgraded.
            .Init("Ruins of the Mad God Core",  // arena: galactic void/space dungeon — single large circular/oval open boss chamber floating in pitch-black void; floor: very dark gray/near-black stone tiles with subtle dark tile pattern (space/void dungeon aesthetic; floor barely distinguishable from surrounding void). Boss entity: large dark gray/black rocky boulder/stone entity; bullet pattern: GOLDEN RING-SHAPED projectiles (swirling gold rings — distinctive 'core' projectile). DOCUMENTED-from-frames disc_l8s6Dho3WM4 f_00240 (active fight: taunts in bottom-left chat "The core awakens! DODGE THIS!"; large dark rocky boss entity center; gold ring bullets throughout arena; very dark gray floor + void walls) + minimap_00030 (single large circular/oval open room in pitch-black void background; no corridors — one boss chamber).
                new State(
                    new State("idle", new PlayerWithinTransition(14, "awaken")),
                    new State("awaken",
                        new Taunt(0.0016, "The core awakens! DODGE THIS!"), // CORRECTED disc_l8s6Dho3WM4 f_00240 (was: partial "DODGE..." right-edge cut; f_00240 bottom-left chat now shows full "DODGE THIS!" verbatim)
                        new Prioritize(new StayCloseToSpawn(0.2, 5), new Wander(0.3)),
                        new Shoot(13, count: 14, shootAngle: 26, coolDown: 700),
                        new Shoot(13, count: 7, shootAngle: 13, predictive: 0.4, coolDown: 1200, projectileIndex: 1),
                        new HpLessTransition(0.4, "limiters")
                    ),
                    // LIMITERS RELEASED — enrage radial nova
                    new State("limiters",
                        new Taunt(0.0016, "LIMITERS RELEASED! DIE!"), // VERIFIED disc_l8s6Dho3WM4 (verify-loop frame-re-read, "<Ruins of the Mad God>" boss-prefix; exact). NOTE: 2 more lines exist (enrich leads): "DEFENSE SYSTEMS ENGAGED..." (cut) + "I am not done yet!"
                        new Flash(0xffff3300, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.15, 4), new Wander(0.25)),
                        new Shoot(13, count: 26, shootAngle: 13.846, coolDown: 600),              // dense radial nova (RECONSTRUCTED 'core/limiters' theme; not frame-traced)
                        new Shoot(13, count: 13, shootAngle: 27, predictive: 0.4, coolDown: 900, projectileIndex: 1)
                    )
                ),
                new Threshold(0.1, new TierLoot(11, ItemType.Weapon, 0.08), new TierLoot(12, ItemType.Armor, 0.08), new TierLoot(5, ItemType.Ring, 0.1))
            )
            // MAGMA — fire/lava boss. Death-confirmed NewDungeons f_00125. Blazetalon drop (wiki: "magmarock" blade).
            // Ice-crystal white projectile observed in frames. 2-phase: lava barrage → meltdown. DESIGNED stub 2026-06-24.
            .Init("Magma",
                new State(
                    new State("idle", new PlayerWithinTransition(14, "fight")),
                    new State("fight",
                        new Taunt("You dare enter my domain?"), // RECONSTRUCTED — fire boss opener
                        new Taunt(0.002, "Feel the heat of Magma!"), // RECONSTRUCTED
                        new Flash(0xff4400, 0.3, 10),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.35)),
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 850),
                        new Shoot(14, projectileIndex: 1, count: 4, shootAngle: 20, predictive: 0.5, coolDown: 1400),
                        new HpLessTransition(0.4, "meltdown")
                    ),
                    new State("meltdown",
                        new Taunt("MELTDOWN! EVERYTHING BURNS!"), // RECONSTRUCTED — enrage
                        new Flash(0xff2200, 0.2, 15),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(14, count: 14, shootAngle: 25, coolDown: 650),
                        new Shoot(14, projectileIndex: 1, count: 6, shootAngle: 18, predictive: 0.6, coolDown: 1000)
                    )
                ),
                new Threshold(0.1,
                    new ItemLoot("Blazetalon", 0.07),
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            // BHENNA — partial name from EpicBosses HP bar ("Bhenna the [suffix clipped]"). Large purple
            // humanoid/witch, checkerboard + wood-plank dungeon. No taunts recovered. DESIGNED stub 2026-06-24.
            .Init("Bhenna",
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("You cannot escape my hex!"), // RECONSTRUCTED — witch theme
                        new Taunt(0.0018, "The dark magic will consume you."), // RECONSTRUCTED
                        new Flash(0x9900cc, 0.3, 9),
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 900),
                        new Shoot(13, projectileIndex: 1, count: 4, shootAngle: 20, predictive: 0.5, coolDown: 1500),
                        new HpLessTransition(0.35, "ascend")
                    ),
                    new State("ascend",
                        new Taunt("You are nothing before my true form!"), // RECONSTRUCTED
                        new Flash(0xcc00ff, 0.25, 12),
                        new Prioritize(new Wander(0.45)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 700),
                        new Shoot(13, projectileIndex: 1, count: 5, shootAngle: 15, predictive: 0.6, coolDown: 1100)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            // ENTITY — Shadow/time boss. NewDungeons/ f_00263-280 confirms "Entity:" chat prefix (3 reads).
            // 242,494 HP from HP bar. Dark purple hex floor, white ghost sprite. 2 VERBATIM taunts 2026-06-24.
            .Init("Entity",
                new State(
                    new State("idle", new PlayerWithinTransition(15, "fight")),
                    new State("fight",
                        new Taunt("Ah, heroes, blessed souls, I shall destroy you with the power of shadow!"), // VERBATIM NewDungeons f_00268/270/272
                        new Taunt(0.0016, "The essence of time forces me..."), // VERBATIM NewDungeons f_00272/275/278 (trailing text cut at right edge)
                        new Flash(0x330066, 0.3, 10),
                        new Prioritize(new StayCloseToSpawn(0.2, 8), new Wander(0.3)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 800),
                        new Shoot(15, projectileIndex: 1, count: 5, shootAngle: 18, predictive: 0.5, coolDown: 1400),
                        new HpLessTransition(0.4, "temporal")
                    ),
                    new State("temporal",
                        new Taunt("Time itself bends to my will!"), // RECONSTRUCTED — refs time theme ("essence of time")
                        new Flash(0x6600cc, 0.25, 14),
                        new Prioritize(new Wander(0.45)),
                        new Shoot(15, count: 16, shootAngle: 22, coolDown: 650),
                        new Shoot(15, projectileIndex: 1, count: 7, shootAngle: 14, predictive: 0.6, coolDown: 1000)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(13, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(6, ItemType.Ring, 0.1),
                    new ItemLoot("Potion of Life", 0.25)
                )
            )
            // EPIC REALM GIANT — EventBosses/ feed confirmed ("killed by Epic Realm Giant"). Realm event boss.
            // DESIGNED stub 2026-06-24. Stronger variant of base Giant Oryx Chicken.
            .Init("Epic Realm Giant",
                new State(
                    new State("idle", new PlayerWithinTransition(16, "fight")),
                    new State("fight",
                        new Taunt("The realm itself trembles before me!"), // RECONSTRUCTED
                        new Flash(0xff9900, 0.3, 10),
                        new Prioritize(new StayCloseToSpawn(0.2, 8), new Wander(0.3)),
                        new Shoot(16, count: 12, shootAngle: 30, coolDown: 750),
                        new Shoot(16, projectileIndex: 1, count: 6, shootAngle: 20, predictive: 0.4, coolDown: 1300),
                        new HpLessTransition(0.4, "rampage")
                    ),
                    new State("rampage",
                        new Taunt("RAMPAGE!"), // RECONSTRUCTED — event boss enrage
                        new Flash(0xff5500, 0.2, 14),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(16, count: 18, shootAngle: 20, coolDown: 600),
                        new Shoot(16, projectileIndex: 1, count: 8, shootAngle: 15, predictive: 0.5, coolDown: 900)
                    )
                ),
                new Threshold(0.05,
                    new TierLoot(13, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(6, ItemType.Ring, 0.1),
                    new ItemLoot("Potion of Life", 0.3)
                )
            )
            // EPIC DEEP SEA BEAST — EventBosses/ feed confirmed. Epic variant of Ocean Trench boss.
            // DESIGNED stub 2026-06-24.
            .Init("Epic Deep Sea Beast",
                new State(
                    new State("idle", new PlayerWithinTransition(14, "fight")),
                    new State("fight",
                        new Taunt("The depths claim you!"), // RECONSTRUCTED
                        new Flash(0x003366, 0.3, 9),
                        new Prioritize(new StayCloseToSpawn(0.25, 7), new Wander(0.3)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 800),
                        new Shoot(15, projectileIndex: 1, count: 5, shootAngle: 20, predictive: 0.5, coolDown: 1400),
                        new HpLessTransition(0.4, "abyssal")
                    ),
                    new State("abyssal",
                        new Taunt("INTO THE ABYSS!"), // RECONSTRUCTED
                        new Flash(0x001133, 0.2, 14),
                        new Prioritize(new Wander(0.45)),
                        new Shoot(15, count: 16, shootAngle: 22, coolDown: 650),
                        new Shoot(15, projectileIndex: 1, count: 7, shootAngle: 14, predictive: 0.6, coolDown: 1000)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            // THE HEARTBURN — EventBosses/ defeat feed confirmed ("The Heartburn has been defeated! (4/30)").
            // Fire/heat realm event boss. DESIGNED stub 2026-06-24.
            .Init("The Heartburn",
                new State(
                    new State("idle", new PlayerWithinTransition(15, "fight")),
                    new State("fight",
                        new Taunt("Your heart will burn for this!"), // RECONSTRUCTED — name-derived
                        new Flash(0xff3300, 0.3, 10),
                        new Prioritize(new StayCloseToSpawn(0.25, 7), new Wander(0.35)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 800),
                        new Shoot(15, projectileIndex: 1, count: 5, shootAngle: 20, predictive: 0.4, coolDown: 1400),
                        new HpLessTransition(0.35, "inferno")
                    ),
                    new State("inferno",
                        new Taunt("INFERNO! BURN THEM ALL!"), // RECONSTRUCTED
                        new Flash(0xff1100, 0.2, 15),
                        new Prioritize(new Wander(0.5)),
                        new Shoot(15, count: 16, shootAngle: 22, coolDown: 650),
                        new Shoot(15, projectileIndex: 1, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 1000)
                    )
                ),
                new Threshold(0.05,
                    new TierLoot(13, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(6, ItemType.Ring, 0.1),
                    new ItemLoot("Potion of Life", 0.3)
                )
            );
    }
}
