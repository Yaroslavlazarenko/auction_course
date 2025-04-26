using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository;

public class InMemoryBidRepository : IBidRepository
{
    public static List<Bid> bidsStorage = [];

    public IEnumerable<Bid> GetByLotId(Guid lotId)
    {
        return bidsStorage.Where(bid => bid.LotId == lotId);
    }

    public Bid? GetById(Guid id)
    {
        return bidsStorage.FirstOrDefault(bid => bid.Id == id);
    }

    public Bid Create(Bid entity)
    {
        bidsStorage.Add(entity);
        
        return entity;
    }

    public Bid Update(Bid entity)
    {
        var index = bidsStorage.FindIndex(bid => bid.Id == entity.Id);
        
        bidsStorage[index] = entity;
        
        return entity;
    }

    public void Delete(Guid id)
    {
        var bid = GetById(id);
        
        bidsStorage.Remove(bid!);
    }
}
