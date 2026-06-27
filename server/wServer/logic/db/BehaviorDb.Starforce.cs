using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF STARFORCE ZONE (tech/space zone; roster footage-confirmed via event feed
    // "Starforce > The Zuck has been defeated", disc_55seoqVjso4). The 4-boss arena reuses
    // Cracked Core (0x8fd1) + Ortar (0x8fd0) from OCBosses; only The Zuck + Gem Gem are new here.
    // No boss TAUNTS were captured (event-feed only) -> tech/space lines RECONSTRUCTED (marked). Auto-registered.
    partial class BehaviorDb
    {
        private _ Starforce = () => Behav()
            .Init("The Zuck",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(15, "fight")
                    ),
                    // P1: tech overlord — wide grey-missile barrage + radial swirls
                    new State("fight",
                        new Taunt("I have collected all of your data."), // RECONSTRUCTED (no footage taunt)
                        new Flash(0x3b5998, 0.3, 10),
                        new Prioritize(new StayCloseToSpawn(0.25, 7), new Wander(0.3)),
                        new Shoot(15, count: 12, shootAngle: 30, coolDown: 700),
                        new Shoot(15, projectileIndex: 1, count: 4, shootAngle: 14, predictive: 0.5, coolDown: 1500),
                        new HpLessTransition(0.4, "overclock")
                    ),
                    // P2: faster, denser nova
                    new State("overclock",
                        new Taunt(0.0025, "Move fast and break things!"), // RECONSTRUCTED
                        new Flash(0x66aaff, 0.25, 12),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 16, shootAngle: 22, coolDown: 600),
                        new Shoot(16, projectileIndex: 1, count: 6, shootAngle: 20, coolDown: 1000)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.14),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            .Init("Gem Gem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "fight")
                    ),
                    // crystalline spread + geometric volleys
                    new State("fight",
                        new Taunt("Shine bright, mortal."), // RECONSTRUCTED
                        new Flash(0xff66cc, 0.3, 9),
                        new Prioritize(new StayCloseToSpawn(0.25, 6), new Wander(0.3)),
                        new Shoot(14, count: 10, shootAngle: 36, coolDown: 750),
                        new Shoot(14, count: 4, shootAngle: 90, fixedAngle: 0, coolDown: 1300)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.1),
                    new TierLoot(4, ItemType.Ring, 0.12)
                )
            )
            // GALACTIC ZONES boss — DESIGNED stub (no footage). Space outlaw; 2-phase gunslinger.
            // Theme inferred from Handcannon drop ("Overwhelmed With Power" primal). All taunts RECONSTRUCTED.
            .Init("L. Bandito",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "fight")
                    ),
                    new State("fight",
                        new Taunt("You wandered into the wrong galaxy, partner."), // RECONSTRUCTED — space cowboy opening
                        new Taunt(0.002, "Nobody outguns L. Bandito!"),             // RECONSTRUCTED — gunslinger boast
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.35)),
                        new Shoot(15, count: 6, shootAngle: 60, coolDown: 900),
                        new Shoot(15, projectileIndex: 1, count: 3, shootAngle: 15, predictive: 0.6, coolDown: 1400),
                        new HpLessTransition(0.4, "overwhelm")
                    ),
                    new State("overwhelm",
                        new Taunt("Fire everything! OVERWHELM THEM!"),          // RECONSTRUCTED — refs Handcannon "Overwhelmed With Power"
                        new Taunt(0.002, "There ain't no law out here in the void."), // RECONSTRUCTED — lawless space outlaw
                        new Flash(0xffaa22, 0.3, 10),
                        new Prioritize(new Wander(0.45)),
                        new Shoot(15, count: 12, shootAngle: 30, coolDown: 650),
                        new Shoot(15, projectileIndex: 1, count: 5, shootAngle: 12, predictive: 0.7, coolDown: 1000)
                    )
                ),
                new Threshold(0.1,
                    new ItemLoot("Handcannon", 0.07),       // confirmed drop (rotfserver wiki)
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.2)
                )
            );
    }
}
