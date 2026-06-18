public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        // Exception Not found 404 error
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = 404;

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = ex.Message
            });
        }
        // Exception Conflicts 409 error
        catch(ConflictException ex)
        {
            context.Response.StatusCode = 409;

            await context.Response.WriteAsJsonAsync(new
            {
               success = false,
               message = ex.Message 
            });
        }
        // Exception Model Validation error
        catch(ValidationException ex)
        {
            var errorMessage = ex.ValidationResult.Errors.Select(x => x.ErrorMessage);
            var errorProperty = ex.ValidationResult.Errors.Select(x => x.PropertyName);
            var errorCause = ex.ValidationResult.Errors.Select(x => x.AttemptedValue);

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = errorMessage,
                property = errorProperty,
                cause = errorCause
            });
        }
        catch(Exception ex)
        {
            context.Response.StatusCode = 500;
            
            await context.Response.WriteAsJsonAsync(new
            {
               success = false,
               message = $"Internal server error. {ex.Message}",
               error = ex.StackTrace
            });
        }
    }
}