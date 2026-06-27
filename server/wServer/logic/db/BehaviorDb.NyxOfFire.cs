using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF NYX OF FIRE — fire/demon REALM WORLD BOSS. Reconstructed from the aced.gg dev blog. Her signature
    // EVIL PLAYER PHASE is modelled with 'recruitment' shots (proj1) that apply Confused — turning a player's
    // controls against the group — plus Evil Thrall summons. Then a world-transformation phase, then a clone
    // jumps in. Taunts here are RECONSTRUCTED deadpan (none recorded). Auto-registered by reflection.
    partial class BehaviorDb
    {
        private _ NyxOfFire = () => Behav()
            .Init("Nyx of Fire",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(16, "bulletHell")
                    ),
                    // P1: opening fire bullet-hell
                    new State("bulletHell",
                        new Taunt("You will burn with the rest of them."), // DESIGNED-no-footage (reconstructed deadpan; no recovery source — aced_NyxOfFire.json has no taunts. Fire-phase flavor, honestly labeled)
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(17, count: 12, shootAngle: 30, coolDown: 500),
                        new Shoot(17, count: 6, shootAngle: 60, predictive: 0.5, coolDown: 900),
                        new HpLessTransition(0.7, "evil")
                    ),
                    // P2: EVIL PLAYER PHASE — 'recruits' a player (Confused) + raises Evil Thralls
                    new State("evil",
                        new Taunt("Kneel. Now fight for me."), // DESIGNED-no-footage (reconstructed; no recovery source. Evil-player-recruit phase flavor, honestly labeled)
                        new Flash(0xff00aa, 0.3, 8),
                        new Spawn("Evil Thrall", maxChildren: 4, initialSpawn: 0.8, coolDown: 6000),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(16, projectileIndex: 1, count: 5, shootAngle: 40, coolDown: 1100),
                        new Shoot(16, count: 8, shootAngle: 45, coolDown: 700),
                        new TimedTransition(7000, "transform")
                    ),
                    // P3: the WORLD TRANSFORMS — everything burns
                    new State("transform",
                        new Flash(0xff3300, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(18, count: 16, shootAngle: 22, coolDown: 480),
                        new Shoot(18, projectileIndex: 1, count: 4, shootAngle: 90, coolDown: 1500),
                        new HpLessTransition(0.4, "clone")
                    ),
                    // P4: a CLONE jumps in — both spray fire
                    new State("clone",
                        new Taunt("There are two of me now."), // DESIGNED-no-footage (reconstructed; no recovery source. Clone-phase flavor, honestly labeled)
                        new Spawn("Nyx Clone", maxChildren: 1, initialSpawn: 1.0, coolDown: 999999),
                        new Flash(0xff0066, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(18, count: 14, shootAngle: 25, coolDown: 520),
                        new Shoot(18, projectileIndex: 1, count: 6, shootAngle: 60, coolDown: 1300)
                    )
                ),
                new Threshold(0.05,
                    new TierLoot(13, ItemType.Weapon, 0.16),
                    new TierLoot(14, ItemType.Armor, 0.16),
                    new TierLoot(6, ItemType.Ring, 0.14),
                    new ItemLoot("Potion of Life", 0.3),
                    new ItemLoot("Potion of Attack", 0.5)
                )
            )
            .Init("Evil Thrall",
                new State(
                    new Prioritize(new Follow(1.1, range: 8), new Wander(0.5)),
                    new Shoot(11, count: 3, shootAngle: 20, coolDown: 700)
                ),
                new Threshold(0.5, new TierLoot(5, ItemType.Potion, 0.06))
            )
            .Init("Nyx Clone",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(16, count: 10, shootAngle: 36, coolDown: 600),
                    new Shoot(16, count: 5, shootAngle: 72, predictive: 0.6, coolDown: 1000)
                )
            );
    }
}
