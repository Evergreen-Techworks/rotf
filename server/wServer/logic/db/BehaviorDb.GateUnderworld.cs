using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF GATE TO THE UNDERWORLD (Hard). Ra fight reconstructed from the aced.gg dev blog:
    // destroy Ra's STAVES first (Ra is invulnerable until they fall) -> sun-channel (Sun Disk)
    // -> calls upon the sky (Sun Lasers). Then Sia & Hu beyond the Sun Docks. Auto-registered.
    partial class BehaviorDb
    {
        private _ GateUnderworld = () => Behav()
            .Init("Ra the Sun God",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "intro")
                    ),
                    // spawn the 4 staves, brief beat so they exist before the invuln check
                    new State("intro",
                        new Spawn("Ra's Staff", maxChildren: 4, initialSpawn: 1.0, coolDown: 999999),
                        new TimedTransition(900, "staves")
                    ),
                    // P1: invulnerable manic bullet-hell until all staves are destroyed
                    new State("staves",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xffdd33, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.2, 4), new Wander(0.2)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 700),
                        new EntityNotExistsTransition("Ra's Staff", 50, "sunChannel")
                    ),
                    // P2: vulnerable, channels the sun — slow Sun Disk (proj 1) + radial shots
                    new State("sunChannel",
                        new Flash(0xffaa00, 0.3, 6),
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.3)),
                        new Shoot(15, count: 8, shootAngle: 45, coolDown: 900),
                        new Shoot(15, projectileIndex: 1, count: 5, shootAngle: 30, coolDown: 1800),
                        new HpLessTransition(0.4, "sky")
                    ),
                    // P3: calls upon the sky — fast Sun Lasers (proj 0) everywhere
                    new State("sky",
                        new Flash(0xff5500, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 14, shootAngle: 25, coolDown: 550),
                        new Shoot(16, count: 6, shootAngle: 12, predictive: 0.6, coolDown: 900)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.14),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            .Init("Ra's Staff",
                new State(
                    new Shoot(12, count: 4, shootAngle: 90, coolDown: 1100)
                ),
                new Threshold(0.5,
                    new TierLoot(4, ItemType.Potion, 0.1))
            )
            .Init("Sia",
                new State(
                    new State("idle", new PlayerWithinTransition(12, "fight")),
                    new State("fight",
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 800)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08))
            )
            .Init("Hu",
                new State(
                    new State("idle", new PlayerWithinTransition(12, "fight")),
                    new State("fight",
                        new Prioritize(new Wander(0.4)),
                        new Shoot(13, count: 10, shootAngle: 36, coolDown: 750)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(12, ItemType.Armor, 0.08))
            );
    }
}
