using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // AUTO-GENERATED default behaviors for the imported real-ROTF enemies
    // (scripts/gen_imported_behaviors.py). Stats are real; AI is a sensible default
    // (wander + shoot, bosses get spread + tier loot). Hand-tune marquee bosses separately.
    partial class BehaviorDb
    {
        private _ ROTFImported = () => Behav()
            .Init("DS Bat",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Rat",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Natural Slime God",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Goblin Peon",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Goblin Warlock",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Goblin Sorcerer",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Goblin Brute",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Goblin Knight",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Alligator",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Brown Slime",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Yellow Slime",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Gulpord the Slime God",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900),
                    new Shoot(8, projectileIndex: 1, coolDown: 1600)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("DS Gulpord the Slime God M",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900),
                    new Shoot(8, projectileIndex: 1, coolDown: 1600)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("DS Gulpord the Slime God S",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Master Rat",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("DS Brown Slime Trail",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("DS Yellow Slime Trail",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("ic CreepyTime",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("ic Whirlwind",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("ic boss purifier generator",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("ic boss purifier",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("ic shielded king",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("ic Esben the Unwilling",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900),
                    new Shoot(8, projectileIndex: 1, coolDown: 1600)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("Snow Bat Mama",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("Snow Bat",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("Mini Yeti",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("Big Yeti",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("Pile of Bones God",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("Megamad Oryx Guardian Sword",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900),
                    new Shoot(8, projectileIndex: 1, coolDown: 1600)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("Megamad Oryx Stone Guardian Right",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Megamad Oryx Stone Guardian Left",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Megamad Oryx Brute",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Megamad Oryx Eye Warrior",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900),
                    new Shoot(8, projectileIndex: 1, coolDown: 1600)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("Ring Bullet",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, count: 3, shootAngle: 15, coolDown: 900)
                    ),
                new Threshold(0.05,
                    new TierLoot(4, ItemType.Potion, numRequired: 1, threshold: 0.2)
                    )
            )
            .Init("Spirit of Oryx",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Possessed Oryx Statue",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Oryx Darkness Trap",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Oryx Daze Trap",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Oryx Confuse Trap",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("meteor explosion",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("EarthSmash",
                new State(
                    new Prioritize(new Wander(0.45)),
                    new Shoot(8, coolDown: 1100)
                    )
            )
            .Init("Hook Head",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Hook Link A",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Hook Link B",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Hook Link C",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Hook Link D",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Hook Link E",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Megamad Oryx Statue",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 8, shootAngle: 45, fixedAngle: 0, coolDown: 1500)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("TG",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("Head",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            )
            .Init("BB Biff the Buffed Bunny",
                new State(
                    new Prioritize(new Wander(0.5)),
                    new Shoot(9, count: 5, shootAngle: 20, coolDown: 700),
                    new Shoot(9, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 1400)
                    ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02),
                    new TierLoot(12, ItemType.Armor, numRequired: 1, threshold: 0.02)
                    )
            );
    }
}
