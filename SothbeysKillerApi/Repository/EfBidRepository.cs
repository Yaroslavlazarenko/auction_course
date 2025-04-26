using SothbeysKillerApi.Contexts;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository
{
    public class EfBidRepository : IBidRepository
    {
        private readonly AuctionDbContext _context;
        public EfBidRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Bid> GetByLotId(Guid lotId)
        {
            return _context.Bids.Where(b => b.LotId == lotId).OrderByDescending(b => b.Created).ToList();
        }

        public Bid? GetById(Guid id)
        {
            return _context.Bids.Find(id);
        }

        public Bid Create(Bid entity)
        {
            _context.Bids.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Bid Update(Bid entity)
        {
            var bid = _context.Bids.Find(entity.Id);
            if (bid == null) return null;
            bid.Amount = entity.Amount;
            bid.Created = entity.Created;
            _context.SaveChanges();
            return bid;
        }

        public void Delete(Guid id)
        {
            var bid = _context.Bids.Find(id);
            if (bid != null)
            {
                _context.Bids.Remove(bid);
                _context.SaveChanges();
            }
        }
    }
}
