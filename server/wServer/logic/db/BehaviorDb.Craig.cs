using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    // Craig's Castle boss — Craig the Mad Intern. Stats are REAL (ripped from the X3.1.1
    // ROTF client: 0x1fe2, 161000 HP, Def 50). The AI is designed (client data carries no
    // behaviors). Auto-registered via the `_`-field reflection in BehaviorDb's ctor.
    // MECH-LOOP: Arena DOCUMENTED from GeneralStream minimap_00160 (boss room) + minimap_00240
    // (full dungeon) + f_00210 (floor detail). Craig's Castle dungeon: complex multi-room
    // tan/cream castle with many branching corridors. Craig's boss chamber: large rounded-rectangular
    // room with horizontal stripe banding (alternating light/medium tan rows on minimap = office
    // carpet tile rows). Floor: warm tan/cream large sandstone tiles with grid seam lines
    // (f_00210). Narrow top-entry corridor. NOTE: footage shows boss name "Craig, The Worst
    // Nightmare" (disc_3eM07g4Ly5k f_00300, "Craig's Castle opened by kitttycat" confirmed same
    // dungeon) — may be an earlier alias. Current entity name "Craig the Mad Intern" matches
    // the X3.1.1 client XML. bossends empty → PARTIAL ATTACK RECOVERY from fight frames:
    //
    // ATTACK GEOMETRY OBSERVED (GeneralStream f_00198/f_00202/f_00205/f_00210 — PARTIAL):
    //   PRIMARY: ORANGE FIRE projectiles in a CROSS/RING pattern. f_00198 shows a clear DIAGONAL X
    //   (arms at ~45°/135°/225°/315°); f_00202 shows a CARDINAL CROSS (arms at 0°/90°/180°/270°).
    //   Both frames are Craig's fight in the same dungeon at different moments → the attack is an
    //   8-directional star (fixedAngle ring, count:8 at 45° spacing) OR two alternating shots
    //   (count:4 at 90° cardinal + count:4 at 90° diagonal offset). Arms are multi-bullet streams
    //   from repeated salvo stacking — exact count per salvo UNRESOLVABLE from accumulated bullets.
    //   SECONDARY: YELLOW STAR-SHAPED projectiles visible in diagonal gaps (different projectile type).
    //   WHAT IS RULED OUT: a 5-bullet narrow 20° spread (Phase1 prior RECONSTRUCTED code) and
    //   a 12-bullet 30° ring (Phase2 prior RECONSTRUCTED code) — neither matches the clearly visible
    //   4-arm / 8-arm cross pattern. Exact cooldowns remain RECONSTRUCTED (no timing data in frames).
    partial class BehaviorDb
    {
        private _ Craig = () => Behav()
            .Init("Craig the Mad Intern",  // arena: large rounded-rectangular Craig's Castle boss chamber — tan/cream sandstone tile floor, horizontal stripe banding, narrow top-entry corridor. DOCUMENTED GeneralStream minimap_00160 + f_00210.
                new State(
                    new State("idle",
                        new PlayerWithinTransition(14, "deadlines")
                        ),
                    // phase 1: overworked — cross-pattern fire + summons interns
                    new State("deadlines",
                        new Prioritize(
                            new StayCloseToSpawn(0.4, 6),
                            new Wander(0.3)
                            ),
                        // PARTIAL-DOCUMENTED (GeneralStream f_00198/f_00202): orange fire bullets in CROSS pattern.
                        // Exact count per salvo unclear from frames; 4-cardinal or 8-star observed. Cooldown RECONSTRUCTED.
                        new Shoot(14, count: 4, shootAngle: 90, fixedAngle: 0, coolDown: 900),           // cardinal cross (PARTIAL — cross geometry observed; count/cooldown reconstructed)
                        new Shoot(14, count: 4, shootAngle: 90, fixedAngle: 45, projectileIndex: 1, coolDown: 1350), // diagonal X (PARTIAL — yellow star secondary observed; exact timing reconstructed)
                        new Spawn("Stressed Intern", maxChildren: 6, initialSpawn: 0.3, coolDown: 6000),
                        new HpLessTransition(0.5, "overtime")
                        ),
                    // phase 2: overtime — faster cross-fire + more interns
                    new State("overtime",
                        new Flash(0xffffaa00, 0.3, 8),
                        new Taunt("I'M STARTING TO GET ANGRY!!!"), // verbatim, recovered from video disc_3eM07g4Ly5k
                        new Prioritize(
                            new Wander(0.5)
                            ),
                        // PARTIAL-DOCUMENTED: same cross/star geometry but faster cadence (RECONSTRUCTED escalation)
                        new Shoot(15, count: 4, shootAngle: 90, fixedAngle: 0, coolDown: 600),           // faster cardinal (PARTIAL)
                        new Shoot(15, count: 4, shootAngle: 90, fixedAngle: 45, projectileIndex: 1, coolDown: 900), // faster diagonal (PARTIAL)
                        new Spawn("Stressed Intern", maxChildren: 10, initialSpawn: 0, coolDown: 4500),
                        new HpLessTransition(0.2, "meltdown")
                        ),
                    // phase 3: meltdown — manic bullet hell
                    new State("meltdown",
                        new Flash(0xffff2200, 0.2, 12),
                        // verbatim enrage taunts recovered from video disc_3eM07g4Ly5k ('NOOOO' death cry handled by the realm announcer)
                        new Taunt("HAHAHA, I AM INCREASING IN SIZE SO I CAN DESTROY YOU EVEN MORE!!!"), // CORRECTED disc_Fx5_2g1_1O4 (was two SPLIT fragments "HAHAHA! I AM INCREASING IN STRENGTH!" + "DESTROY YOU EVEN MORE!!!"; the UNCUT line in disc_Fx5_2g1_1O4 reads as ONE continuous taunt -> "STRENGTH" was a guessed completion for the cut word, actual is "SIZE", and "DESTROY YOU EVEN MORE" is the same line's tail. comma after HAHAHA per prior specs)
                        new Taunt("PREPARE TO DIE!!!"), // VERIFIED-4x-spec (GeneralStream + disc_YC4vDxWTAyc + disc_awf8n3A7KOk + disc_wsRSijbExRI) — was missing from build
                        new Taunt(0.0016, "So if you don't want to suffer... I might just have to kill you..."), // VERIFIED-4x-spec (disc_Fx5_2g1_1O4 [uncut] + disc_YC4vDxWTAyc + disc_wsRSijbExRI + LongRPE). First clause: 3 specs read "su[ffer]" -> "suffer"; disc_Fx5 paraphrased it "say thanks to me" (3-vs-1 favors "suffer"). Second clause "I might just have to kill you..." from disc_Fx5 uncut. (NOTE: the two disc_SPBkDdC5JGM lines — "Obvi[ously I knew]... my next tri[ck]..." / "You [...] fix that problem with some gain!" — were DEFERRED, not added: single-source, illegible bracketed gaps, the std 224px chat crop cuts those tails and frames weren't cleanly re-readable this pass -> flagged for a targeted wide re-crop rather than injecting guessed text)
                        new Prioritize(
                            new Wander(0.6)
                            ),
                        new Shoot(16, count: 10, shootAngle: 36, coolDown: 450),
                        new Shoot(16, count: 6, shootAngle: 15, predictive: 0.5, coolDown: 800)
                        )
                    ),
                new Threshold(0.01,
                    new TierLoot(5, ItemType.Potion, numRequired: 2, threshold: 0.1),
                    new TierLoot(11, ItemType.Weapon, numRequired: 1, threshold: 0.02)
                    ),
                new Threshold(0.05,
                    new ItemLoot("Shield of Vendettas", 0.10)
                    )
            )
            .Init("Stressed Intern",
                new State(
                    new Prioritize(
                        new Wander(0.4),
                        new StayCloseToSpawn(0.5, 10)
                        ),
                    new Shoot(10, coolDown: 1100)
                    )
            );
    }
}
