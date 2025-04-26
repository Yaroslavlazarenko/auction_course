using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository;

public class InMemoryUserRepository : IUserRepository
{
    public static List<User> usersStorage = [];

    public User? GetById(Guid id)
    {
        return usersStorage.FirstOrDefault(u => u.Id == id);
    }

    public User? GetByEmail(string email)
    {
        return usersStorage.FirstOrDefault(u => u.Email == email);
    }

    public User Create(User entity)
    {
        usersStorage.Add(entity);
        
        return entity;
    }

    public User Update(User entity)
    {
        var index = usersStorage.FindIndex(u => u.Id == entity.Id);
        
        usersStorage[index] = entity;
        
        return entity;
    }
}
