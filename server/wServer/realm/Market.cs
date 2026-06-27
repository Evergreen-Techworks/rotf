using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace wServer.realm
{
    // ROTF item Marketplace (Phase: economy). A consignment market: sellers escrow an item +
    // a Fame price; buyers pay Fame -> get the item, seller's account is credited Fame.
    // MVP: in-memory (offers clear on restart — escrowed items are returned to live sellers via
    // /market cancel; a persistence pass is TODO). Recovered from ROTF's web Auction House.
    public class MarketListing
    {
        public int Id;
        public int SellerAccountId;
        public string SellerName;
        public ushort ItemType;
        public int Price; // Fame
    }

    public static class Market
    {
        static readonly Dictionary<int, MarketListing> _listings = new Dictionary<int, MarketListing>();
        static readonly object _lock = new object();
        static int _nextId;

        public static MarketListing Add(int sellerAccountId, string sellerName, ushort itemType, int price)
        {
            var l = new MarketListing
            {
                Id = Interlocked.Increment(ref _nextId),
                SellerAccountId = sellerAccountId,
                SellerName = sellerName,
                ItemType = itemType,
                Price = price
            };
            lock (_lock) _listings[l.Id] = l;
            return l;
        }

        public static bool TryGet(int id, out MarketListing listing)
        {
            lock (_lock) return _listings.TryGetValue(id, out listing);
        }

        public static MarketListing Remove(int id)
        {
            lock (_lock)
            {
                _listings.TryGetValue(id, out var l);
                _listings.Remove(id);
                return l;
            }
        }

        public static List<MarketListing> Recent(int max = 15)
        {
            lock (_lock) return _listings.Values.OrderByDescending(x => x.Id).Take(max).ToList();
        }

        public static List<MarketListing> By(int sellerAccountId)
        {
            lock (_lock) return _listings.Values.Where(x => x.SellerAccountId == sellerAccountId).ToList();
        }

        public static int Count { get { lock (_lock) return _listings.Count; } }
    }
}
