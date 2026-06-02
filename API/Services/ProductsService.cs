using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ProductsService
{
    private readonly ApiDbContext _context;

    public ProductsService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.products
            .FirstOrDefaultAsync(p => p.id == id);
    }
}