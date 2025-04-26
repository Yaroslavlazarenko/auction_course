using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Repository;

namespace SothbeysKillerApi.Services;

public interface IUserService
{
    void RegisterUser(RegisterUserRequest request);
    LoginUserResponse LoginUser(LoginUserRequest request);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public User? GetUserById(Guid id)
    {
        return _userRepository.GetById(id);
    }
    
    public void RegisterUser(RegisterUserRequest request)
    {
        if (request.Email.Length < 3 || request.Email.Length > 255)
        {
            throw new ArgumentException("Email must be between 3 and 255 characters");
        }

        if (request.Name.Length < 3 || request.Name.Length > 255)
        {
            throw new ArgumentException("Name must be between 3 and 255 characters");
        }

        if (request.Password.Length < 3 || request.Password.Length > 255)
        {
            throw new ArgumentException("Password must be between 3 and 255 characters");
        }

        if (_userRepository.GetByEmail(request.Email) != null)
        {
            throw new ArgumentException();
        }
        
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.Name,
            Password = request.Password
        };
        
        _userRepository.Create(user);
    }
    
    public LoginUserResponse LoginUser(LoginUserRequest request)
    {
        if (request.Email.Length < 3 
            || request.Email.Length > 255 
            || request.Password.Length < 3 
            || request.Password.Length > 255)
        {
            throw new ArgumentException();
        }
        
        var user = _userRepository.GetByEmail(request.Email);
        
        if (user == null)
        {
            throw new ArgumentException();
        }
        
        if (user.Password != request.Password)
        {
            throw new UnauthorizedAccessException("Invalid password");
        }
        
        return new LoginUserResponse(user.Id, user.Name, user.Email);
    }
}