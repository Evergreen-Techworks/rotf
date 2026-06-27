using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF ASGARD dungeon — the Norse pantheon. TAUNTS are VERBATIM, recovered from video
    // (disc_9vQughNZIX0, disc_cu6l3YQR608, disc_fFKeD2EZFSg). Entities/stats/sprites are
    // reconstructed (OrdinaryClient_Asgard.xml — placeholder sprites). Auto-registered via reflection.
    // MECH-LOOP 2026-06-16: Heimdall attacks PARTIAL — large blue lance bullets (f_00115), Slow debuff (f_00108).
    // MECH-LOOP 2026-06-24: Odin attacks PARTIAL — small rounded/oval ORANGE projectiles (f_00145/f_00148/f_00152).
    // MECH-LOOP 2026-06-24: Loki attacks PARTIAL — disc_9vQughNZIX0 f_00220: TRIDENT (proj-0, teal/aqua) CONFIRMED.
    //   Loki XML has only proj-0 (Trident); both Shoot() calls fire Trident. count+angles RECONSTRUCTED.
    //   Taunt correction: "Now you see me! Now you die!" (was "Now you don't!"). New taunt: "How did you manage to see me?"
    // MECH-LOOP (Heimdall + Loki): Arena DOCUMENTED from disc__1He_TVKDuw minimap_00002 + f_00001/f_00005.
    // Asgard boss chamber: large rectangular+arched room with fine small-square grid floor tiles (white/light
    // gray), two tall dark-stone PURPLE PILLAR structures flanking the top entry corridor. Dungeon palette
    // olive-green/tan on minimap (= dungeon confirmed). Entry: narrow vertical corridor at top center.
    // Multiple Asgard Guardian enemies visible in fight frames. Arena is shared by Heimdall and Loki
    // (same room/same minimap source); Hela, Odin, Thor may have separate chambers (Hela confirmed separately).
    // MECH-LOOP (Odin): Throne Room DOCUMENTED from disc_cu6l3YQR608 f_00150/f_00160/f_00170/f_00180 + minimap_00150.
    // "Throne Room" name is verbatim from Odin's own taunt ("intruders in the Throne Room"). Floor: large
    // alternating gray stone and orange/amber diamond-shaped tiles (diagonal checkerboard, large tile scale).
    // Room shape: large D-shaped/semicircular chamber — minimap_00150 shows distinctive concentric rectangular
    // terrace structure (stone coliseum-like). Chamfered corners visible in fight frames (f_00170). Many Asgard
    // Guardians simultaneously active. Two new taunts frame-verified this pass and added below.
    // MECH-LOOP (Thor): Arena DOCUMENTED from disc_-N3MH_woifQ minimap_00080 + f_00500/f_00550/f_00600.
    // Thor's boss chamber: large CIRCULAR golden room — bright yellow/gold stone ring wall forming circular
    // perimeter; white/cream tile floor with scattered small gold/amber diamond accent tiles; narrow golden
    // entry corridor from below. Asgard dungeon exterior has snowy outdoor aesthetic (pine trees, snowy ground)
    // in corridors, distinct from Thor's golden circular chamber. Radial white lightning-bolt burst visible in
    // f_00500 (bossends absent; not used for formal attack recovery). Map confirmed dungeon (not realm).
    partial class BehaviorDb
    {
        private _ Asgard = () => Behav()
            .Init("Heimdall",  // arena: shared Asgard castle boss chamber — large arch-floor room, fine square-grid tiles, two purple pillar flanks. DOCUMENTED disc__1He_TVKDuw minimap_00002 + f_00001/f_00005.
                new State(
                    new State("idle", new PlayerWithinTransition(12, "fight")),
                    new State("fight",
                        new Taunt("I will destroy the Bifrost!"), // VERIFIED disc_9vQughNZIX0 (frame-read) + disc_Yx0kTFEixbA
                        new Taunt(0.0016, "The sound will penetrate your mind!"), // recovered disc_Yx0kTFEixbA (Gjallarhorn) + full verbatim disc__1He_TVKDuw
                        new Taunt(0.0016, "Attack them, guardians!"), // VERIFIED disc_9vQughNZIX0 (frame-read)
                        new Taunt(0.0016, "Guardians, attack!!"), // VERIFIED Asgard frames chat_00045 ("Heimdallr:" prefix — Norse spelling; entity name kept "Heimdall")
                        new Taunt(0.0016, "My blade will eventually find you!"), // VERIFIED disc_9vQughNZIX0 (frame-read; on-screen though absent from specs)
                        new Taunt(0.0016, "You shall not pass!"), // VERIFIED disc_9vQughNZIX0 (frame-read; was missing from build)
                        new Taunt(0.0016, "Your attacks does not hurt me!"), // DESIGNED-no-footage (not in any spec, not seen in frames; grammar suggests invented)
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        // PARTIAL (f_00115 disc_9vQughNZIX0): bullet shape = large blue elongated lance/diamond;
                        // "Slowed!" debuff on player f_00108 (attribution uncertain — Asgard Guardians also shoot).
                        // count:5 shootAngle:18 RECONSTRUCTED — ~3-4 lances visible in 90° arc, exact count unresolvable.
                        new Shoot(13, count: 5, shootAngle: 18, coolDown: 800),
                        new Spawn("Asgard Guardian", maxChildren: 4, initialSpawn: 0.25, coolDown: 7000)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1)
                )
            )
            .Init("Hela",  // arena: small dark-purple stone room in branching Asgard castle; Hela at top of cross-corridor structure, purple entry gateway at bottom — DOCUMENTED-from-minimap disc_HzO8vU8xmgo:00002. Loot: Hela's Power OBSERVED-from-chat disc_HzO8vU8xmgo:b001_2s_005
                new State(
                    new State("idle", new PlayerWithinTransition(12, "fight")),
                    new State("fight",
                        new Taunt("Deal with the vermins."), // VERIFIED disc_cu6l3YQR608 (frame-read)
                        new Taunt(0.0016, "Vermins, destroy them!"), // VERIFIED disc_cu6l3YQR608 (frame-read)
                        new Taunt(0.0016, "Powered by thousands of souls, instant death."), // VERIFIED disc_cu6l3YQR608 (frame-read)
                        new Taunt(0.0016, "You have come a long way, just to die!"), // VERIFIED disc_cu6l3YQR608 (frame-read; Hela's own line, not the Odin one)
                        // ADDED — frame-verified Hela lines that were missing from the build (disc_cu6l3YQR608):
                        new Taunt(0.0016, "Whatever, it just means that I need to try a little harder."), // VERIFIED disc_cu6l3YQR608
                        new Taunt(0.0016, "Getting hit by my magic will sting you for years... or forever."), // VERIFIED disc_cu6l3YQR608
                        new Taunt(0.0016, "This attack will make you disappear!"), // VERIFIED disc_cu6l3YQR608
                        new Taunt(0.0016, "You start to annoy me! I will now strike harder!"), // VERIFIED disc_cu6l3YQR608
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(13, count: 6, shootAngle: 60, coolDown: 900),
                        new Spawn("Asgard Guardian", maxChildren: 6, initialSpawn: 0.3, coolDown: 6000)
                    )
                ),
                new Threshold(0.1,
                    new ItemLoot("Hela's Power", 0.05), // her signature UT staff (recovered: 5 shots 100-360, multi-hit+pierce, +80HP/+12Wis/7%fame, Hexed-on-hit)
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1)
                )
            )
            .Init("Odin",  // arena: large D-shaped/semicircular Throne Room — diagonal checkerboard floor (gray stone + orange/amber diamond tiles, large scale), chamfered corners. DOCUMENTED disc_cu6l3YQR608 f_00150/f_00160/f_00170/f_00180 + minimap_00150.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "throne")),
                    new State("throne",
                        new Flash(0xffffdd33, 0.3, 10),
                        new Taunt("Attention to all guards, intruders in the Throne Room."), // VERIFIED disc_cu6l3YQR608 chat_00140 (boss-colored prefix on-screen; frame-read this pass)
                        new Taunt(0.0016, "You cannot win against my gods!"), // VERIFIED disc_cu6l3YQR608 (frame-read)
                        new Taunt(0.0016, "You have no chance in this fight!"), // VERIFIED disc_cu6l3YQR608 (frame-read)
                        new Taunt(0.0016, "Help me, guards. Kill the mortals!"), // VERIFIED disc_cu6l3YQR608 (frame-read)
                        new Taunt(0.0016, "You have come a long way. It will end soon!"), // VERIFIED disc_cu6l3YQR608 (frame-read; on-screen comma vs period kept as-is)
                        new Taunt(0.0016, "I would normally welcome my guests, but you already have shown you are not here for pleasure."), // VERIFIED disc_cu6l3YQR608 (frame-read; was missing from build)
                        new Taunt(0.0016, "Only a matter of time, before you fall."), // recovered disc_HhwYZGR0M3s
                        new Taunt(0.0016, "A guard composed of gods. How can you win against us?"), // CORRECTED Asgard frames chat_00070/100/101 (prior: "! How can you ever win now?" — wrong punct+ending)
                        new Taunt(0.0016, "Defeat the mortals!"), // recovered GeneralStream
                        new Taunt(0.0016, "I have awaited you. Prepare to die!"), // VERIFIED disc_cu6l3YQR608 chat_00140 + chat_00150 (double-confirmed, boss-colored prefix)
                        new Taunt(0.0016, "You should be long gone, but you are still here!"), // VERIFIED disc_cu6l3YQR608 chat_00150 (boss-colored prefix)
                        new Prioritize(new StayCloseToSpawn(0.25, 7), new Wander(0.3)),
                        // PARTIAL (disc_HhwYZGR0M3s f_00145/f_00148/f_00152): bullet shape = small rounded/oval ORANGE
                        // projectiles DOCUMENTED; bullets distributed across wide arena = consistent with radial ring.
                        // count:10 shootAngle:36 (full ring) + count:6 predictive burst — both RECONSTRUCTED counts.
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 700),
                        new Shoot(14, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 1100),
                        new Spawn("Asgard Guardian", maxChildren: 8, initialSpawn: 0.3, coolDown: 5000)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.1),
                    new TierLoot(13, ItemType.Armor, 0.1),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.15)
                )
            )
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_9vQughNZIX0 f_00295 (Thor at 2% HP): yellow/gold
            // THUNDER SWIRL (proj-0) entities scattered around boss in fight area CONFIRMED. Count/angle RECONSTRUCTED.
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_9vQughNZIX0 f_00220: teal/aqua TRIDENT (proj-0)
            // bullet clearly visible in Loki fight. Loki XML has ONLY proj-0 (Trident, Speed:90 Dmg:90).
            // Both Shoot() calls fire Trident. count:8/4 + angles RECONSTRUCTED from fight frame density.
            // NEW TAUNTS from chat_00056-00058: "Now you see me! Now you die!" CORRECTS prior "Now you don't!"
            // "How did you manage to see me?" VERIFIED-partial (1 char truncated, completion certain).
            // "For my next trick, I will make..." VERIFIED-STEM but completion off-screen — NOT INJECTED.
            .Init("Loki",  // attacks PARTIAL disc_9vQughNZIX0 f_00220: Trident (proj-0, teal/aqua) CONFIRMED; both Shoot() fire Trident (only proj-0 in XML).
                new State(
                    new State("idle", new PlayerWithinTransition(12, "fight")),
                    new State("fight",
                        new Taunt("Don't fall for my tricks!"), // VERIFIED disc_9vQughNZIX0 (frame-read)
                        new Taunt(0.0018, "Why are you still alive? Die!"), // VERIFIED disc_9vQughNZIX0 (wording frame-confirmed; final ?/! ambiguous at resolution, kept as-is)
                        new Taunt(0.0016, "I will disappear now! Or will I! Hahahahahaha!"), // VERIFIED disc_9vQughNZIX0 (frame-read) + disc__1He_TVKDuw + Asgard frames chat_00180/204/210 (6×ha confirmed)
                        new Taunt(0.0016, "Now you see me! Now you die!"), // CORRECTED disc_9vQughNZIX0 chat_00056 (was "Now you don't!" — frame clearly shows "die")
                        new Taunt(0.0016, "I was invisible! Are you cheating!"), // SPEC-DOCUMENTED disc__1He_TVKDuw (frame not located among 2754; spec-grounded)
                        new Taunt(0.0016, "trick, trick, trick."), // VERIFIED disc_9vQughNZIX0 (frame-read)
                        new Taunt(0.0016, "You seem to be still alive."), // VERIFIED disc_9vQughNZIX0 (frame-read)
                        new Taunt(0.0016, "How did you manage to see me? Unfair!"), // CORRECTED Asgard frames chat_00200/210/300 (prior build missing "Unfair!" suffix; Loki: prefix explicit)
                        new Taunt(0.0016, "You have no chance in this fight!"), // VERIFIED Asgard frames chat_00101 (Loki: prefix explicit)
                        new Prioritize(new Wander(0.5)),
                        new Shoot(13, count: 8, shootAngle: 12, coolDown: 700),  // PARTIAL f_00220: Trident (proj-0, teal/aqua) CONFIRMED; count+angle RECONSTRUCTED
                        new Shoot(13, count: 4, shootAngle: 30, predictive: 0.6, coolDown: 1200), // aimed variant — Trident (proj-0, XML only has 1 proj); RECONSTRUCTED
                        // trickster: briefly vanish, then reappear (matches the "I was invisible!" taunt)
                        new State("vanish",
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new Prioritize(new Follow(0.5, acquireRange: 10, range: 1), new Wander(0.4)),
                            new TimedTransition(2500, "reveal")
                        ),
                        new State("reveal",
                            new TimedTransition(4000, "vanish")
                        )
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1)
                )
            )
            // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_9vQughNZIX0 f_00295 (Thor at 2% HP): yellow/gold
            // THUNDER SWIRL (proj-0) entities scattered around boss in fight area CONFIRMED. Count/angle RECONSTRUCTED.
            .Init("Thor",  // attacks PARTIAL disc_9vQughNZIX0 f_00295: Thunder Swirl (proj-0, yellow/gold) scatter CONFIRMED; count:12 shootAngle:30 RECONSTRUCTED.
                new State(
                    new State("idle", new PlayerWithinTransition(13, "fight")),
                    new State("fight",
                        new Taunt("Beware my thunder!!"), // VERIFIED disc_9vQughNZIX0 chat_00074 (boss-coloured "<Thor>" prefix, exact)
                        new Taunt(0.0018, "You fight good, mortal, but I am better!"), // CORRECTED disc_9vQughNZIX0 f_00295 (was: "But I am stronger!" — unconfirmed guess; full line now visible: "You fight good, mortal, but I am better!")
                        new Taunt(0.0016, "Power of the thunder!"), // VERIFIED RunesNodes (spec-documented) + disc_9vQughNZIX0 (frame-read)
                        new Taunt(0.0016, "Watch out for the thunder!"), // VERIFIED RunesNodes (spec-documented)
                        new Taunt(0.0016, "noooo :C"), // VERIFIED disc_-N3MH_woifQ (spec-documented; meme death line)
                        new Prioritize(new Wander(0.4)),
                        new Shoot(14, count: 12, shootAngle: 30, coolDown: 650),                  // PARTIAL f_00295: Thunder Swirl scatter CONFIRMED; count+angle RECONSTRUCTED
                        new Shoot(14, count: 3, shootAngle: 10, predictive: 0.7, coolDown: 1000) // aimed burst RECONSTRUCTED
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08),
                    new TierLoot(5, ItemType.Ring, 0.1)
                )
            )
            .Init("Asgard Guardian",
                new State(
                    new Prioritize(
                        new Wander(0.4),
                        new StayCloseToSpawn(0.5, 10)
                    ),
                    new Shoot(10, coolDown: 1000)
                )
            );
    }
}
