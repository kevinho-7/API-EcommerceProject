public interface ICurrentUserService
{
    string? GetUserId();
    string? GetUserEmail();
    string? GetUserRole();
}