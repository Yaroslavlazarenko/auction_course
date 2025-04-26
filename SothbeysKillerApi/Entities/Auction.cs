namespace SothbeysKillerApi.Entities
{
    public class Auction
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }
}
