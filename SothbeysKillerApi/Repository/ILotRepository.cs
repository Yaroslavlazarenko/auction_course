using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository;

public interface ILotRepository
{
    IEnumerable<Lot> GetByAuctionId(Guid auctionId);
    Lot? GetById(Guid id);
    Lot Create(Lot entity);
    Lot Update(Lot entity);
    void Delete(Guid id);
}
