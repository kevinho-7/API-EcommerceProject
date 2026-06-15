using API.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace API.Services;

public class UserService
{
    private readonly ApiDbContext _context;
    private readonly RegisterCustomerValidator _regCustomerValidator;

    public UserService(ApiDbContext context, RegisterCustomerValidator regCustomerValidator)
    {
        _context = context;
        _regCustomerValidator = regCustomerValidator;
    }

    // GET all users
    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.users.ToListAsync();
    }

    // POST (register) "Customer"
    public async Task<User> RegisterCustomerAsync(User customer)
    {
        var validation = _regCustomerValidator.Validate(customer);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(customer.password_hash);

        customer.password_hash = passwordHash;
        customer.user_role = Role.Customer;

        await _context.users.AddAsync(customer);
        await _context.SaveChangesAsync();

        return customer;
    }
}


//   "auth_provider": "corinthians,
//   "cep": "",
//   "street": "",
//   "neighborhood": "",
//   "complement": "",
//   "city": "",
//   "state": "sp",