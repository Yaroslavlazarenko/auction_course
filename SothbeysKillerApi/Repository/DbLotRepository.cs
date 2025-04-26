using System.Data;
using Dapper;
using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository;

public class DbLotRepository : ILotRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _transaction;

    public DbLotRepository(IDbConnection connection, IDbTransaction transaction)
    {
        _dbConnection = connection;
        _transaction = transaction;
    }

    public IEnumerable<Lot> GetByAuctionId(Guid auctionId)
    {
        var query = @"select * from lots where auctionid = @AuctionId order by title;";
        
        return _dbConnection.Query<Lot>(query, new { AuctionId = auctionId }, transaction: _transaction);
    }

    public Lot? GetById(Guid id)
    {
        var query = "select * from lots where id = @Id;";
        
        return _dbConnection.QuerySingleOrDefault<Lot>(query, new { Id = id }, transaction: _transaction);
    }

    public Lot Create(Lot entity)
    {
        var command = @"insert into lots (id, auctionid, title, description, startprice, pricestep) values (@Id, @AuctionId, @Title, @Description, @StartPrice, @PriceStep) returning *;";
        
        var lot = _dbConnection.QueryFirst<Lot>(command, entity, transaction: _transaction);
        
        return lot;
    }

    public Lot? Update(Lot entity)
    {
        var updateCommand = "update lots set title = @Title, description = @Description, startprice = @StartPrice, pricestep = @PriceStep where id = @Id returning *;";
        
        var lot = _dbConnection.QueryFirstOrDefault<Lot>(updateCommand, entity, transaction: _transaction);
        
        return lot;
    }

    public void Delete(Guid id)
    {
        var deleteCommand = "delete from lots where id = @Id;";
        
        _dbConnection.ExecuteScalar(deleteCommand, new { Id = id }, transaction: _transaction);
    }
}