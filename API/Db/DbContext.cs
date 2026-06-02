using API.Models;
using Microsoft.EntityFrameworkCore;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<User> users {get; set;}
    public DbSet<Product> products {get; set;}
}