using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class UserService
{
    private readonly ApiDbContext _context;

    public UserService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.users.ToListAsync();
    }
}