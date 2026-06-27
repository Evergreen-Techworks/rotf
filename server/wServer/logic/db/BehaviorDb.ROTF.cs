using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF vertical slice — the Illusion dungeon boss. Auto-registered via the `_`
    // field reflection in BehaviorDb's constructor (no manual wiring needed).
    partial class BehaviorDb
    {
        private _ ROTF = () => Behav()
            // MECH-LOOP 2026-06-24: attacks NO-FOOTAGE — no source tags; DESIGNED-no-footage confirmed.
            // Attacks (Shoot(13) + Spawn Illusion Mirage + enrage) are fully RECONSTRUCTED.
            .Init("The Illusionist",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(12, "fight")
                        ),
                    new State("fight",
                        new Prioritize(
                            new Wander(0.35)
                            ),
                        new Shoot(13, count: 5, shootAngle: 18, coolDown: 800),
                        new Shoot(13, coolDown: 1600),
                        new Spawn("Illusion Mirage", maxChildren: 4, initialSpawn: 0, coolDown: 8000, givesNoXp: false),
                        new HpLessTransition(0.35, "enrage")
                        ),
                    new State("enrage",
                        new Flash(0xff8800ff, 0.3, 10),
                        new Taunt("You cannot tell what is real!"), // DESIGNED-no-footage (invented flavor for the custom Illusion-dungeon boss + its Illusion Mirage clone gimmick; NOT a recovered ROTF line — checked: disc_JbQvbkBLRjE "illusionist" = The Mysterious Card, disc_9vQughNZIX0 = Loki, neither is this boss. Honestly labeled.)
                        new Prioritize(
                            new Wander(0.5)
                            ),
                        new Shoot(14, count: 8, shootAngle: 12, coolDown: 600),
                        new Shoot(14, count: 10, shootAngle: 36, fixedAngle: 0, coolDown: 1200)
                        )
                    ),
                new Threshold(0.01,
                    new TierLoot(4, ItemType.Potion, numRequired: 2, threshold: 0.1)
                    ),
                new Threshold(0.04,
                    new ItemLoot("Blade of the Fallen Sky", 0.08),
                    new ItemLoot("Asura", 0.08),
                    new ItemLoot("Bel's Decapitator", 0.06)
                    )
            )
            .Init("Illusion Mirage",
                new State(
                    new Prioritize(
                        new Wander(0.4)
                        ),
                    new Shoot(11, coolDown: 1000)
                    )
            );
    }
}
