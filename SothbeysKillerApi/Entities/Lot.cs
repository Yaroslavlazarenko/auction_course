namespace SothbeysKillerApi.Entities
{
    public class Lot
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal StartPrice { get; set; }
        public decimal PriceStep { get; set; }
    }
}
