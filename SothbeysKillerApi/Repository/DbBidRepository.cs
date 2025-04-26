using System.Data;
using Dapper;
using SothbeysKillerApi.Entities;

namespace SothbeysKillerApi.Repository;

public class DbBidRepository : IBidRepository
{
    private readonly IDbConnection _dbConnection;

    public DbBidRepository(IDbConnection connection)
    {
        _dbConnection = connection;
    }

    public IEnumerable<Bid> GetByLotId(Guid lotId)
    {
        var query = "select * from bids where lotid = @LotId order by created desc;";
        
        var bids = _dbConnection.Query<Bid>(query, new { LotId = lotId });
        
        return bids;
    }

    public Bid? GetById(Guid id)
    {
        var query = "select * from bids where id = @Id;";
        
        var bid = _dbConnection.QuerySingleOrDefault<Bid>(query, new { Id = id });
        
        return bid;
    }

    public Bid Create(Bid entity)
    {
        var command = @"insert into bids (id, lotid, userid, amount, created) values (@Id, @LotId, @UserId, @Amount, @Created) returning *;";
        
        var bid = _dbConnection.QueryFirst<Bid>(command, entity);
        
        return bid;
    }

    public Bid Update(Bid entity)
    {
        var updateCommand = "update bids set amount = @Amount, created = @Created where id = @Id returning *;";
        
        var bid = _dbConnection.QueryFirstOrDefault<Bid>(updateCommand, entity);
        
        return bid!;
    }

    public void Delete(Guid id)
    {
        var deleteCommand = "delete from bids where id = @Id;";
        
        _dbConnection.ExecuteScalar(deleteCommand, new { Id = id });
    }
}
