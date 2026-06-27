using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF COLLAPSED WOODS (Medium forest dungeon). Treesmasher is DORMANT until awoken (dev-blog): it
    // sits invulnerable + silent as fallen timber until players step into the grove, then erupts. No
    // taunts were documented (reconstructed deadpan). Auto-registered by reflection.
    // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_AUiz1LBnNJg f_00122: teal/aqua TRIDENT (proj-0)
    // projectiles in radial scatter during active fight (~60% HP wake/rampage). Radial spray confirms
    // count:8 wake pattern. proj-1 = Demon Blade (red/orange, SLOWS on hit) fires in rampage/fall —
    // not isolated in frame but RECONSTRUCTED from sibling dungeon design. Arena DOCUMENTED f_00120.
    partial class BehaviorDb
    {
        private _ CollapsedWoods = () => Behav()
            .Init("Treesmasher",  // attacks PARTIAL disc_AUiz1LBnNJg f_00122: Trident (proj-0, teal/aqua) radial spray CONFIRMED; Demon Blade (proj-1, SLOWS) rampage/fall RECONSTRUCTED.
                new State(
                    // dormant: looks like collapsed timber — invulnerable, motionless, harmless
                    new State("dormant",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(7, "wake")
                    ),
                    new State("wake",
                        new Taunt("...who disturbs the deadwood?"), // DESIGNED-no-footage (invented; the real recovered Treesmasher wake line is "You have awoken the..." below)
                        new Taunt(0.0016, "You have awoken the..."), // VERIFIED-partial disc_AUiz1LBnNJg (frame-read stem "Treesmasher: You have awoken the..."; completion CUT off-screen). PATTERN now CONFIRMED on 2 sibling Lords: Firebreather "You have awoken the spirit of fire..." (disc_-9_o5EilupU) + Shadowscale "You have awoken the spirit of shadow..." (disc_OpvS02UXO-I) -> Treesmasher's is structurally "...spirit of the [woods/forest/nature]...", but the EXACT noun was NEVER captured for Treesmasher -> NOT completing the in-game text (no guessing). The auto-bridge's "[Treesmasher/woods]" is a bracketed guess, deliberately NOT injected. Kept the confirmed stem; flag for a clean Treesmasher-wake frame to finish the line.
                        new Spawn("Living Twig", maxChildren: 5, initialSpawn: 0.8, coolDown: 6000),
                        new Spawn("Forest Spider", maxChildren: 3, initialSpawn: 0.5, coolDown: 9000),
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.3)),
                        new Shoot(15, count: 8, shootAngle: 45, coolDown: 750),
                        new HpLessTransition(0.5, "rampage")
                    ),
                    new State("rampage",
                        new Flash(0x33aa33, 0.3, 8),
                        new Spawn("Angry Sapling", maxChildren: 3, initialSpawn: 0.5, coolDown: 8000),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 700),
                        new Shoot(15, projectileIndex: 1, count: 4, shootAngle: 30, coolDown: 1700),
                        new HpLessTransition(0.2, "fall")
                    ),
                    new State("fall",
                        new Flash(0x117711, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 14, shootAngle: 25, coolDown: 600),
                        new Shoot(16, projectileIndex: 1, count: 6, shootAngle: 60, coolDown: 1200)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.18),
                    new ItemLoot("Potion of Defense", 0.4)
                )
            )
            .Init("Living Twig",
                new State(
                    new Prioritize(new Follow(0.8, range: 8), new Wander(0.4)),
                    new Shoot(9, count: 1, coolDown: 850)
                ),
                new Threshold(0.5, new TierLoot(3, ItemType.Potion, 0.04))
            )
            .Init("Forest Spider",
                new State(
                    new Prioritize(new Follow(1.2, range: 9), new Wander(0.6)),
                    new Shoot(10, count: 2, shootAngle: 20, coolDown: 700)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.05))
            )
            .Init("Angry Sapling",
                new State(
                    new Prioritize(new StayCloseToSpawn(0.4, 2)),
                    new Shoot(11, count: 4, shootAngle: 90, coolDown: 950)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.05))
            );
    }
}
