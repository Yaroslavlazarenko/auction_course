using SothbeysKillerApi.Contexts;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository
{
    public class EfAuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext _context;
        public EfAuctionRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Auction> GetPast()
        {
            return _context.Auctions.Where(a => a.Finish < DateTime.UtcNow).OrderByDescending(a => a.Start).ToList();
        }

        public IEnumerable<Auction> GetActive()
        {
            return _context.Auctions.Where(a => a.Start < DateTime.UtcNow && a.Finish > DateTime.UtcNow).OrderByDescending(a => a.Start).ToList();
        }

        public IEnumerable<Auction> GetFuture()
        {
            return _context.Auctions.Where(a => a.Start > DateTime.UtcNow).OrderByDescending(a => a.Start).ToList();
        }

        public Auction? GetById(Guid id)
        {
            return _context.Auctions.Find(id);
        }

        public Auction Create(Auction entity)
        {
            _context.Auctions.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Auction? Update(Auction entity)
        {
            var auction = _context.Auctions.Find(entity.Id);
            if (auction == null) return null;
            auction.Title = entity.Title;
            auction.Start = entity.Start;
            auction.Finish = entity.Finish;
            _context.SaveChanges();
            return auction;
        }

        public void Delete(Guid id)
        {
            var auction = _context.Auctions.Find(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                _context.SaveChanges();
            }
        }
    }
}
