using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // ROTF SHROUDED SANCTUM (Medium shadow dungeon). Shadowscale 'hides secrets' (dev-blog): it cloaks
    // (Invisible) to stalk, then reappears to strike, looping. Its shots Blind. No taunts were documented
    // (reconstructed deadpan). Auto-registered by reflection.
    // MECH-LOOP 2026-06-24: attacks PARTIAL — disc_OpvS02UXO-I f_00166: Shadowscale visible at top of frame
    // doing radial spray of RED/ORANGE DEMON BLADE (proj-0) bullets in dark purple vine dungeon. Pattern is
    // wide scatter ring (count:8 RECONSTRUCTED from density). Demon Blade (proj-0) CONFIRMED in fight state.
    // proj-1 = Thunder Swirl with Blind — fires in strike state burst (white X-shapes visible f_00168 but
    // ambiguous with dungeon decorations — RECONSTRUCTED; not definitively isolated).
    // Prior MECH-LOOP: Boss confirmed in disc_-9_o5EilupU b002_4s_002 — white ghost-type entity on dark
    // circular-dot dungeon floor. 3-4 Shade minions confirmed. Bullet geometry was UNVERIFIABLE-ILLEGIBLE
    // (Shade minions also shoot, could not attribute to boss) — NOW UPGRADED to PARTIAL via f_00166.
    // Arena DOCUMENTED: dark multi-room DUNGEON — dark purple vine/plant walls, dark grey stone floor tiles
    // (disc_OpvS02UXO-I f_00166/f_00168); minimap_00001 narrow corridor shape; minimap_00002 scattered rooms.
    partial class BehaviorDb
    {
        private _ ShroudedSanctum = () => Behav()
            .Init("Shadowscale",  // attacks PARTIAL disc_OpvS02UXO-I f_00166: Demon Blade (proj-0, red/orange) radial spray CONFIRMED. proj-1 Thunder Swirl/Blind strike burst RECONSTRUCTED.
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "fight")
                    ),
                    new State("fight",
                        new Taunt("You have awoken the spirit of shadow..."), // SOURCED-2x-spec (disc_OpvS02UXO-I "DOCUMENTED VERBATIM", completing the truncated disc_LS16CHnxyhU version; matches the confirmed sibling awaken-pattern Firebreather "spirit of fire"/Shadowscale "spirit of shadow"). Frame-read attempted this pass but the line is event-feed-buried among 68 crops (top-red frames were realm/Abyss content) -> not frame-re-isolated; provenance solid, text unchanged.
                        new Taunt(0.0015, "Do not think you're invincible, you sure aren't..."), // SOURCED disc_OpvS02UXO-I (DOCUMENTED VERBATIM per recovery spec, completes the disc_LS16CHnxyhU truncation). Not frame-re-isolated this pass (event-feed-buried); no error evidence -> kept as-is.
                        new Taunt(0.0015, "I will reward you with a swift death. Meet your end..."), // SOURCED disc_OpvS02UXO-I (DOCUMENTED VERBATIM; "Meet your end" completes the spec's "Mee[t your end]..." bracket). Not frame-re-isolated this pass; no error evidence -> kept as-is.
                        new Spawn("Shade", maxChildren: 4, initialSpawn: 0.6, coolDown: 7000),
                        new Spawn("Sanctum Cultist", maxChildren: 3, initialSpawn: 0.5, coolDown: 9000),
                        new Prioritize(new StayCloseToSpawn(0.25, 5), new Wander(0.35)),
                        new Shoot(15, count: 8, shootAngle: 45, coolDown: 750),  // PARTIAL f_00166: Demon Blade (proj-0, red/orange) CONFIRMED
                        new TimedTransition(5000, "cloak")
                    ),
                    // hides — Invisible, stalks the nearest player
                    new State("cloak",
                        new ConditionalEffect(ConditionEffectIndex.Invisible),
                        new Follow(1.3, acquireRange: 16, range: 0.5),
                        new TimedTransition(2400, "strike")
                    ),
                    // reappears under a player — Blinding burst
                    new State("strike",
                        new Flash(0x6622aa, 0.2, 8),
                        new Shoot(13, projectileIndex: 1, count: 12, shootAngle: 30, coolDown: 600),  // Thunder Swirl with Blind (proj-1) — RECONSTRUCTED; not isolated vs dungeon deco in f_00168
                        new Shoot(13, count: 8, shootAngle: 45, coolDown: 700),  // Demon Blade (proj-0) — RECONSTRUCTED count/angle for strike burst
                        new TimedTransition(1800, "fight")
                    )
                ),
                new Threshold(0.1,
                    new TierLoot(12, ItemType.Weapon, 0.12),
                    new TierLoot(13, ItemType.Armor, 0.12),
                    new TierLoot(5, ItemType.Ring, 0.13),
                    new ItemLoot("Potion of Life", 0.18),
                    new ItemLoot("Potion of Wisdom", 0.4)
                )
            )
            .Init("Shade",
                new State(
                    new Prioritize(new Wander(0.7)),
                    new Shoot(11, count: 3, shootAngle: 30, coolDown: 850)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.05))
            )
            .Init("Sanctum Cultist",
                new State(
                    new Prioritize(new Follow(0.9, range: 8), new Wander(0.3)),
                    new Shoot(12, count: 4, shootAngle: 30, coolDown: 800)
                ),
                new Threshold(0.5, new TierLoot(4, ItemType.Potion, 0.05))
            )
            .Init("Shadow Sentinel",
                new State(
                    new Prioritize(new StayCloseToSpawn(0.4, 2)),
                    new Shoot(12, count: 6, shootAngle: 60, coolDown: 900)
                ),
                new Threshold(0.5, new TierLoot(5, ItemType.Potion, 0.07))
            );
    }
}
