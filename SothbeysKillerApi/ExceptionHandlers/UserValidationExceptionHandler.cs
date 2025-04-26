using Microsoft.AspNetCore.Diagnostics;
using SothbeysKillerApi.Exceptions;

namespace SothbeysKillerApi.ExceptionHandlers;

public class UserValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is UserValidationException ex)
        {
            httpContext.Response.StatusCode = 400;

            await httpContext.Response.WriteAsJsonAsync(ex.Errors, cancellationToken);
            
            return true;
        }

        return false;
    }
}
