namespace SothbeysKillerApi.Exceptions;

public class LotValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }
    public LotValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
