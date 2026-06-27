using System.Linq;
using common.resources;
using wServer.networking;
using wServer.realm.entities;
using wServer.realm.terrain;

namespace wServer.realm.worlds.logic
{
    class Nexus : World
    {
        public Nexus(ProtoWorld proto, Client client = null) : base(proto)
        {
        }

        protected override void Init()
        {
            base.Init();

            var monitor = Manager.Monitor;
            foreach (var i in Manager.Worlds.Values)
            {
                if (i is Realm)
                {
                    monitor.AddPortal(i.Id);
                    continue;
                }

                if (i.Id >= 0)
                    continue;
            }

            // ROTF dungeon portals — drop them onto spread-out Nexus spawn tiles so
            // players can walk into the ROTF dungeons without an admin /spawn.
            var spawns = GetSpawnPoints();
            if (spawns.Length > 0)
            {
                // all ROTF dungeon portals: Illusion, Craig's Castle, Limon's Lair, Deadwater Docks,
                // Esben's Lair, Slime God Den, Bone Dungeon, Chicken Coop, Ruthven's Castle, Bunny Hollow,
                // Asgard, Gate to the Underworld, Tomb of Decaying Death, Riverside Refuge, Flaming Hearth, Twilight Necropolis,
                // Collapsed Woods, Shrouded Sanctum, Sacrificial Grounds (the 6-world Medium tier is now complete),
                // Anubi's Lair (Hard; Anubis + destructible Pillars ward — Ra-pattern); Icy Peaks (0x8214);
                // Starforce (0x8215; tech/space zone — Zuck + Gem Gem + Cracked Core + Ortar)
                ushort[] rotfPortals = { 0x8200, 0x8201, 0x8202, 0x8203, 0x8204, 0x8205, 0x8206, 0x8207, 0x8208, 0x8209, 0x820a, 0x820b, 0x820c, 0x820d, 0x820e, 0x820f, 0x8210, 0x8211, 0x8212, 0x8213, 0x8214, 0x8215 };
                for (var k = 0; k < rotfPortals.Length; k++)
                {
                    var pt = spawns[(k * 3 + 1) % spawns.Length].Key;
                    var portal = Entity.Resolve(Manager, rotfPortals[k]);
                    portal.Move(pt.X + 0.5f, pt.Y + 0.5f);
                    EnterWorld(portal);
                }
            }
        }
    }
}
