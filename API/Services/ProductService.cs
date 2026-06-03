using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ProductService
{
    private readonly ApiDbContext _context;

    public ProductService(ApiDbContext context)
    {
        _context = context;
    }

        public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.products
            .Include(p => p.Category)
            .Include(p => p.Company)
            .ToListAsync();
    }

    public async Task<Product?> GetProductAsync(Guid id)
    {
        return await _context.products
            .FirstOrDefaultAsync(p => p.id == id);
    }
}