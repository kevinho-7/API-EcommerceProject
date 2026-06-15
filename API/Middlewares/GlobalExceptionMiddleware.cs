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
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = 404;

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = ex.Message
            });
        }
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
    }
}