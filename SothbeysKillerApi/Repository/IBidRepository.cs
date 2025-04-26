using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository;

public interface IBidRepository
{
    IEnumerable<Bid> GetByLotId(Guid lotId);
    Bid? GetById(Guid id);
    Bid Create(Bid entity);
    Bid Update(Bid entity);
    void Delete(Guid id);
}
