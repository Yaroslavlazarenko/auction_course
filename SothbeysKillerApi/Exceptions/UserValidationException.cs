namespace SothbeysKillerApi.Exceptions;

public class UserValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }
    public UserValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
