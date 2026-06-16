using API.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace API.Services;

public class CustomerService
{
    private readonly ApiDbContext _context;
    private readonly RegisterCustomerValidator _regCustomerValidator;

    public CustomerService(ApiDbContext context, RegisterCustomerValidator regCustomerValidator)
    {
        _context = context;
        _regCustomerValidator = regCustomerValidator;
    }

    // GET all Customers
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.customers
            .Where(u => u.role == Role.Customer)
            .ToListAsync();
    }

    // POST (register) "Customer"
    public async Task<Customer> RegisterCustomerAsync(Customer customer)
    {
        var validation = _regCustomerValidator.Validate(customer);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        var emailExists = await _context.customers.AnyAsync(u => u.email == customer.email);
        if (emailExists)
        {
            throw new ConflictException("Email ja cadastrado");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(customer.password_hash);

        customer.password_hash = passwordHash;
        customer.role = Role.Customer;

        await _context.customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return customer;
    }
}