using API.Models;
using Microsoft.EntityFrameworkCore;
namespace API.Services;

public class CompanyService
{
    private readonly ApiDbContext _context;

    public CompanyService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<Company>> GetCompaniesAsync()
    {
        return await _context.companies.ToListAsync();
    }
}