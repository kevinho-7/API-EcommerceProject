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

    // GET All products
        public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.products
            .Include(p => p.Category)
            .Include(p => p.Company)
            .ToListAsync();
    }

    // GET Product by product_id
    public async Task<Product?> GetProductAsync(Guid product_id)
    {
        return await _context.products
            .Include(p => p.Category)
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.id == product_id);
    }

    // GET Products by company_id
    public async Task<List<Product>> GetProductsByCompanyIdAsync(Guid company_id)
    {
        return await _context.products
            .Include(p => p.Category)
            .Include(p => p.Company )
            .Where(p => p.company_id == company_id)
            .ToListAsync();
    }

    // GET product by company_id
    public async Task<Product?> GetProductByCompanyIdAsync(Guid company_id, Guid product_id)
    {
        return await _context.products
            .Include(p => p.Category)
            .Include(p => p.Company)
            .Where(p => p.company_id == company_id)
            .FirstOrDefaultAsync(p => p.id == product_id);

    }

    // POST (create) product
    public async Task<Guid> CreateProductAsync(Product product)
    {
        product.id = Guid.NewGuid();
        await _context.products.AddAsync(product);
        await _context.SaveChangesAsync();
        
        return product.id;
    } 



}