using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository;

public class InMemoryAuctionRepository: IAuctionRepository
{
    public static List<Auction> auctionsStorage = [];
    
    public IEnumerable<Auction> GetPast()
    {
        var auctions = auctionsStorage
            .Where(a => a.Finish < DateTime.Now)
            .OrderByDescending(a => a.Start);

        return auctions;
    }

    public IEnumerable<Auction> GetActive()
    {
        var auctions = auctionsStorage
            .Where(a => a.Start < DateTime.Now && a.Finish > DateTime.Now)
            .OrderByDescending(a => a.Start);
        
        return auctions;
    }

    public IEnumerable<Auction> GetFuture()
    {
        var auctions = auctionsStorage
            .Where(a => a.Start > DateTime.Now)
            .OrderByDescending(a => a.Start);
        
        return auctions;
    }

    public Auction? GetById(Guid id)
    {
        var auction = auctionsStorage.FirstOrDefault(a => a.Id == id);
        
        return auction;
    }

    public Auction Create(Auction entity)
    {
        auctionsStorage.Add(entity);

        return entity;
    }

    public Auction? Update(Auction entity)
    {
        var index = auctionsStorage.FindIndex(a => a.Id == entity.Id);
        
        auctionsStorage[index] = entity;
        
        return entity;
    }

    public void Delete(Guid id)
    {
        var auction = GetById(id);
        
        auctionsStorage.Remove(auction!);
    }
}