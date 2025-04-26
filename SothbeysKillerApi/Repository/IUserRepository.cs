using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository;

public interface IUserRepository
{
    User? GetById(Guid id);
    User? GetByEmail(string email);
    User Create(User entity);
    User Update(User entity);
}
