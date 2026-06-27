using System.Collections.Generic;
using Newtonsoft.Json;

namespace wServer.realm
{
    /// <summary>
    /// ROTF Runes (Phase D2) — server foundation. Each character has 4 named rune slots
    /// (recovered from the dev blog: Arcane=blue, Chrysalis=green, Impact=orange, Adaptive=clear).
    /// For now a rune is an item ObjectType whose &lt;ActivateOnEquip&gt; stat boosts are applied
    /// passively (mirroring equipment). Reactive rune effects + the Skill/Class tree come later.
    /// Persisted per-character as JSON in DbChar.RuneData.
    /// </summary>
    public class RuneEquipment
    {
        public ushort Arcane;     // item ObjectType, 0 = empty
        public ushort Chrysalis;
        public ushort Impact;
        public ushort Adaptive;

        [JsonIgnore]
        public IEnumerable<ushort> Equipped
        {
            get
            {
                if (Arcane != 0) yield return Arcane;
                if (Chrysalis != 0) yield return Chrysalis;
                if (Impact != 0) yield return Impact;
                if (Adaptive != 0) yield return Adaptive;
            }
        }

        public static RuneEquipment FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "{}")
                return new RuneEquipment();
            try { return JsonConvert.DeserializeObject<RuneEquipment>(json) ?? new RuneEquipment(); }
            catch { return new RuneEquipment(); }
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}
