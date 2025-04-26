namespace SothbeysKillerApi.Entities
{
    public class Bid
    {
        public Guid Id { get; set; }
        public Guid LotId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
    }
}
