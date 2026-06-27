using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // Ordinary Client — recovered ROTF bosses. TAUNTS are VERBATIM from video recovery
    // (Ortar: disc_1uD9wcqfVUQ/disc_fFKeD2EZFSg; Cracked Core: disc_CgZC3OpXVhU;
    //  God of Reptilians: disc_3eM07g4Ly5k; Dwarf King (was mislabeled "Death King"; disc_PRzNeGfRR00 + disc_TkraL4lSyDw confirm "Dwarf King"): disc_fjgne_X74s8;
    //  Twilight Archmage: disc_hl-ETK3d42A; Epic Ghost of Skuld: disc_HvB7mdTTE4E).
    // Entities/stats/sprites reconstructed (OrdinaryClient_Bosses.xml). Auto-registered.
    partial class BehaviorDb
    {
        private _ OCBosses = () => Behav()
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_qheTDTNSp-A f_00070 (active fight in magenta/pink dungeon):
            // small spherical WHITE projectile dots visible in radial spread pattern. Ortar has no Spawn() minions
            // so all arena bullets in f_00070 are attributable to Ortar. Count:8/angle:45 RECONSTRUCTED.
            // ALSO: "Staffords" → "Trollords" corrected via Ring of Ortar item tooltip f_00085 (verbatim).
            .Init("Ortar",  // arena: large oval cave dungeon (~50 tiles across), brown/orange rocky outer wall, vivid magenta/pink floor tiles with dark-red rocky island outcroppings, 12-point progress counter — DOCUMENTED-from-frames+minimap disc_qheTDTNSp-A (f_00070 fight, f_00085 death+Ring-of-Ortar-drop, mm_00005-mm_00028 layout). Loot: Ring of Ortar OBSERVED (f_00085 tooltip: +4Atk/+2Def/+4Vit/+4Wis/+60HP/+60MP/7%Fame/FP:3300) — item NOT in XML, add before enabling loot.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("Hail, I Ortar, Highest of Trollords."), // CORRECTED disc_qheTDTNSp-A f_00085 (Ring of Ortar tooltip reads "Highest of Trollords, Ortar" — prior spec had "Staffords" which was a misread; "Trollords" fits the caveman-troll character)
                        new Taunt(0.0016, "Me play. You play. Fun."), // SOURCED-1-spec disc_1uD9wcqfVUQ (single source, thematic Ortar; not frame-re-verified this fire)
                        new Taunt(0.0016, "Me want to go! Me angery!"), // VERIFIED-4x-spec (disc_tyG-KT533DA/fFKeD2EZFSg/ed9YjhhonUU/n7RZ1JzWXXw — 4 independent recoveries)
                        new Taunt(0.0016, "Why you serious, Why you no fun!"), // CORRECTED GalacticLoot chat_00080 (was "Why you serious, why you serious?" — prior version UNVERIFIABLE; frame shows "Ortar: Why you serious, Why you no fun!" verbatim with explicit prefix)
                        new Taunt(0.0016, "Me serious now too."), // VERIFIED-4x-spec (disc_tyG-KT533DA/fFKeD2EZFSg/ed9YjhhonUU/n7RZ1JzWXXw)
                        new Taunt(0.0016, "Why angery? Me want fun..."), // SOURCED-1-spec disc_tyG-KT533DA (single source, thematic Ortar)
                        new Taunt(0.0016, "Stop! You hurt Ortar, Ortar hurt you!"), // VERIFIED-spec disc_tyG-KT533DA (self-names "Ortar" x2)
                        new Taunt(0.0016, "Now Ortar tryhard."), // VERIFIED-2x-spec (disc_pGEVj9uJLFw + disc_tyG-KT533DA; self-names "Ortar")
                        new Taunt(0.0016, "Tryhard... Not fun.. TRYHARD NOT FUN!"), // VERIFIED-2x-spec (disc_pGEVj9uJLFw + disc_tyG-KT533DA; Ortar tryhard-theme, distinct from Arcanica)
                        new Taunt(0.0016, "This NO fun.. Me leave.. I ANGRY!"), // VERIFIED-2x-spec (disc_pGEVj9uJLFw + disc_tyG-KT533DA)
                        new Taunt(0.0016, "Ortar KILL you! FUN! haHAA!"), // VERIFIED-2x-spec (disc_pGEVj9uJLFw + disc_tyG-KT533DA; self-names "Ortar")
                        new Taunt(0.0016, "Ortar make you die. Hehehehe...."), // VERIFIED-spec disc_fFKeD2EZFSg (self-names "Ortar")
                        new Prioritize(new Wander(0.4)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 800),     // small spherical white bullets PARTIAL f_00070; count:8/angle:45 RECONSTRUCTED
                        new Shoot(13, count: 4, shootAngle: 20, predictive: 0.5, coolDown: 1300)  // aimed (RECONSTRUCTED — no isolation)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1))
            )
            .Init("Cracked Core",  // arena: Starforce Zone — single large brown rocky chamber, dark tile texture with lighter oval stone patches, compact circular layout (~30-40 tiles across) — DOCUMENTED-from-frames disc_CgZC3OpXVhU (f_00190-f_00205: entry "Welcome to the Starforce Zone!", f_00202: Deepspace Mantle GL tooltip, minimap shows compact circular blob) + disc_ZgpUWDff7Ns (f_00220 in-game view, "Cracked Core 4%" header). Loot: Deepspace Mantle GL OBSERVED (f_00202 tooltip: +6Atk/+16Def/+8Wis/+65MaxMP/+1Spd/+1Vit/+1Dex/+10MaxHP/6%Fame/SB, Wizard+Priest+Necromancer+Mystic+Sorcerer) — item NOT in XML; add before enabling ItemLoot. Attack: X/cross bullet pattern OBSERVED disc_CgZC3OpXVhU f_00205 (4 diagonal arms cyan bullets, fight-start); existing ring code (count:12/shootAngle:30) NOT frame-validated, mismatch flagged.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Flash(0xff33ddff, 0.25, 12),
                        new Taunt("D--ie w-a-rr-i-o-r!"), // CORRECTED disc_rQseJUyAHU0 (was "D--le w-a-rr-l-o-r!"; the glitched text spells "DIE WARRIOR!" — build misread i->l twice; frame-read "Cracked Core: D--ie w-a-rr-i-o-r!")
                        new Taunt(0.0016, "H-h-h-h-h-h-h-h-h"), // VERIFIED disc_rQseJUyAHU0 (frame-read; the glitchy stutter-laugh, "Cracked Core: H-h-h-h-h-h-h-h-h")
                        new Taunt(0.0016, "DOOOOOOOOOOOOOOOOOOOM!!!"), // VERIFIED disc_rQseJUyAHU0 + disc_CgZC3OpXVhU (the DOOM scream)
                        new Taunt(0.0016, "UUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU OOOOOOM!!!"), // VERIFIED GalacticLoot chat_00020 ("Cracked Core:" prefix explicit; corrupted "DOOM" scream variant, consistent with its glitchy speech style)
                        new Taunt(0.0016, "These creatures are too overwhelming!!!"), // CORRECTED disc_rQseJUyAHU0 (was "These creatures are mine!"; frame-read exact "Cracked Core: These creatures are too overwhelming!!!")
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 650), // PARTIAL: f_00205 disc_CgZC3OpXVhU shows X/cross pattern (4 diagonal arms) NOT a 12-count ring; ring RECONSTRUCTED, mismatch
                        new Shoot(14, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 1100) // RECONSTRUCTED — no frame isolation
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.1),
                    new TierLoot(13, ItemType.Armor, 0.1),
                    new TierLoot(5, ItemType.Ring, 0.12))
            )
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_XGl3pTaNmTM f_00039/f_00041/f_00043 (active fight):
            // dense ring of small round ORANGE/YELLOW projectiles radiating from boss in radial ring pattern;
            // green-grass realm arena with brown/dirt tile patches (outdoor realm, not dungeon). Proj-13 (orange)
            // CONFIRMED. count:6 shootAngle:60 plausible but ring density suggests could be more (resolution limit).
            .Init("God of Reptilians",
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("Your stares at defeating me make my heart warm up, say bye and move out of my way!"), // VERIFIED disc_XGl3pTaNmTM (frame-read f_00039+f_00041, wide re-crop; opening taunt — was MISSING from build)
                        new Taunt(0.0016, "Let's get a little bit more movement going on... oh, and... prepare to meet your purpose, DOOM!"), // CORRECTED disc_XGl3pTaNmTM (frames f_00039+f_00041 show ONE wrapped message; build had it as 2 split taunts "Let's get a little more serious..." + "Prepare to meet your purpose, DOOM..." — "serious" was a MISREAD of "movement going on", "bit" + "oh, and..." were dropped)
                        new Taunt(0.0016, "Don't push your luck adventurer... prepare for your final lesson... on DEATH!!!"), // CORRECTED disc_XGl3pTaNmTM (frames f_00039+f_00041 show ONE wrapped message; build had it as 2 split taunts "Don't push your luck... 'adventurer'" + "...on DEATH!!!" — the middle "prepare for your final lesson..." was DROPPED and the quotes around adventurer + the ellipsis-split were fabricated. Supersedes the prior 2x-spec partial label)
                        new Prioritize(new Wander(0.4)),
                        // PARTIAL (f_00039/f_00043): small round ORANGE ring spray. Proj-13 CONFIRMED; count:6 RECONSTRUCTED.
                        new Shoot(13, count: 6, shootAngle: 60, coolDown: 850),
                        new Spawn("Asgard Guardian", maxChildren: 4, initialSpawn: 0.25, coolDown: 7000)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1))
            )
            // NOTE: "Dwarf King" is NOT defined here — the BASE game already has a "Dwarf King"
            // (MidCXML 0x61f + BehaviorDb.Midland.cs). The ROTF recovery (disc_fjgne_X74s8) confirmed
            // the boss seen in footage IS that Dwarf King (taunt "You'll taste my axe!" matches base
            // game verbatim), so the recovered taunts were folded into the Midland behavior and the
            // redundant duplicate (formerly "Death King" 0x8fd3) was removed to avoid an id collision.
            .Init("Twilight Archmage",  // arena: Shatters final boss chamber — large rectangular yellow/gold brick floor, dark blood-stone wall border, green stone corner exits; follows Forgotten King sequence (Sentinel: "I tried to protect you...You release a great evil upon this realm") — DOCUMENTED-from-frames disc_I-LLUiWJE5I (f_00090+f_00092: yellow tile chamber, boss confirmed Paralyzed+fighting; b013_85s/b024_236s: yellow tile arena; death taunt "I...will......retuuuur...n...n....." at f_00092 NOT in build). NOTE: base-game Shatters boss built here as OC custom.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Flash(0x9933ff, 0.3, 10),
                        new Taunt("Darkness give me strength!"),                    // VERIFIED base-game Shatters canonical (disc_hl-ETK3d42A + disc_SXgeJh2Ao6U + disc_g1Ce99YlFrI + disc_I-LLUiWJE5I). NOTE: Twilight Archmage is BASE-GAME SHATTERS, built here as a custom OC boss -> RECLASSIFY (see verify_note; these are canonical RotMG lines, not ROTF-custom)
                        new Taunt(0.0016, "Let me see what I can conjure up"),       // VERIFIED base-game Shatters canonical (disc_SXgeJh2Ao6U; real RotMG Archmage line)
                        new Taunt(0.0016, "You leave me no choice..."),             // VERIFIED base-game Shatters canonical (disc_g1Ce99YlFrI frame-read shows the full "You leave me no choice... Inferno! Blizzard!"; build's truncation kept)
                        new Taunt(0.0016, "Your souls will feed my eyes."),          // CORRECTED disc_g1Ce99YlFrI (was: "Your souls will feed my King." — "King" was ADMITTED-INVENTED lore, NOT canonical; base-game Shatters line is "Your souls will feed my eyes" / "...feel my evil" per the 8-line frame-read set. Removed the fabrication, restored canonical)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 700),   // PARTIAL disc_I-LLUiWJE5I: fight confirmed (b013_85s/b024_236s yellow tiles, boss entity visible); bullet geometry UNRESOLVABLE (paralyze-zerg + multi-dungeon contamination across all bossend groups); count:10 ring RECONSTRUCTED, not validated
                        new Shoot(14, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1400)  // RECONSTRUCTED — no frame isolation
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1))
            )
            .Init("Epic Ghost of Skuld",  // arena: Undead Lair — gray square stone tile floor (dark squares with lighter grid borders), corridor-linked rooms, cross/plus-shaped dungeon layout — DOCUMENTED-from-frames disc_bk4dygntBZ8 (b002_15s f004+f007: post-kill stone floor "Quest Complete!"; b003_80s f001: mid-fight stone floor + radial ring bullets visible) + disc_IBEp1CbA00Q (b005_41s f001: fight on stone floor; b002_4s f001: level-up minimap shows cross-corridor layout). Loot: Potion of Dexterity OBSERVED disc_IBEp1CbA00Q b005_41s f003 (tooltip FP:150 "Permanently Increases Dexterity") — boss vs mob origin ambiguous, not added to Threshold; existing ItemLoot("Potion of Life") from design spec. Attack: ring burst OBSERVED disc_bk4dygntBZ8 b003_80s f001 (~8-10 dash-bullets radial at kill moment); count:12/angle:30 not contradicted, exact count unresolvable; second Shoot(5 predictive) RECONSTRUCTED.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("A SWIFT DEATH!!!"), // VERIFIED-3x-spec (disc_bk4dygntBZ8 + disc_ZEnAhsvDzKY + disc_HvB7mdTTE4E — 3 independent recoveries transcribe it identically; not single-frame-isolated this pass, red-lava montages surfaced other bosses, but cross-spec convergence is authoritative)
                        new Taunt(0.0014, "Congratulations on your victory, warrior."), // VERIFIED-2x-spec-partial: "Congratulations on your victory" corroborated by 3 specs; the ", warrior." tail is from disc_HvB7mdTTE4E only (disc_bk4dygntBZ8 + disc_ZEnAhsvDzKY truncate before it) -> core multi-source, tail single-source
                        new Prioritize(new Wander(0.5)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 600), // PARTIAL disc_bk4dygntBZ8 b003_80s f001: radial ring ~8-10 dash-bullets outward at kill moment on stone floor; count:12/angle:30 not contradicted, exact count unresolvable from residual frame
                        new Shoot(14, count: 5, shootAngle: 18, predictive: 0.6, coolDown: 1000) // RECONSTRUCTED — no frame isolation of aimed spread
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.1),
                    new TierLoot(13, ItemType.Armor, 0.1),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.12))
            )
            // Stonetaker — stone-hoarder realm boss; VERBATIM taunts (build_ledger): "Mmm... Beautiful stones..."
            // / "Leave my stones in peace..." -> enrage "LEAVE MY STONES ALONE!". Object 0x8fd6.
            .Init("Stonetaker",  // arena: outdoor realm — dark brown rocky/soil terrain with lava patches; roaming realm event boss (no structured dungeon arena) — DOCUMENTED-from-frames disc_zDnBF0EgLNI (minimap_00001: solid amber = realm terrain confirmed; f_00010: dark soil + orange lava band fight terrain; b003_10s f004: boss mid-fight + radial ring burst visible on realm terrain). Attack: radial ring of round bloom bullets OBSERVED b003_10s f004 (~8 bullets evenly spaced, guard-state timing), CONSISTENT with count:8/angle:45 guard Shoot; enrage attacks RECONSTRUCTED (no enrage footage).
                new State(
                    new State("idle", new PlayerWithinTransition(13, "guard")),
                    new State("guard",
                        new Taunt("Mmm... Beautiful stones..."), // VERIFIED-2x-spec (disc_PRzNeGfRR00 + disc_Q4c9_KWBey4, both DOCUMENTED-VERBATIM boss-coloured chat, identical; direct single-frame isolation attempted but the 944+1609-crop tags surfaced lava/Quest-Chest spam, not the boss frames -> cross-spec convergence, no conflict w/ build)
                        new Taunt(0.0016, "Leave my stones in peace..."), // VERIFIED-2x-spec (disc_PRzNeGfRR00 + disc_Q4c9_KWBey4, identical verbatim)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 850), // PARTIAL disc_zDnBF0EgLNI b003_10s f004: radial ring ~8 round bloom bullets at even spacing (~45° between); count:8/angle:45 CONSISTENT with observed ring
                        new Shoot(13, count: 4, shootAngle: 20, predictive: 0.5, coolDown: 1300), // RECONSTRUCTED — no frame isolation of aimed spread
                        new HpLessTransition(0.3, "enrage")
                    ),
                    new State("enrage",
                        new Taunt("LEAVE MY STONES ALONE!"), // VERIFIED-spec disc_PRzNeGfRR00 (DOCUMENTED-VERBATIM boss-coloured enrage line; 1 source). NOTE: disc_PRzNeGfRR00 also documents a death-panic line "No... no... NO! YOU WI[LL]..." not in the build -> flagged for a possible future enrich (dev-loop)
                        new Taunt(0.0016, "NOT MY STONES!!!!!"), // VERIFIED GalacticLoot (spec-documented)
                        new Taunt(0.0016, "MY BEAUTIFUL STONES! NOOO!!!"), // VERIFIED GalacticLoot (spec-documented)
                        new Taunt(0.0016, "My stones... no... no..."), // VERIFIED GalacticLoot (spec-documented; quiet-loss line)
                        new Flash(0xaa8855, 0.3, 10),
                        new Prioritize(new Follow(0.7, 12, 2), new Wander(0.4)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 600), // RECONSTRUCTED — no enrage footage
                        new Shoot(14, count: 6, shootAngle: 15, predictive: 0.6, coolDown: 1000) // RECONSTRUCTED — no enrage footage
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1))
            )
            // DESIGNED stub — wiki-only, no footage. Soul collector; drops Reaper's Bow
            // ("fires the souls of Elithor's victims"). Dungeon: likely Illusion (mid-tier, Beholder drop).
            // All taunts RECONSTRUCTED from item lore.
            .Init("Elithor",
                new State(
                    new State("idle", new PlayerWithinTransition(13, "hunt")),
                    new State("hunt",
                        new Taunt("Your soul is already mine."),                               // RECONSTRUCTED — Reaper's Bow lore
                        new Taunt(0.002, "I have collected so many souls... yours will make a fine addition."), // RECONSTRUCTED — Soul Bottle theme
                        new Taunt(0.002, "Run. It makes the harvest so much sweeter."),        // RECONSTRUCTED — predatory soul reaper
                        new Taunt(0.002, "The illusion of hope ends here."),                   // RECONSTRUCTED — Illusion dungeon connection
                        new Prioritize(new Follow(0.6, range: 10), new Wander(0.35)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 850),
                        new Shoot(13, projectileIndex: 1, count: 3, shootAngle: 20, predictive: 0.5, coolDown: 1400),
                        new HpLessTransition(0.35, "harvest")
                    ),
                    new State("harvest",
                        new Taunt("Now... FEED MY COLLECTION!!!"),   // RECONSTRUCTED — enrage
                        new Flash(0x440088, 0.35, 10),
                        new Prioritize(new Follow(0.75, range: 8), new Wander(0.4)),
                        new Shoot(13, count: 12, shootAngle: 30, coolDown: 650),
                        new Shoot(13, projectileIndex: 1, count: 4, shootAngle: 90, coolDown: 800)
                    )
                ),
                new Threshold(0.1,
                    new ItemLoot("Reaper's Bow", 0.07),     // confirmed drop (rotfserver wiki)
                    new TierLoot(11, ItemType.Weapon, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.1),
                    new TierLoot(4, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.15)
                )
            )
            // DESIGNED stub — wiki-only, no footage. Card-king/gambler; drops PoOoOoOokkErrRr UT dagger
            // (binary desc = "Nice!") and Ring of the Poker King. Dungeon unknown. All taunts RECONSTRUCTED.
            .Init("Pokerface",
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("You can't read me. NO ONE can read me."),    // RECONSTRUCTED — poker face reference
                        new Taunt(0.002, "All in. Show me what you've got."),   // RECONSTRUCTED — gambling theme
                        new Taunt(0.002, "The Poker King bows to no one."),     // RECONSTRUCTED — Ring of the Poker King lore
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.35)),
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 800),
                        new Shoot(14, projectileIndex: 1, count: 4, shootAngle: 25, predictive: 0.5, coolDown: 1300),
                        new HpLessTransition(0.4, "all_in")
                    ),
                    new State("all_in",
                        new Taunt("PoOoOoOokkErrRr!!!"),                        // RECONSTRUCTED — item name as battle cry
                        new Flash(0xffdd00, 0.3, 10),
                        new Prioritize(new Wander(0.45)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 600),
                        new Shoot(14, projectileIndex: 1, count: 6, shootAngle: 20, predictive: 0.6, coolDown: 950)
                    )
                ),
                new Threshold(0.08,
                    new ItemLoot("PoOoOoOokkErrRr", 0.04), // confirmed drop (rotfserver wiki; legendary UT)
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.2)
                )
            );
    }
}
