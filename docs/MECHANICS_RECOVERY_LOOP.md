# MECHANICS RECOVERY LOOP

Recovers the **non-taunt** layers of built ROTF bosses from **already-prepped frames**
(attacks / maps / loot / stats). It is the counterpart to the TAUNT VERIFY LOOP:
taunts are ~90% frame-grounded, but attack patterns are ~83% reconstructed and
maps/loot/stats are mostly designed. This loop closes that gap.

**Key property:** it runs off frames already on disk, so it is productive even while
bulk_prep is stalled / discovery is dry. Current runway (see `docs/mechanics_recovery.json`
summary): ~36 bosses attack-addressable (have `bossends/`), ~70 map-addressable (have `minimap_`).

**Sprites are NOT in scope** — they cannot be recovered from compressed video frames
(needs original assets / hand-art). Do not attempt sprite extraction.

---

## VERBATIM LOOP PROMPT (fire this on a cron, like the other loops)

AUTONOMOUS MECHANICS RECOVERY LOOP — upgrade ONE built boss's NON-TAUNT layers (attacks/maps/loot/stats) from already-prepped frames, then STOP. Fresh context; everything is on disk in /home/jesse/rotf. Do NOT loop, do NOT process more than one boss. This loop turns RECONSTRUCTED/designed mechanics into DOCUMENTED-from-frames where the footage allows, and is scrupulously honest about what it cannot read. Sprites are OUT OF SCOPE (not recoverable from video).

STEP 0: Read docs/mechanics_recovery.json. If it is missing or a recently-built boss is absent (stale), regenerate with `python3 scripts/mechanics_recovery_ledger.py` (it PRESERVES prior verdicts). 

STEP 1: Pick the target boss, in this priority order:
  (A) FIRST boss with attacks_status NOT in (documented, no-footage, unverifiable-illegible) AND a non-empty bossends_tags  -> ATTACK recovery.
  (B) else FIRST boss with map_status == "undocumented" AND a non-empty minimap_tags  -> MAP recovery.
  If neither exists, append "[<time>] mechanics recovery: all addressable bosses documented" to docs/build_progress.md and STOP.

STEP 2: Mine the chosen boss's source frames (docs/video-recovery/frames/<tag>/ for a tag in its bossends_tags / minimap_tags). Open its .Init block in server/wServer/logic/db/<file>.
  (a) ATTACKS (primary when bossends exist): montage each bossends/ b###_at<S>s group (upscale 2-3x, -colorspace Gray -level 8-18% -unsharp). TRACE the bullet geometry into this controlled vocab so it maps to a Shoot behaviour: spiral/pinwheel (arm count + CW/CCW) | radial ring/nova (count) | spread/cone/shotgun (predictive) | aimed/predictive line | diamond/geometric/cross (fixedAngle) | wave/sweeping | wall/grid | dive/stalk | summon-only. Record count/angle where readable. Compare to the boss's existing Shoot(...) calls:
       * footage CONFIRMS the existing params  -> relabel that Shoot's comment `// DOCUMENTED-from-frames <tag> (b###)`.
       * footage CONTRADICTS  -> EDIT the Shoot params (count / shootAngle / projectileIndex / coolDown / fixedAngle / predictive) to match the traced geometry, and stamp `// CORRECTED-from-frames <tag> (was: <old>)`.
       * bossend group is an OVERLAY/UI fade or otherwise NOT a real fight (false positive) -> skip it, note it.
       * illegible / not confident -> leave RECONSTRUCTED, set attacks_status="unverifiable-illegible", do NOT invent geometry.
     Also record phases (per-HP colorFlash/pattern change) and colorFlashes if visibly documented.
  (b) MAP (when minimaps exist): montage 3-6 minimap_*.png + ~6 spaced f_*.png. Describe the ACTUAL arena layout (overall shape, room sequence, arena geometry, choke points) and write it into the boss's recovery spec `map` field (docs/video-recovery/<tag>.json) AND/OR a one-line `// arena: ...` comment in the .Init block. Set map_status="documented".
  (c) LOOT (opportunistic): grep/scan that tag's chat_*.png for "<player> has looted <X>" / "just unboxed ... <X>" lines occurring right after this boss's death-feed. Record OBSERVED drops as flat strings in the spec `drops` (do NOT fabricate a full RNG table). Optionally nudge a TierLoot tier toward an observed item, labeled. If nothing observed, leave designed, set loot_status accordingly.
  (d) STATS (opportunistic): check bossend frames for the boss HP-BAR max value (shown when targeted) and damage numbers. If a real value is CLEARLY readable, you MAY set MaxHitPoints / projectile Damage toward it and stamp `<!-- DOCUMENTED-from-frames -->`; else leave the designed placeholder. DO NOT guess stats.
  (e) SPRITES: OUT OF SCOPE. Do not attempt.
  HONESTY: DOCUMENTED-from-frames vs RECONSTRUCTED must be accurate. Conservative beats confident — never invent a pattern/loot/stat you cannot actually read; mark unverifiable instead.

STEP 3: Compile-verify in a BACKGROUND shell: `export PATH="$HOME/.dotnet:$PATH"; dotnet build server/wServer/wServer.csproj -c Debug --nologo -v q` (expect 0 errors). If you edited server/common/resources/xml/OrdinaryClient_Bosses.xml (e.g. MaxHitPoints), also `cp` it to client/src/kabam/rotmg/assets/EmbeddedData_OC_BossesCXML.dat. Run dup-key check `grep -rhoE '\.Init\("[^"]+"' server/wServer/logic/db/*.cs | sort | uniq -d` (MUST be empty). If build fails or a dup appears, REVERT and record the failure.

STEP 4: Update docs/mechanics_recovery.json for that boss (set attacks_status / map_status / loot_status / stats_status + a short note with the frame refs). Append one line to docs/build_progress.md: "[<time>] MECH <boss>: attacks <documented/corrected/unverifiable>, map <documented/none>, loot <N observed>, stats <documented/designed> — server <e> errors (MECH LOOP)". If you CORRECTED attack params, note old->new for the audit trail.

STEP 5: STOP. One boss per fire. Do NOT touch the recovery/dev/taunt-verify/bulk-prep loops. Keep work small so the next fire handles the next boss.

---

## Ledger
`docs/mechanics_recovery.json` — per-boss {attacks_status, map_status, loot_status, stats_status, bossends_tags, minimap_tags}. Regenerate (preserving verdicts) with `python3 scripts/mechanics_recovery_ledger.py`.

Statuses: attacks = documented | partial | reconstructed | no-footage | unverifiable-illegible.
map = documented | undocumented | no-footage. loot = designed | observed | none-observed.
stats = designed-placeholder | documented. sprite = placeholder-unrecoverable-from-video (fixed).
