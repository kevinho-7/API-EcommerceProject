using Microsoft.EntityFrameworkCore;

public class CategoryService
{
    private readonly ApiDbContext _context;

    public CategoryService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _context.categories
            .Include(c => c.Company)
            .ToListAsync();
    }
}