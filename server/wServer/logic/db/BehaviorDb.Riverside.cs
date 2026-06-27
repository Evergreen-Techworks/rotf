// MECH-LOOP: Arena/map comments added 2026-06-16. Verified from disc_wsRSijbExRI (VoltiPlay ROTF footage). All comments below are DOCUMENTED-from-frames unless marked RECONSTRUCTED.
// MECH-LOOP 2026-06-24: attacks re-scan attempt — all captured frames (f_00169/f_00171/f_00172) show Riverborn 0% HP;
// no active bullet patterns visible. Multi-phase behavior (wade/wind/dive/surface) remains RECONSTRUCTED.
// NOTE: f_00172 chat reads "...play in the rain!" (4-char word); existing build says "...in the river!" (5-char).
// Both fit the theme; prior correction from "realm"→"river" is trusted. No change to taunt text.
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF RIVERSIDE REFUGE (Medium swamp dungeon). Riverborn reconstructed from the aced.gg dev blog:
    // TORRENTS OF WIND -> a DIVE (submerge = Invulnerable + Invisible, stalk a player) -> SURFACE-STRIKE
    // burst, looping. The Lurker / Living Brush is an impostor (stays Invisible until you walk into it).
    // No taunts were documented (kept silent). Auto-registered by reflection.
    partial class BehaviorDb
    {
        private _ Riverside = () => Behav()
            .Init("Riverborn",  // arena: Riverside Refuge dungeon — outdoor open swamp/river dungeon (no enclosed stone walls); floor: dark teal/blue-gray water tiles (the boss fights in an open water section of the river); dark green reed/plant decorations scattered at edges of fight area; green swamp terrain surrounds the water. DOCUMENTED-from-frames disc_wsRSijbExRI f_00173 (active fight: "Riverborn / 0%" boss label visible top-center; dark teal water tile floor; player crowd + boss entity in water; reed decorations at edges) + minimap_00022 (outdoor swamp/river dungeon minimap: large green swamp terrain area with distinct blue water section — no dungeon walls, open outdoor style).
                new State(
                    new State("idle",
                        new PlayerWithinTransition(15, "wade")
                    ),
                    // wades up out of his home waters; calls the swamp to him
                    new State("wade",
                        new Taunt("Let's see how well you play in the river!"), // CORRECTED disc_wsRSijbExRI (was truncated "Let's see how well you play..."; the auto-bridge read it "...in the realm" but the frame clearly says "...in the river!" — fits the Riverside theme; speaker prefix "Riverborn:" confirmed)
                        new Taunt(0.0016, "Only one of us is coming out of here alive..."), // VERIFIED disc_wsRSijbExRI (frame-read exact: "Riverborn: Only one of us is coming out of here alive...")
                        new Spawn("River Strider", maxChildren: 4, initialSpawn: 0.6, coolDown: 9000),
                        new Spawn("River Serpent", maxChildren: 2, initialSpawn: 0.5, coolDown: 11000),
                        new Prioritize(new StayCloseToSpawn(0.3, 6), new Wander(0.3)),
                        new Shoot(15, count: 6, shootAngle: 60, coolDown: 800),
                        new TimedTransition(2500, "wind")
                    ),
                    // TORRENTS OF WIND — dense rotating radial fire (the spinning attack)
                    new State("wind",
                        new Flash(0x66ccff, 0.3, 6),
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.4)),
                        new Shoot(16, count: 12, shootAngle: 30, coolDown: 350),
                        new TimedTransition(4500, "dive")
                    ),
                    // DIVE — submerges (invulnerable + invisible) and stalks the nearest player
                    new State("dive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ConditionalEffect(ConditionEffectIndex.Invisible),
                        new Follow(1.4, acquireRange: 16, range: 0.5),
                        new TimedTransition(2600, "surface")
                    ),
                    // SURFACE-STRIKE — bursts out from under the player, heavy point-blank shotgun
                    new State("surface",
                        new Flash(0x3388ff, 0.2, 8),
                        new Shoot(4, projectileIndex: 1, count: 16, shootAngle: 22, coolDown: 600),
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 700),
                        new TimedTransition(1800, "wind")
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.18),
                    new ItemLoot("Potion of Speed", 0.4)
                )
            )
            .Init("River Strider",
                new State(
                    new Prioritize(new Follow(0.85, range: 9), new Wander(0.4)),
                    new Shoot(11, count: 2, shootAngle: 20, coolDown: 850)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.06))
            )
            // impostor: hides as scenery (Invisible) until a player steps close, then ambushes
            .Init("Living Brush",
                new State(
                    new State("hide",
                        new ConditionalEffect(ConditionEffectIndex.Invisible),
                        new PlayerWithinTransition(3.5, "ambush")
                    ),
                    new State("ambush",
                        new Prioritize(new Follow(0.9, range: 6), new Wander(0.3)),
                        new Shoot(10, count: 5, shootAngle: 40, coolDown: 700)
                    )
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.07))
            )
            // flying snake: drifts and fires slow tricky bubbles
            .Init("River Serpent",
                new State(
                    new Prioritize(new Wander(0.6)),
                    new Shoot(13, count: 3, shootAngle: 24, coolDown: 1100)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.07))
            )
            .Init("Swamp Frog",
                new State(
                    new Prioritize(new Follow(1.1, range: 7), new Wander(0.6)),
                    new Shoot(8, count: 1, coolDown: 900)
                ),
                new Threshold(0.6, new TierLoot(3, ItemType.Potion, 0.04))
            )
            .Init("Tree Bug",
                new State(
                    new Prioritize(new Wander(0.7)),
                    new Shoot(9, count: 3, shootAngle: 30, coolDown: 800)
                ),
                new Threshold(0.6, new TierLoot(3, ItemType.Potion, 0.04))
            );
    }
}
