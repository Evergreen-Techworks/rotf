using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF TOMB OF DECAYING DEATH (returning legacy dungeon, graveyard biome). Reconstructed from the
    // aced.gg dev blog (no fight footage exists): the roster CRAWLS / SPRINTS / FLOATS (varied AI), so
    // the Tombstone Carrier summons that roster across decay phases. Taunts here are RECONSTRUCTED
    // flavor (none were recorded) and kept sparse + deadpan. Auto-registered by reflection.
    partial class BehaviorDb
    {
        private _ ToDD = () => Behav()
            .Init("Tombstone Carrier",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "rise")
                    ),
                    // wakes the graveyard — calls up the first crawlers
                    new State("rise",
                        new Taunt("The dead do not rest easy here."), // DESIGNED-no-footage (Tomb of Decaying Death is a PRIORITY no-footage dungeon; .cs header confirms "RECONSTRUCTED flavor, none were recorded". Reconstructed-deadpan from the aced.gg dev-blog roster. Honestly labeled.)
                        new Spawn("Rotten Corpse", maxChildren: 4, initialSpawn: 0.8, coolDown: 6000),
                        new Spawn("Buried Hand", maxChildren: 2, initialSpawn: 0.5, coolDown: 9000),
                        new TimedTransition(1500, "swarm")
                    ),
                    // P1: melee-range swipes (proj 0) while the roster crawls in
                    new State("swarm",
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.3)),
                        new Spawn("Brain Craver", maxChildren: 3, initialSpawn: 0.6, coolDown: 8000),
                        new Shoot(13, count: 6, shootAngle: 60, coolDown: 800),
                        new HpLessTransition(0.5, "decay")
                    ),
                    // P2: rot spreads — Sick gas (proj 1) + heavier flesh sprints in
                    new State("decay",
                        new Taunt("Rot takes everything in time."), // DESIGNED-no-footage (reconstructed decay-phase flavor; no footage for ToDD. Honestly labeled.)
                        new Flash(0x66aa44, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Spawn("Spoilt Flesh", maxChildren: 3, initialSpawn: 0.5, coolDown: 7000),
                        new Shoot(14, count: 8, shootAngle: 45, coolDown: 850),
                        new Shoot(14, projectileIndex: 1, count: 4, shootAngle: 30, coolDown: 1700),
                        new HpLessTransition(0.2, "final")
                    ),
                    // P3: the tomb collapses — everything at once
                    new State("final",
                        new Taunt("Join the others below."), // DESIGNED-no-footage (reconstructed collapse-phase flavor; no footage for ToDD. Honestly labeled.)
                        new Flash(0x448822, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(15, count: 12, shootAngle: 30, coolDown: 600),
                        new Shoot(15, projectileIndex: 1, count: 6, shootAngle: 60, coolDown: 1100)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.14),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            // crawler — slow, shambles toward players, Sick melee
            .Init("Rotten Corpse",
                new State(
                    new Prioritize(new Follow(0.7, range: 9), new Wander(0.4)),
                    new Shoot(8, count: 1, coolDown: 900)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.06))
            )
            // sprinter — fast, erratic, Confused shots
            .Init("Brain Craver",
                new State(
                    new Prioritize(new Follow(1.2, range: 11), new Wander(0.6)),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 700)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.06))
            )
            // buried turret — stays put, Slowed shots, reveals its counterpart
            .Init("Buried Hand",
                new State(
                    new Prioritize(new StayCloseToSpawn(0.4, 2)),
                    new Spawn("Revealed Remains", maxChildren: 2, initialSpawn: 1.0, coolDown: 12000),
                    new Shoot(11, count: 3, shootAngle: 120, coolDown: 1000)
                ),
                new Threshold(0.5, new TierLoot(3, ItemType.Potion, 0.05))
            )
            // floater — drifts, sprays fast shots
            .Init("Revealed Remains",
                new State(
                    new Prioritize(new Wander(0.7)),
                    new Shoot(12, count: 5, shootAngle: 72, coolDown: 950)
                ),
                new Threshold(0.5, new TierLoot(3, ItemType.Potion, 0.05))
            )
            // heavy — tanky shambler, Sick melee burst
            .Init("Spoilt Flesh",
                new State(
                    new Prioritize(new Follow(0.6, range: 8), new Wander(0.3)),
                    new Shoot(9, count: 4, shootAngle: 90, coolDown: 1100)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.08))
            );
    }
}
