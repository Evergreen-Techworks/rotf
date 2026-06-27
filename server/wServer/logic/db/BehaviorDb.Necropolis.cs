// MECH-LOOP: Necropolis dungeon — Gravedigger boss chamber: square/octagonal room (chamfered corners), dark gray/black stone tile floor, tombstone/grave decoration entities. DOCUMENTED-from-frames disc_2nyUJDNu9Cs f_00050 (Gravedigger name label, floor, taunts in chat) + minimap_00020 (square/octagonal chamber with red boss dot) + minimap_00015 (players clustered during fight).
// MECH-LOOP 2026-06-24: attacks PARTIAL — disc_2nyUJDNu9Cs f_00050 (grayscale frame): CRESCENT/SWOOSH shaped white projectiles confirmed in radial spread pattern around Gravedigger. Both "open" and "summon" state taunts visible simultaneously → fight at or below 60% HP (summon state). Bullet shape = crescent/swoosh; count/angle RECONSTRUCTED. Color unverifiable (grayscale image). attacks_status upgraded no-footage → partial.
//
// DISCOVERY 2026-06-24 — TWILIGHT NECROPOLIS BOSS SEQUENCE confirmed from docs/video-recovery/frames/TwilightNecropolis/ footage.
// STATUS: NORGHUS (0x8f77) BUILT 2026-06-24, THUSALA (0x8f78) BUILT 2026-06-24.
// DISCOVERY 2026-06-24 — TWILIGHT NECROPOLIS BOSS SEQUENCE:
//   Boss 1: NORGHUS   — fight ~f_00415-430. Kills players ("AlanWalker has been crushed by Norghus!" chat_00415).
//                        Visual: large dark entity, dense red round bullets, dark purple hex dungeon floor.
//                        Mechanic note: "turrets in the corners" (player advice chat_00385 — TN boss room has turret entities).
//                        Confirmed taunts (all "Norghus:" prefix in orange boss-color chat frames):
//                          "Now, hand over your flesh!" (chat_00415)
//                          "I think its time we had some FUN!" (chat_00420)
//                          "Go my children, FEAST ON THEIR FLESH!" (chat_00420)
//                          "Oh Mary Ann, I think its time to let you out of your cage!" (chat_00430)
//                          "The ghouls mean nothing to me, I shall defend the necropolis!" (chat_00425 truncated; full line inferred)
//   Boss 2: THUSALA   — fight ~f_00370 (open) + f_00440-495 (main). TN room: dark purple hexagonal floor tiles.
//                        Visual: large dark humanoid with red circular aura; ring attack of red/orange round balls (f_00457).
//                        Confirmed taunts ("Thusala:" prefix explicitly visible in full frames):
//                          "I have waited a thousand years for this day to come." (f_00372)
//                          "VENGEANCE IS MINE FOR THE TAKING..." (f_00372 follow-up line)
//                          "With every passing second, I GROW EVEN STRONGER." (f_00453, f_00485 — explicit prefix both)
//   Boss 3: DIAGON    — fight ~f_00540-580. Confirmed by "DIAGON 60%!" player announcement (f_00580).
//                        Already built below. Existing "Enough of this. BEHOLD. THE PERFECTION OF DIAGON THE ETERNAL!" confirmed.
//   Also noted: TN has TN Barrier3 + TN Statue3 entities (obstacle mechanic); Norghus taunts persist in chat log into Diagon fight.
//   NORGHUS and THUSALA are UNBUILT — add .Init() stubs once XML type IDs are allocated (check all OrdinaryClient_*.xml first).
//   BEL: item "Bel's Decapitator" says "guarded the Necropolis for thousands of years" — possible 4th TN boss or Asgard boss;
//        NOT found in TwilightNecropolis footage captured so far. Location TBD.
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF TWILIGHT NECROPOLIS. HIGHEST-FIDELITY recovery — all taunts below are VERBATIM from gameplay
    // footage (docs/video-recovery/TwilightNecropolis*.json). Gravedigger = the summoner main boss;
    // the Grave Caretaker speaks the paired lich dialogue ("All things shall be revealed" / "Revealed
    // things shall be hidden" / "The body dies."). Diagon and the Necro Doggo are room mini-bosses with
    // their footage-confirmed drops (Wand of Bone / Claw of the Beast). Auto-registered by reflection.
    partial class BehaviorDb
    {
        private _ Necropolis = () => Behav()
            // TN BOSS 1 — fight ~f_00415-430. Flesh/ghoul summoner; 3 phases.
            // All taunts VERBATIM from docs/video-recovery/frames/TwilightNecropolis/ chat frames.
            .Init("Norghus",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "open")
                    ),
                    new State("open",
                        new Taunt("Now, hand over your flesh!"),              // VERBATIM chat_00415
                        new Taunt(0.002, "I think its time we had some FUN!"), // VERBATIM chat_00420
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.3)),
                        new Shoot(15, count: 8, shootAngle: 45, coolDown: 800), // dense red round bullets — count/angle RECONSTRUCTED
                        new HpLessTransition(0.6, "summon")
                    ),
                    new State("summon",
                        new Taunt("Go my children, FEAST ON THEIR FLESH!"),                            // VERBATIM chat_00420
                        new Taunt(0.002, "Oh Mary Ann, I think its time to let you out of your cage!"), // VERBATIM chat_00430
                        new Spawn("Norghus Ghoul", maxChildren: 5, initialSpawn: 0.8, coolDown: 6000),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 700),
                        new HpLessTransition(0.3, "last_stand")
                    ),
                    new State("last_stand",
                        new Taunt("The ghouls mean nothing to me, I shall defend the necropolis!"), // VERBATIM chat_00425 (truncated in source; full line inferred)
                        new Prioritize(new Wander(0.4)),
                        new Shoot(15, count: 12, shootAngle: 30, coolDown: 550)
                    )
                ),
                new Threshold(0.08,
                    new ItemLoot("Flesh Trap", 0.07),        // confirmed ("flesh from Norghus' collection of corpses")
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            // TN BOSS 2 — fight ~f_00370 (open) + f_00440-495 (main). Dark humanoid, red aura, ring attacks.
            // All taunts VERBATIM from full frame reads (f_00372, f_00453, f_00485). Drops Thusala's Slasher.
            .Init("Thusala",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "open")
                    ),
                    new State("open",
                        new Taunt("I have waited a thousand years for this day to come."), // VERBATIM f_00372
                        new Taunt(0.001, "VENGEANCE IS MINE FOR THE TAKING..."),           // VERBATIM f_00372 follow-up
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.3)),
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 900),  // ring of red/orange balls DOCUMENTED f_00457 — count:8/angle:45 RECONSTRUCTED
                        new HpLessTransition(0.5, "empowered")
                    ),
                    new State("empowered",
                        new Taunt("With every passing second, I GROW EVEN STRONGER."), // VERBATIM f_00453 + f_00485 (repeats in this phase)
                        new Flash(0xff2222, 0.4, 8),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.4)),
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 600),
                        new Shoot(14, count: 4, shootAngle: 90, coolDown: 400)
                    )
                ),
                new Threshold(0.08,
                    new ItemLoot("Thusala's Slasher", 0.08),  // confirmed drop (rotfserver.fandom.com wiki)
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            .Init("Norghus Ghoul",
                new State(
                    new Prioritize(new Follow(1.0, range: 8), new Wander(0.4)),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 900)
                ),
                new Threshold(0.6, new TierLoot(3, ItemType.Potion, 0.04))
            )
            .Init("Gravedigger",  // arena: Necropolis dungeon — large square/octagonal boss chamber (chamfered corners), dark gray/black stone tile floor, tombstone/grave decoration entities. DOCUMENTED-from-frames disc_2nyUJDNu9Cs f_00050 + minimap_00020
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "open")
                    ),
                    // P1
                    new State("open",
                        new Taunt("Let's see how you handle this..."), // VERIFIED disc_2nyUJDNu9Cs (frame-read exact: "Gravedigger: Let's see how you handle this...")
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.3)),
                        new Shoot(15, count: 8, shootAngle: 45, coolDown: 750),  // crescent/swoosh bullet shape PARTIAL f_00050; count:8/angle:45 RECONSTRUCTED
                        new HpLessTransition(0.6, "summon")
                    ),
                    // P2 — calls his minions
                    new State("summon",
                        new Taunt("MINIONS. SAVE ME!!!"), // VERIFIED disc_2nyUJDNu9Cs (frame-read exact: "Gravedigger: MINIONS. SAVE ME!!!")
                        new Spawn("Gravedigger Minion", maxChildren: 6, initialSpawn: 0.8, coolDown: 5000),
                        new Flash(0x88ff88, 0.3, 6),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(15, count: 6, shootAngle: 60, coolDown: 800),
                        new Shoot(15, projectileIndex: 1, count: 4, shootAngle: 30, coolDown: 1600),
                        new HpLessTransition(0.3, "finish")
                    ),
                    // P3
                    new State("finish",
                        new Taunt("Death is your reward! Say no more and FINISH...",
                                  "...your courage softens me. I could grant you a..."),
                        new Taunt(0.0016, "It seems like you have survived... I offer you my congratulations, but you..."), // VERIFIED-partial disc_ZEnAhsvDzKY ("survived" = spec's bracketed read; line trails off — false-congratulations menace)
                        new Flash(0x44cc44, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 12, shootAngle: 30, coolDown: 600),
                        new Shoot(16, projectileIndex: 1, count: 6, shootAngle: 60, coolDown: 1200)
                    )
                ),
                new Threshold(0.08,
                    new ItemLoot("Wand of Retribution", 0.06),
                    new ItemLoot("Gravedigger's Shovel", 0.06),
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            // room mini-boss — drops the footage-confirmed Wand of Bone
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_MszcvkbWED8 f_00155/f_00160/f_00162 (active Necropolis fight):
            // Diagon entity = large RED SKULL figure with glowing red eyes (confirmed via "BEHOLD... DIAGON" taunt).
            // Small round RED/CRIMSON projectiles visible throughout dark dungeon arena — but the Necropolis room has
            // multiple enemies (Gravedigger, Grave Caretaker, etc.) so dense red coverage is multi-entity.
            // Proj-14 → red sprite CONFIRMED from Diagon. Shoot(14, count:6, shootAngle:60) CONSISTENT with ring of 6.
            // Arena: dark charcoal/black stone floor dungeon CONFIRMED (f_00155/f_00162 both show dark stone tiles).
            .Init("Diagon",
                new State(
                    new State("idle", new PlayerWithinTransition(12, "fight")),
                    new State("fight",
                        new Taunt("Enough of this. BEHOLD. THE PERFECTION OF DIAGON THE ETERNAL!"), // CORRECTED disc_MszcvkbWED8 (was: "...THE PERFECTION OF DIAGON!"; full line frame-read exact: "Diagon: Enough of this. BEHOLD. THE PERFECTION OF DIAGON THE ETERNAL!")
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(14, count: 6, shootAngle: 60, coolDown: 700)
                    )
                ),
                new Threshold(0.15,
                    new ItemLoot("Wand of Bone", 0.08),
                    new TierLoot(11, ItemType.Weapon, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.1))
            )
            // werewolf room mini-boss — fast, bleeds you; drops Claw of the Beast
            .Init("Necro Doggo",
                new State(
                    new State("idle", new PlayerWithinTransition(12, "hunt")),
                    new State("hunt",
                        new Prioritize(new Follow(1.1, range: 7), new Wander(0.4)),
                        new Shoot(12, count: 4, shootAngle: 20, coolDown: 600)
                    )
                ),
                new Threshold(0.15,
                    new ItemLoot("Claw of the Beast", 0.08),
                    new TierLoot(11, ItemType.Weapon, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.1))
            )
            // white-robed lich — speaks the paired dialogue while it fights
            .Init("Grave Caretaker",
                new State(
                    new State("idle", new PlayerWithinTransition(12, "fight")),
                    new State("fight",
                        new Taunt(0.0025, "All things shall be revealed",
                                          "Revealed things shall be hidden",
                                          "The body dies."),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 800)
                    )
                ),
                new Threshold(0.15,
                    new ItemLoot("Undead's Gross Bow", 0.06),
                    new TierLoot(12, ItemType.Weapon, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.1),
                    new TierLoot(4, ItemType.Ring, 0.12))
            )
            .Init("Troll Matriarch",  // arena: Skuld Challenge gauntlet — open rectangular green grass arena (~20-30 tiles wide), brown/dirt patches, dark tree-lined forest border; boss spawns after "Ghost of Skuld: Look sharp! A mighty foe is approaching!" — DOCUMENTED-from-frames disc_sTTSEekkX1o:f_00310-f_00324
                new State(
                    new Taunt(0.0016, "This forest will be your tomb!"), // VERIFIED disc_sTTSEekkX1o (verbatim-full, f_00320 chat)
                    new Taunt(0.0016, "I call upon the aid of warriors past! Smite these trespassers!"), // CORRECTED disc_sTTSEekkX1o f_00310 (was: tail-cut "...past!"; full line now readable: "...Smite these trespassers!")
                    new Taunt(0.0016, "I feel invincible with my minions here!"), // VERIFIED disc_sTTSEekkX1o (chat_00080/00084)
                    new Prioritize(new Follow(0.7, range: 9), new Wander(0.3)),
                    new Shoot(11, count: 3, shootAngle: 30, coolDown: 900) // PARTIAL disc_sTTSEekkX1o: aimed pink/red spread confirmed; exact count:3/angle:30 unverifiable at 2fps multi-player
                ),
                new Threshold(0.4,
                    new ItemLoot("Undead's Gross Bow", 0.03),
                    new TierLoot(5, ItemType.Potion, 0.07))
            )
            .Init("Ghost Bride",
                new State(
                    new Prioritize(new Wander(0.6)),
                    new Shoot(12, count: 5, shootAngle: 40, coolDown: 850)
                ),
                new Threshold(0.4,
                    new ItemLoot("Undead's Gross Bow", 0.03),
                    new TierLoot(4, ItemType.Potion, 0.06))
            )
            .Init("Gravedigger Minion",
                new State(
                    new Prioritize(new Follow(0.9, range: 8), new Wander(0.4)),
                    new Shoot(9, count: 2, shootAngle: 20, coolDown: 800)
                ),
                new Threshold(0.6, new TierLoot(3, ItemType.Potion, 0.03))
            );
    }
}
