using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Exceptions;
using SothbeysKillerApi.Entities;
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
        if (string.IsNullOrWhiteSpace(request.Email) || request.Email.Length < 3 || request.Email.Length > 255)
        {
            throw new UserValidationException([new ValidationError("Email", "Email має бути від 3 до 255 символів")]);
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length < 3 || request.Name.Length > 255)
        {
            throw new UserValidationException([new ValidationError("Name", "Ім'я має бути від 3 до 255 символів")]);
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6 || request.Password.Length > 255)
        {
            throw new UserValidationException([new ValidationError("Password", "Пароль має бути не менше 6 символів і не більше 255 символів")
            ]);
        }

        if (_userRepository.GetByEmail(request.Email) != null)
        {
            throw new UserValidationException([new ValidationError("Email", "Користувач з таким email вже існує")]);
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
        if (string.IsNullOrWhiteSpace(request.Email) || request.Email.Length < 3 || request.Email.Length > 255)
        {
            throw new UserValidationException([new ValidationError("Email", "Email має бути від 3 до 255 символів")]);
        }
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6 || request.Password.Length > 255)
        {
            throw new UserValidationException([new ValidationError("Password", "Пароль має бути не менше 6 символів і не більше 255 символів")
            ]);
        }
        
        var user = _userRepository.GetByEmail(request.Email);
        
        if (user == null)
        {
            throw new UserValidationException([new ValidationError("Email", "Користувача не знайдено")]);
        }
        
        if (user.Password != request.Password)
        {
            throw new UserValidationException([new ValidationError("Password", "Невірний пароль")]);
        }
        
        return new LoginUserResponse(user.Id, user.Name, user.Email);
    }
}