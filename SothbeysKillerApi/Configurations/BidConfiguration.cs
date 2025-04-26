using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Configurations
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(b => b.Created).IsRequired().HasColumnType("timestamp with time zone");
            builder.HasOne<Lot>()
                   .WithMany()
                   .HasForeignKey(b => b.LotId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
