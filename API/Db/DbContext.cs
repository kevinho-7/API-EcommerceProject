using API.Models;
using Microsoft.EntityFrameworkCore;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {    
    }

    public DbSet<Customer> customers {get; set;}
    public DbSet<Company> companies {get; set;}
    public DbSet<Product> products {get; set;}
    public DbSet<Category> categories {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ForeignKeys for Product Model
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.category_id);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Company)
            .WithMany()
            .HasForeignKey(p => p.company_id);

        // ForeignKeys for Category Model
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Company)
            .WithMany()
            .HasForeignKey(c => c.company_id);

        // Converting user_role column to type String
        modelBuilder.Entity<Customer>()
            .Property(c => c.role)
            .HasConversion<string>();
    }
    
}