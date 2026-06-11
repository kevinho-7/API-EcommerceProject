using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
    public async Task<Product> AddProductAsync(Product product)
    {
        var validation = _validator.Validate(product);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        //product.id = Guid.NewGuid();
        await _context.products.AddAsync(product);
        await _context.SaveChangesAsync();
        
        return product;
    } 

    // PUT (update) product
    public async Task<Product> UpdateProductAsync(Guid product_id, Product newProduct)
    {
        
        var product = await _context.products.FindAsync(product_id);
        var validation = _validator.Validate(newProduct);

        if(product == null)
        {
            throw new NotFoundException("Produto não encontrado");
        }
        else if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        product.title = newProduct.title;
        product.description = newProduct.description;
        product.price = newProduct.price;
        product.image_path = newProduct.image_path;
        product.quantity = newProduct.quantity;

        await _context.SaveChangesAsync();

        return product;            
    }

    // DELETE product
    public async Task<Guid> DeleteProductAsync(Guid product_id)
    {
        var product = await _context.products.FindAsync(product_id);

        if(product == null)
        {
            throw new NotFoundException("Produto não encontrado");
        }

        _context.products.Remove(product);
        await _context.SaveChangesAsync();
        
        return product_id;
    }
}