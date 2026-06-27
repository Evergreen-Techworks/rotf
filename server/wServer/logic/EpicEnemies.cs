using System;
using System.Collections.Generic;
using common.resources;
using wServer.logic.loot;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic
{
    /// <summary>
    /// ROTF "Epic Enemies" (Phase D1). A small fraction of eligible enemies spawn as an
    /// elite "Epic" variant: scaled-up HP + size, a renamed "Epic &lt;name&gt;" title, and a
    /// bonus loot roll on death. Recovered from video: e.g. "Epic Mysterious Card",
    /// "Epic Riverborn" roam and are deadly, with their own (boss-specific) taunts and loot.
    /// This implements the generic SYSTEM; per-boss epic taunts live in each boss's behavior.
    /// </summary>
    public static class EpicEnemies
    {
        static readonly Random Rand = new Random();

        // --- tunables ---
        public const double SpawnChance = 0.04;   // 4% of eligible enemies become epic
        public const double HpMult      = 8.0;    // epic HP multiplier
        public const double SizeMult    = 1.4;    // epic size multiplier
        public const int    MinHp       = 700;    // exclude fodder/minions
        public const int    MaxHp       = 150000; // exclude already-huge bosses (no "Epic Oryx")

        // bonus loot rolled IN ADDITION to the enemy's normal drops when an epic dies
        static Loot _bonusLoot;
        static Loot BonusLoot => _bonusLoot ??= new Loot(
            new TierLoot(8,  ItemType.Weapon,  0.40),
            new TierLoot(12, ItemType.Armor,   0.40),
            new TierLoot(5,  ItemType.Ring,    0.40),
            new TierLoot(6,  ItemType.Ability, 0.40),
            new ItemLoot("Potion of Life", 0.12),
            new ItemLoot("Potion of Mana", 0.12)
        );

        public static bool Eligible(Enemy e)
        {
            var d = e.ObjectDesc;
            return d != null && d.Enemy && !d.Static && !e.Spawned && !e.isPet
                   && d.MaxHP >= MinHp && d.MaxHP <= MaxHp;
        }

        /// <summary>Roll the epic chance for a freshly-spawned enemy; upgrade it if it wins.</summary>
        public static void TryMakeEpic(Enemy e)
        {
            if (e == null || e.IsEpic || !Eligible(e))
                return;
            if (Rand.NextDouble() >= SpawnChance)
                return;
            MakeEpic(e);
        }

        public static void MakeEpic(Enemy e)
        {
            e.IsEpic = true;
            e.MaximumHP = (int)(e.MaximumHP * HpMult);
            e.HP = e.MaximumHP;
            e.Size = (int)(e.Size * SizeMult);
            e.Name = "Epic " + e.ObjectDesc.DisplayName;
        }

        /// <summary>Bonus items granted (shared) when an epic enemy dies. Called from Loot.Handle.</summary>
        public static IEnumerable<Item> RollBonus(RealmManager manager)
        {
            return BonusLoot.GetLoots(manager, 2, 4);
        }
    }
}
