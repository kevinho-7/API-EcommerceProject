using API.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Net.Http.Headers;

namespace API.Services;

public class CustomerService
{
    private readonly ApiDbContext _context;
    private readonly RegisterCustomerValidator _regCustomerValidator;
    private readonly LoginCustomerValidation _loginCustomerValidator;

    public CustomerService(ApiDbContext context, RegisterCustomerValidator regCustomerValidator, LoginCustomerValidation loginCustomerValidator)
    {
        _context = context;
        _regCustomerValidator = regCustomerValidator;
        _loginCustomerValidator = loginCustomerValidator;

    }

    // GET all Customers
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.customers
            .Where(u => u.role == Role.Customer)
            .ToListAsync();
    }

    // POST (register) "Customer"
    public async Task<Customer> RegisterCustomerAsync(Customer register)
    {
        var validation = _regCustomerValidator.Validate(register);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        var emailExists = await _context.customers.AnyAsync(c => c.email == register.email);
        if (emailExists)
        {
            throw new ConflictException("Email ou Senha invalido");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(register.password_hash);

        register.password_hash = passwordHash;
        register.role = Role.Customer;

        await _context.customers.AddAsync(register);
        await _context.SaveChangesAsync();

        return register;
    }

    // POST (login) "Customer"
    public async Task<Customer> LoginCustomerAsync(Customer login)
    {
        var validation = _loginCustomerValidator.Validate(login);
        var emailExists = await _context.customers.AnyAsync(c => c.email == login.email);
        var customerPassword = await _context.customers
            .Where(c => c.email == login.email)
            .Select(c => c.password_hash)
            .FirstOrDefaultAsync();

        bool IsValidPassword = BCrypt.Net.BCrypt.Verify(
            login.password_hash,
            customerPassword
        );

        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        if (!emailExists || !IsValidPassword)
        {
            throw new ConflictException("Email ou senha invalidos");
        }

        return login;
    }
}