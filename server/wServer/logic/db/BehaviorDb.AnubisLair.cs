using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF ANUBI'S LAIR (Hard). Gold sandstone Egyptian temple. Anubis is invulnerable and
    // shielded by his 4 PILLARS ("Pillars... please help me!"); destroy them all to break the
    // ward, then he weakens ("I am losing strength... I am slowly dying...") and is killable.
    // TAUNTS are verbatim-recovered from footage; the PILLARS ward is a drop-in on Ra-and-Staves
    // (BehaviorDb.GateUnderworld.cs); attack geometry reconstructed (no fight-frame data). Auto-registered.
    partial class BehaviorDb
    {
        private _ AnubisLair = () => Behav()
            .Init("Anubis",  // arena: multi-room Egyptian temple, large central rectangular boss chamber (gold/yellow floor), brown sandstone walls, 4-5 chambers, 2 blue pools — DOCUMENTED-from-minimap disc_AAQEUfbwkyQ:00021
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "intro")
                    ),
                    // spawn the 4 Pillars, brief beat so they exist before the invuln check
                    new State("intro",
                        new Taunt("So... you dare disturb the rites of the dead?"), // DESIGNED-no-footage (invented intro flavor; not in any recovery spec)
                        new Spawn("Anubis's Pillar", maxChildren: 4, initialSpawn: 1.0, coolDown: 999999),
                        new TimedTransition(900, "warded")
                    ),
                    // P1: invulnerable behind the Pillars — radial pressure + a slow aimed volley
                    new State("warded",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt(0.002, "Pillars... please help me!"), // VERIFIED-3x-spec (disc_t8v5IlBPPJk + disc_TkraL4lSyDw + disc_XGl3pTaNmTM all record it verbatim for Anubis)
                        new Flash(0xffcc33, 0.3, 8),
                        new Prioritize(new StayCloseToSpawn(0.2, 4), new Wander(0.2)),
                        new Shoot(15, count: 10, shootAngle: 36, coolDown: 750),
                        new Shoot(15, projectileIndex: 1, count: 3, shootAngle: 12, predictive: 0.5, coolDown: 1600),
                        new EntityNotExistsTransition("Anubis's Pillar", 50, "weakened")
                    ),
                    // P2: ward broken — vulnerable, weakening, faster and more frantic
                    new State("weakened",
                        new Taunt("I am losing strength... I am slowly dying..."), // SPEC-DOCUMENTED disc_t8v5IlBPPJk (single source; weakened-phase line; frame not relocated this pass)
                        new Flash(0xff7700, 0.25, 10),
                        new Prioritize(new Wander(0.4)),
                        new Shoot(16, count: 12, shootAngle: 30, coolDown: 600),
                        new Shoot(16, projectileIndex: 1, count: 5, shootAngle: 20, coolDown: 1100)
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.14),
                    new ItemLoot("Potion of Life", 0.2)
                )
            )
            .Init("Anubis's Pillar",
                new State(
                    new Shoot(12, count: 4, shootAngle: 90, coolDown: 1100)
                ),
                new Threshold(0.5,
                    new TierLoot(4, ItemType.Potion, 0.1))
            );
    }
}
