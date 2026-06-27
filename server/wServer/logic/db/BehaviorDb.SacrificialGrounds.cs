using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF SACRIFICIAL GROUNDS (Medium reptilian-temple dungeon). Dev-blog: the Devoted Shaman's reptilian
    // hunters try to sacrifice players to a REPTILIAN GOD. Two-act fight — the Shaman summons hunters, and
    // at low HP completes the ritual to AWAKEN the Reptilian God (a second, larger boss). No taunts were
    // documented (reconstructed deadpan). Auto-registered by reflection.
    partial class BehaviorDb
    {
        private _ SacrificialGrounds = () => Behav()
            .Init("Devoted Shaman",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(13, "ritual")
                    ),
                    new State("ritual",
                        new Taunt("Your blood feeds the Old One!"), // DESIGNED-no-footage (invented flavor; reconstructed-deadpan from the aced.gg dev-blog roster aced_MediumWorldsRoster.json, which gives the boss NAME + sacrifice-to-a-'Reptilian God' theme but taunts:[] — no taunt text in any source. "Old One" is the invented deity name (roster says "Reptilian God"). Honestly labeled.)
                        new Spawn("Reptilian Hunter", maxChildren: 6, initialSpawn: 0.8, coolDown: 5000),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(14, count: 7, shootAngle: 30, coolDown: 800),
                        new HpLessTransition(0.25, "summon")
                    ),
                    // completes the ritual — the God rises
                    new State("summon",
                        new Taunt("RISE! RISE AND DEVOUR THEM!"), // DESIGNED-no-footage (invented ritual-climax flavor for the Reptilian-God summon; reconstructed from the aced.gg dev-blog theme, NO source taunt text. Honestly labeled.)
                        new Spawn("Reptilian God", maxChildren: 1, initialSpawn: 1.0, coolDown: 999999),
                        new Flash(0x44dd44, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(14, count: 9, shootAngle: 40, coolDown: 750)
                    )
                ),
                new Threshold(0.12,
                    new TierLoot(11, ItemType.Weapon, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.1),
                    new TierLoot(4, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.12)
                )
            )
            .Init("Reptilian Hunter",
                new State(
                    new Prioritize(new Follow(1.1, range: 8), new Wander(0.5)),
                    new Shoot(11, count: 2, shootAngle: 20, coolDown: 700)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.05))
            )
            .Init("Reptilian God",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(16, "fight")
                    ),
                    new State("fight",
                        new Spawn("Reptilian Hunter", maxChildren: 4, initialSpawn: 0.5, coolDown: 8000),
                        new Prioritize(new StayCloseToSpawn(0.3, 7), new Wander(0.3)),
                        new Shoot(16, count: 10, shootAngle: 36, coolDown: 700),
                        new HpLessTransition(0.4, "wrath")
                    ),
                    new State("wrath",
                        new Flash(0x22cc22, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 14, shootAngle: 25, coolDown: 550),
                        new Shoot(16, projectileIndex: 1, count: 6, shootAngle: 60, coolDown: 1200)
                    )
                ),
                new Threshold(0.08,
                    new TierLoot(13, ItemType.Weapon, 0.14),
                    new TierLoot(13, ItemType.Armor, 0.14),
                    new TierLoot(6, ItemType.Ring, 0.12),
                    new ItemLoot("Potion of Life", 0.25)
                )
            );
    }
}
