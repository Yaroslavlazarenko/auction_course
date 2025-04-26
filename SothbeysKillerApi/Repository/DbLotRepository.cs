using System.Data;
using Dapper;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository;

public class DbLotRepository : ILotRepository
{
    private readonly IDbConnection _dbConnection;

    public DbLotRepository(IDbConnection connection)
    {
        _dbConnection = connection;
    }

    public IEnumerable<Lot> GetByAuctionId(Guid auctionId)
    {
        var query = @"select * from lots where auctionid = @AuctionId order by title;";
        
        return _dbConnection.Query<Lot>(query, new { AuctionId = auctionId });
    }

    public Lot? GetById(Guid id)
    {
        var query = "select * from lots where id = @Id;";
        
        return _dbConnection.QuerySingleOrDefault<Lot>(query, new { Id = id });
    }

    public Lot Create(Lot entity)
    {
        var command = @"insert into lots (id, auctionid, title, description, startprice, pricestep) values (@Id, @AuctionId, @Title, @Description, @StartPrice, @PriceStep) returning *;";
        
        var lot = _dbConnection.QueryFirst<Lot>(command, entity);
        
        return lot;
    }

    public Lot Update(Lot entity)
    {
        var updateCommand = "update lots set title = @Title, description = @Description, startprice = @StartPrice, pricestep = @PriceStep where id = @Id returning *;";
        
        var lot = _dbConnection.QueryFirstOrDefault<Lot>(updateCommand, entity);
        
        return lot!;
    }

    public void Delete(Guid id)
    {
        var deleteCommand = "delete from lots where id = @Id;";
        
        _dbConnection.ExecuteScalar(deleteCommand, new { Id = id });
    }
}