using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Contexts
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
