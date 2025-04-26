namespace SothbeysKillerApi.Exceptions;

public class BidValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }
    public BidValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
