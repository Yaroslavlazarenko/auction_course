using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository;

public class InMemoryLotRepository : ILotRepository
{
    public static List<Lot> lotsStorage = [];

    public IEnumerable<Lot> GetByAuctionId(Guid auctionId)
    {
        return lotsStorage.Where(lot => lot.AuctionId == auctionId);
    }

    public Lot? GetById(Guid id)
    {
        return lotsStorage.FirstOrDefault(lot => lot.Id == id);
    }

    public Lot Create(Lot entity)
    {
        lotsStorage.Add(entity);
        
        return entity;
    }

    public Lot Update(Lot entity)
    {
        var index = lotsStorage.FindIndex(lot => lot.Id == entity.Id);
        
        lotsStorage[index] = entity;
        
        return entity;
    }

    public void Delete(Guid id)
    {
        var lot = GetById(id);
        
        lotsStorage.Remove(lot!);
    }
}