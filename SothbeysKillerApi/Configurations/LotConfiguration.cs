using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Configurations
{
    public class LotConfiguration : IEntityTypeConfiguration<Lot>
    {
        public void Configure(EntityTypeBuilder<Lot> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Title).IsRequired().HasMaxLength(255);
            builder.Property(l => l.Description).HasMaxLength(255);
            builder.Property(l => l.StartPrice).HasColumnType("decimal(18,2)");
            builder.Property(l => l.PriceStep).HasColumnType("decimal(18,2)");
            builder.HasOne<Auction>()
                   .WithMany()
                   .HasForeignKey(l => l.AuctionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
