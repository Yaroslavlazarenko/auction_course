using System.Data;
using Dapper;
using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository;

public class DbUserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public DbUserRepository(IDbConnection connection)
    {
        _dbConnection = connection;
    }

    public User? GetById(Guid id)
    {
        var query = "select * from users where id = @Id;";
        
        var user = _dbConnection.QuerySingleOrDefault<User>(query, new { Id = id });
        
        return user;
    }

    public User? GetByEmail(string email)
    {
        var query = "select * from users where email = @Email;";
        
        var user = _dbConnection.QuerySingleOrDefault<User>(query, new { Email = email });
        
        return user;
    }

    public User Create(User entity)
    {
        var command = @"insert into users (id, name, email, password) values (@Id, @Name, @Email, @Password) returning *;";
        
        var user = _dbConnection.QueryFirst<User>(command, entity);
        
        return user;
    }

    public User Update(User entity)
    {
        var updateCommand = "update users set name = @Name, email = @Email, password = @Password where id = @Id returning *;";
        
        var user = _dbConnection.QueryFirstOrDefault<User>(updateCommand, entity);
        
        return user!;
    }
}
