using System;
using common.resources;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    // ROTF item Marketplace (consignment): /market sell escrows an item for a Fame price;
    // /market buy pays Fame -> item to buyer, Fame credited to the seller's account (works offline).
    // Player-usable (not admin). MVP: in-memory store (see Market.cs).
    class MarketCommand : Command
    {
        public MarketCommand() : base("market") { }

        static string Name(XmlData gd, ushort type)
            => gd.ObjectTypeToId.ContainsKey(type) ? gd.ObjectTypeToId[type] : type.ToString();

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var parts = args.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sub = parts.Length > 0 ? parts[0].ToLowerInvariant() : "list";
            var gd = player.Manager.Resources.GameData;
            var accId = player.Client.Account.AccountId;

            switch (sub)
            {
                case "list":
                {
                    var ls = Market.Recent(15);
                    if (ls.Count == 0) { player.SendInfo("Market is empty. List one: /market sell <slot> <price>"); return true; }
                    player.SendInfo($"-- Market ({Market.Count} listings) --");
                    foreach (var l in ls)
                        player.SendInfo($"#{l.Id}: {Name(gd, l.ItemType)} - {l.Price} Fame  (seller: {l.SellerName})");
                    player.SendInfo("Buy with: /market buy <id>");
                    return true;
                }
                case "mine":
                {
                    var ls = Market.By(accId);
                    if (ls.Count == 0) { player.SendInfo("You have no active listings."); return true; }
                    foreach (var l in ls) player.SendInfo($"#{l.Id}: {Name(gd, l.ItemType)} - {l.Price} Fame");
                    return true;
                }
                case "sell":
                {
                    if (parts.Length < 3 || !int.TryParse(parts[1], out var slot) || !int.TryParse(parts[2], out var price) || price <= 0)
                    { player.SendError("Usage: /market sell <backpack slot 4-19> <price in Fame>"); return false; }
                    if (slot < 4 || slot >= player.Inventory.Length) { player.SendError("Choose a backpack slot (4-19)."); return false; }
                    var item = player.Inventory[slot];
                    if (item == null) { player.SendError("That inventory slot is empty."); return false; }
                    player.Inventory[slot] = null; // escrow into the market
                    var listing = Market.Add(accId, player.Name, item.ObjectType, price);
                    player.SendInfo($"Listed {Name(gd, item.ObjectType)} for {price} Fame (#{listing.Id}). Reclaim: /market cancel {listing.Id}");
                    return true;
                }
                case "buy":
                {
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var id)) { player.SendError("Usage: /market buy <id>"); return false; }
                    if (!Market.TryGet(id, out var l)) { player.SendError("No such listing."); return false; }
                    if (l.SellerAccountId == accId) { player.SendError("That's your own listing (use /market cancel)."); return false; }
                    if (player.CurrentFame < l.Price) { player.SendError($"Not enough Fame ({player.CurrentFame}/{l.Price})."); return false; }
                    if (!gd.Items.ContainsKey(l.ItemType)) { player.SendError("Listing item is missing."); return false; }
                    var item = gd.Items[l.ItemType];
                    var slot = player.Inventory.GetAvailableInventorySlot(item);
                    if (slot == -1) { player.SendError("Your inventory is full."); return false; }
                    if (Market.Remove(id) == null) { player.SendError("Listing was just taken."); return false; }
                    var db = player.Manager.Database;
                    db.UpdateCurrency(accId, -l.Price, CurrencyType.Fame);          // buyer pays
                    db.UpdateCurrency(l.SellerAccountId, l.Price, CurrencyType.Fame); // seller credited (offline-safe)
                    player.CurrentFame -= l.Price;
                    player.Inventory[slot] = item;
                    player.SendInfo($"Bought {Name(gd, l.ItemType)} for {l.Price} Fame from {l.SellerName}.");
                    return true;
                }
                case "cancel":
                {
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var id)) { player.SendError("Usage: /market cancel <id>"); return false; }
                    if (!Market.TryGet(id, out var l)) { player.SendError("No such listing."); return false; }
                    if (l.SellerAccountId != accId) { player.SendError("That isn't your listing."); return false; }
                    if (!gd.Items.ContainsKey(l.ItemType)) { player.SendError("Listing item is missing."); return false; }
                    var item = gd.Items[l.ItemType];
                    var slot = player.Inventory.GetAvailableInventorySlot(item);
                    if (slot == -1) { player.SendError("Inventory full - make room to reclaim."); return false; }
                    Market.Remove(id);
                    player.Inventory[slot] = item;
                    player.SendInfo($"Reclaimed {Name(gd, l.ItemType)} (listing #{id} cancelled).");
                    return true;
                }
                default:
                    player.SendInfo("/market [list | sell <slot> <price> | buy <id> | cancel <id> | mine]");
                    return true;
            }
        }
    }
}
