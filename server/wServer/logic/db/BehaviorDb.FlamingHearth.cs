using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF FLAMING HEARTH (Medium fire dungeon). Firebreather reconstructed from the aced.gg dev blog:
    // a returning elemental fire dragon. fight -> inferno (lava globs + turret flowers + tumbleweeds)
    // -> rage. Fire shots Bleed (burn DoT). Auto-registered.
    // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_AUiz1LBnNJg f_00123/f_00127: dense orange/red
    // DEMON BLADE (proj-0) radial spray during fight state CONFIRMED. proj-1 = Thunder Swirl (slow
    // lava globs, orange/yellow) in inferno/rage RECONSTRUCTED. Wake taunt VERIFIED f_00125.
    partial class BehaviorDb
    {
        private _ FlamingHearth = () => Behav()
            .Init("Firebreather",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(15, "fight")
                    ),
                    // P1: sweeping fire breath, salamanders + armadillos pour in
                    new State("fight",
                        new Taunt("You have awoken the spirit of fire!"), // VERIFIED disc_AUiz1LBnNJg f_00125 (chat: "You have awoken the spirit of fire" — Firebreather wake taunt, fires once on player detect)
                        new Taunt(0.0016, "Your courage softens me... considering it won't be lasting much longer!"), // CORRECTED disc_rQseJUyAHU0
                        new Taunt(0.0016, "Death is your reward! Say no more and I'll make it quick!"), // CORRECTED disc_rQseJUyAHU0
                        new Spawn("Salamander", maxChildren: 5, initialSpawn: 0.6, coolDown: 7000),
                        new Spawn("Armored Armadillo", maxChildren: 2, initialSpawn: 0.4, coolDown: 12000),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(15, count: 7, shootAngle: 18, coolDown: 700),
                        new HpLessTransition(0.6, "inferno")
                    ),
                    // P2: the Hearth erupts — slow lava globs + turret flowers + rolling tumbleweeds
                    new State("inferno",
                        new Flash(0xff6600, 0.3, 8),
                        new Spawn("Firespitting Flower", maxChildren: 3, initialSpawn: 0.5, coolDown: 9000),
                        new Spawn("Flaming Tumbleweed", maxChildren: 4, initialSpawn: 0.6, coolDown: 6000),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(15, count: 8, shootAngle: 45, coolDown: 750),
                        new Shoot(15, projectileIndex: 1, count: 3, shootAngle: 30, coolDown: 1800),
                        new HpLessTransition(0.25, "rage")
                    ),
                    // P3: full burn — dense fire everywhere
                    new State("rage",
                        new Flash(0xff2200, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 14, shootAngle: 25, coolDown: 550),
                        new Shoot(16, projectileIndex: 1, count: 4, shootAngle: 90, coolDown: 1300)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.18),
                    new ItemLoot("Potion of Attack", 0.4)
                )
            )
            // fast skittering lizard
            .Init("Salamander",
                new State(
                    new Prioritize(new Follow(1.3, range: 8), new Wander(0.6)),
                    new Shoot(10, count: 1, coolDown: 650)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.06))
            )
            // slow armored bruiser
            .Init("Armored Armadillo",
                new State(
                    new Prioritize(new Follow(0.55, range: 9), new Wander(0.2)),
                    new Shoot(11, count: 3, shootAngle: 30, coolDown: 950)
                ),
                new Threshold(0.5, new TierLoot(5, ItemType.Potion, 0.08))
            )
            // stationary turret flower
            .Init("Firespitting Flower",
                new State(
                    new Prioritize(new StayCloseToSpawn(0.4, 1)),
                    new Shoot(12, count: 6, shootAngle: 60, coolDown: 900)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.06))
            )
            // rolling hazard — fast, erratic, short fiery bursts
            .Init("Flaming Tumbleweed",
                new State(
                    new Prioritize(new Wander(0.9)),
                    new Shoot(6, count: 8, shootAngle: 45, coolDown: 500)
                ),
                new Threshold(0.6, new TierLoot(3, ItemType.Potion, 0.03))
            );
    }
}
