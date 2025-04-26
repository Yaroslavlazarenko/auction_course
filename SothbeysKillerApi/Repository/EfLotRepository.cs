using SothbeysKillerApi.Contexts;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository
{
    public class EfLotRepository : ILotRepository
    {
        private readonly AuctionDbContext _context;
        public EfLotRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Lot> GetByAuctionId(Guid auctionId)
        {
            return _context.Lots.Where(l => l.AuctionId == auctionId).OrderBy(l => l.Title).ToList();
        }

        public Lot? GetById(Guid id)
        {
            return _context.Lots.Find(id);
        }

        public Lot Create(Lot entity)
        {
            _context.Lots.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Lot Update(Lot entity)
        {
            var lot = _context.Lots.Find(entity.Id);
            if (lot == null) return null;
            lot.Title = entity.Title;
            lot.Description = entity.Description;
            lot.StartPrice = entity.StartPrice;
            lot.PriceStep = entity.PriceStep;
            lot.AuctionId = entity.AuctionId;
            _context.SaveChanges();
            return lot;
        }

        public void Delete(Guid id)
        {
            var lot = _context.Lots.Find(id);
            if (lot != null)
            {
                _context.Lots.Remove(lot);
                _context.SaveChanges();
            }
        }
    }
}
