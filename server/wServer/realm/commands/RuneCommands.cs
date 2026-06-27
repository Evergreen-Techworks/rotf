using System;
using System.Collections.Generic;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    // ROTF Runes (Phase D2) — the BLESSING flow: consume 10 matching Symbols from inventory
    // to grant/equip the rune for that slot. Recovered: "Arcane Symbol x10 consumed, blessing you".
    class BlessCommand : Command
    {
        const int Need = 10;
        public BlessCommand() : base("bless", reqAdmin: true) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            string slot = args.Trim().ToLowerInvariant();
            string symbol, rune;
            switch (slot)
            {
                case "arcane":    symbol = "Arcane Symbol";    rune = "Arcane Rune";    break;
                case "chrysalis": symbol = "Chrysalis Symbol"; rune = "Chrysalis Rune"; break;
                case "impact":    symbol = "Impact Symbol";    rune = "Impact Rune";    break;
                case "adaptive":  symbol = "Adaptive Symbol";  rune = "Adaptive Rune";  break;
                default:
                    player.SendError("Usage: /bless <arcane|chrysalis|impact|adaptive>");
                    return false;
            }

            var gd = player.Manager.Resources.GameData;
            if (!gd.IdToObjectType.TryGetValue(symbol, out var symType) ||
                !gd.IdToObjectType.TryGetValue(rune, out var runeType))
            {
                player.SendError("Rune items not loaded (restart server?).");
                return false;
            }

            // find Symbols in the inventory
            var found = new List<int>();
            for (var i = 0; i < player.Inventory.Length; i++)
            {
                var it = player.Inventory[i];
                if (it != null && it.ObjectType == symType)
                    found.Add(i);
            }
            if (found.Count < Need)
            {
                player.SendError($"Need {Need} {symbol}(s) to bless (have {found.Count}).");
                return false;
            }

            for (var k = 0; k < Need; k++)
                player.Inventory[found[k]] = null; // consume

            switch (slot)
            {
                case "arcane":    player.Runes.Arcane = runeType; break;
                case "chrysalis": player.Runes.Chrysalis = runeType; break;
                case "impact":    player.Runes.Impact = runeType; break;
                case "adaptive":  player.Runes.Adaptive = runeType; break;
            }
            player.Stats.ReCalculateValues();
            player.SendInfo($"Blessing complete! Consumed {Need} {symbol}(s) -> {rune} now empowers your {slot} slot.");
            return true;
        }
    }

    // ROTF Runes (Phase D2) — equip/clear a rune into one of the 4 slots (no client UI yet).
    // Usage: /rune <arcane|chrysalis|impact|adaptive> <rune item name | clear>
    class RuneCommand : Command
    {
        public RuneCommand() : base("rune", reqAdmin: true) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var parts = args.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                player.SendInfo("Usage: /rune <arcane|chrysalis|impact|adaptive> <rune item name | clear>");
                return false;
            }

            var slot = parts[0].Trim().ToLowerInvariant();
            var itemArg = parts[1].Trim();

            ushort type = 0;
            if (!itemArg.Equals("clear", StringComparison.OrdinalIgnoreCase))
            {
                var gd = player.Manager.Resources.GameData;
                if (!gd.IdToObjectType.TryGetValue(itemArg, out type) || !gd.Items.ContainsKey(type))
                {
                    player.SendError($"Unknown rune item: {itemArg}");
                    return false;
                }
            }

            switch (slot)
            {
                case "arcane":    player.Runes.Arcane = type; break;
                case "chrysalis": player.Runes.Chrysalis = type; break;
                case "impact":    player.Runes.Impact = type; break;
                case "adaptive":  player.Runes.Adaptive = type; break;
                default:
                    player.SendError("Slot must be: arcane, chrysalis, impact, or adaptive.");
                    return false;
            }

            player.Stats.ReCalculateValues();
            player.SendInfo(type == 0
                ? $"Cleared {slot} rune."
                : $"Set {slot} rune to '{itemArg}'. Stats recalculated.");
            return true;
        }
    }

    // Quick read-out of currently-slotted runes.
    class RunesCommand : Command
    {
        public RunesCommand() : base("runes", reqAdmin: true) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var gd = player.Manager.Resources.GameData;
            string Name(ushort t) => t != 0 && gd.ObjectTypeToId.ContainsKey(t) ? gd.ObjectTypeToId[t] : "(empty)";
            var r = player.Runes;
            player.SendInfo($"Runes — Arcane: {Name(r.Arcane)} | Chrysalis: {Name(r.Chrysalis)} | Impact: {Name(r.Impact)} | Adaptive: {Name(r.Adaptive)}");
            return true;
        }
    }
}
