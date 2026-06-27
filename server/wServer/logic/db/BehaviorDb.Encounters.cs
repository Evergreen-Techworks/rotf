// MECH-LOOP: Breathing/Smashing/Shadow/Flying Encounter — all four are open REALM events (no dungeon). Spawn anywhere in realm; floor varies by biome (sandy/tan, dark stone, white/snowy confirmed disc_fnAwkbaB_0c f_00325-00345). Realm minimap confirmed (scattered colored dots = realm enemies/portals; minimap_00325/328). All four share the same arena classification. DOCUMENTED-from-frames disc_fnAwkbaB_0c f_00325-00345 + minimap_00325/328.
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF meme "ENCOUNTER" REALM EVENT BOSSES. They taunt the players with condescending one-liners.
    // The taunts on Breathing / Smashing / Shadow Encounter are VERBATIM from recovered footage
    // (docs/video-recovery/disc_aCAA_5Od6WY.json). Flying Encounter's line is RECONSTRUCTED in the same
    // style (none recorded). Auto-registered by reflection.
    partial class BehaviorDb
    {
        private _ Encounters = () => Behav()
            .Init("Breathing Encounter",  // arena: open REALM event — no dungeon; spawns anywhere in realm; floor varies by biome; realm minimap DOCUMENTED disc_fnAwkbaB_0c minimap_00325/328 (scattered colored dots = realm)
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "fight")
                    ),
                    new State("fight",
                        new Taunt("GRAVE MISTAKES HAVE BEEN MADE."), // verbatim (disc_ZEnAhsvDzKY/disc_fnAwkbaB_0c)
                        new Taunt(0.0015, "I've seen leprechauns hit harder than that. TAKE THIS!"), // CORRECTED disc_aCAA_5Od6WY (was: "I've seen leprechauns hit harder!"; frame-read exact: "Breathing Encounter: I've seen leprechauns hit harder than that. TAKE THIS!" — templated family: Shadow="babies", Smashing="mermaids")
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.35)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 600),
                        new Shoot(15, count: 6, shootAngle: 60, predictive: 0.4, coolDown: 1000)
                    )
                ),
                new Threshold(0.08,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            .Init("Smashing Encounter",  // arena: open REALM event — same as Breathing Encounter; no dungeon; floor varies by biome; realm minimap DOCUMENTED disc_fnAwkbaB_0c minimap_00325/328
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "fight")
                    ),
                    new State("fight",
                        new Taunt("GRAVE MISTAKES HAVE BEEN MADE."), // VERIFIED disc_fnAwkbaB_0c (shared Encounter family line, frame-verified; cf. Shadow Encounter). disc_ZEnAhsvDzKY attribution for THIS exact string dropped — in disc_ZEnAhsvDzKY this boss is seen saying the distinct variant below instead.
                        new Taunt(0.0015, "YOU HAVE MADE A GRAVE MISTAKE, WARRIOR!"), // VERIFIED disc_ZEnAhsvDzKY (frame-read f_00334/00336: boss-coloured <Smashing Encounter> + <Breathing Encounter>; a 2nd grave-themed shared Encounter taunt, distinct from the period line above)
                        new Flash(0xcc6622, 0.3, 6),
                        // heavy slow slams that stun
                        new Prioritize(new Follow(0.55, range: 8), new Wander(0.2)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 1100),
                        new Shoot(13, count: 4, shootAngle: 90, coolDown: 1600)
                    )
                ),
                new Threshold(0.08,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            .Init("Shadow Encounter",  // arena: open REALM event — same as Breathing Encounter; no dungeon; floor varies by biome; realm minimap DOCUMENTED disc_fnAwkbaB_0c minimap_00325/328
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "fight")
                    ),
                    new State("fight",
                        new Taunt("GRAVE MISTAKES HAVE BEEN MADE."), // VERIFIED disc_fnAwkbaB_0c (frame-read exact incl. period; shared Encounter line, e.g. "Flying Encounter: GRAVE MISTAKES HAVE BEEN MADE.")
                        new Taunt(0.0015, "I've seen babies hit harder than that. TAKE THIS!"), // CORRECTED disc_aCAA_5Od6WY (was: "I've seen babies hit harder than that"; frame-read exact: "Shadow Encounter: I've seen babies hit harder than that. TAKE THIS!" — templated family: Breathing="leprechauns", Smashing="mermaids")
                        new Flash(0x6622aa, 0.3, 6),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.35)),
                        new Shoot(15, count: 12, shootAngle: 30, coolDown: 650)
                    )
                ),
                new Threshold(0.08,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            .Init("Flying Encounter",  // arena: open REALM event — same as Breathing Encounter; no dungeon; floor varies by biome; realm minimap DOCUMENTED disc_fnAwkbaB_0c minimap_00325/328
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "fight")
                    ),
                    new State("fight",
                        new Taunt("GRAVE MISTAKES HAVE BEEN MADE."), // VERIFIED disc_fnAwkbaB_0c (shared Encounter family line; prior pass frame-cited this exact line for Flying Encounter, e.g. "Flying Encounter: GRAVE MISTAKES HAVE BEEN MADE.")
                        new Taunt(0.0015, "Your attempt at taking me down is..."), // UNVERIFIABLE-illegible disc_fnAwkbaB_0c (recovered but TRUNCATED at chat edge; could NOT re-isolate in the 2084-frame source this verify pass. Kept as recorded with honest "..."; the recovery's bracketed "cute/comical" guesses deliberately NOT baked in.)
                        new Prioritize(new Wander(0.6)),
                        new Shoot(16, count: 8, shootAngle: 45, coolDown: 550),
                        new Shoot(16, count: 5, shootAngle: 72, predictive: 0.6, coolDown: 900)
                    )
                ),
                new Threshold(0.08,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.2)
                )
            );
    }
}
