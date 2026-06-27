using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF ICY PEAKS (seasonal ice dungeon; recovered from IcyPeaks_tutorial).
    // Ice Queen is invulnerable behind 4 ice-encased TOWERS; destroy them to thaw her.
    // TOWER-GATE = same family as Ra-Staves (GateUnderworld) / Anubis-Pillars (AnubisLair).
    // No Ice Queen taunt was captured -> ice-themed lines RECONSTRUCTED (clearly marked). Auto-registered.
    // MECH-LOOP 2026-06-16: map DOCUMENTED (compact L-layout: narrow tan/stone entry corridor → large rectangular
    //   light-blue ice-tile boss chamber; outdoor snowy biome with dark pine trees exterior). Attacks: no-footage.
    partial class BehaviorDb
    {
        private _ IcyPeaks = () => Behav()
            // Arena: compact Icy Peaks dungeon — narrow tan/brown stone entry corridor leads to large rectangular
            // boss chamber with light-blue ice tile floor; exterior: outdoor snowy biome (dark pine trees, snow).
            // Ice Queen's Towers are blue ice-cube entities inside boss chamber.
            // DOCUMENTED IcyPeaks f_00130/f_00140 (active Ice Queen fight, light-blue floor) + minimap f_00145.
            .Init("Ice Queen",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "intro")
                    ),
                    // spawn the 4 towers, brief beat so they exist before the invuln check
                    new State("intro",
                        new Taunt("You will freeze in my halls."), // RECONSTRUCTED (no footage taunt)
                        new Spawn("Ice Queen's Tower", maxChildren: 4, initialSpawn: 1.0, coolDown: 999999),
                        new TimedTransition(900, "warded")
                    ),
                    // P1: invulnerable behind the towers — slowing ice volleys
                    new State("warded",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt(0.002, "My towers will not fall!"), // RECONSTRUCTED
                        new Flash(0x66ccff, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.2, 4), new Wander(0.2)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 750),
                        new Shoot(15, projectileIndex: 1, count: 3, shootAngle: 12, predictive: 0.5, coolDown: 1600),
                        new EntityNotExistsTransition("Ice Queen's Tower", 50, "thawed")
                    ),
                    // P2: towers down — thawed, vulnerable, faster
                    new State("thawed",
                        new Taunt("No... my ice melts..."), // RECONSTRUCTED
                        new Flash(0x3399ff, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 12, shootAngle: 30, coolDown: 600),
                        new Shoot(16, projectileIndex: 1, count: 5, shootAngle: 20, coolDown: 1100)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.14),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            .Init("Ice Queen's Tower",
                new State(
                    new Shoot(12, count: 4, shootAngle: 90, coolDown: 1100)
                ),
                new Threshold(0.5,
                    new TierLoot(4, ItemType.Potion, 0.1))
            );
    }
}
