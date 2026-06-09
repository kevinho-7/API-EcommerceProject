using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ProductService
{
    private readonly ApiDbContext _context;
    private readonly ProductValidator _validator;

    public ProductService(ApiDbContext context, ProductValidator validator)
    {
        _context = context;
        _validator = validator;
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
        var product = await _context.products
            .Include(p => p.Category)
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.id == product_id);
        
        if(product == null)
        {
            throw new NotFoundException("Produto não encontrado");
        }

        return product;
    }

    // GET Products by company_id
    public async Task<List<Product>> GetProductsByCompanyIdAsync(Guid company_id)
    {
         
        var product = await _context.products
            .Include(p => p.Category)
            .Include(p => p.Company )
            .Where(p => p.company_id == company_id)
            .ToListAsync();

        if(product == null)
        {
            throw new NotFoundException("Empresa não encontrada");
        }

        return product;
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

    // POST (add) product
    public async Task<Guid> CreateProductAsync(Product product)
    {
        var validation = _validator.Validate(product);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        product.id = Guid.NewGuid();
        await _context.products.AddAsync(product);
        await _context.SaveChangesAsync();
        
        return product.id;
    } 

    // PUT (update) product
    
}