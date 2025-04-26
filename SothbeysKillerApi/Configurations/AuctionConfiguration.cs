using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Configurations
{
    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Start).IsRequired().HasColumnType("timestamp with time zone");
            builder.Property(a => a.Finish).IsRequired().HasColumnType("timestamp with time zone");
        }
    }
}
