public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId()
    {
        return _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst("id")
            ?.Value;
    }

    public string? GetUserEmail()
    {
        return _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst("email")
            ?.Value;
    }

    public string? GetUserRole()
    {
        return _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst("role")
            ?.Value;
    }
}