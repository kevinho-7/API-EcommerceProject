using API.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Net.Http.Headers;

namespace API.Services;

public class CustomerService
{
    private readonly ApiDbContext _context;
    private readonly RegisterCustomerValidator _regCustomerValidator;
    private readonly ReqLoginValidation _reqloginValidator;
    private readonly JwtService _jwtService;

    public CustomerService(ApiDbContext context, RegisterCustomerValidator regCustomerValidator, ReqLoginValidation reqloginValidator, JwtService jwtService)
    {
        _context = context;
        _regCustomerValidator = regCustomerValidator;
        _reqloginValidator = reqloginValidator;
        _jwtService = jwtService;

    }

    // GET all Customers
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.customers
            .ToListAsync();
    }

    // GET Customer by Id
    public async Task<Customer?> GetCustomerAsync(string customer_id)
    {
        return await _context.customers
            .FirstOrDefaultAsync(c => c.id == Guid.Parse(customer_id));
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
            throw new ConflictException("Invalid Email or Password");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(register.password_hash);

        register.password_hash = passwordHash;
        register.role = Role.Customer;

        await _context.customers.AddAsync(register);
        await _context.SaveChangesAsync();

        return register;
    }

    // POST (login) "Customer"
    public async Task<string> LoginAsync(JwtClaimsData req)
    {
        var validation = _reqloginValidator.Validate(req);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        var emailExists = await _context.customers
            .AnyAsync(l => l.email == req.email);

        var customerPassword = await _context.customers
            .Where(l => l.email == req.email)
            .Select(l => l.password_hash)
            .FirstOrDefaultAsync();


        if (!emailExists || customerPassword == null)
        {
            throw new ConflictException("Invalid Email or Password");
        }

        bool IsValidPassword = BCrypt.Net.BCrypt.Verify(
            req.password,
            customerPassword
        );

        if (!IsValidPassword)
        {
            throw new ConflictException("Incorrect Password");
        }

        var customerId = await _context.customers
            .Where(c => c.email == req.email)
            .Select(c => c.id)
            .FirstOrDefaultAsync();

        var customerEmail = await _context.customers
            .Where(c => c.email == req.email)
            .Select(c => c.email)
            .FirstOrDefaultAsync();

        var customerRole = await _context.customers
            .Where(c => c.email == req.email)
            .Select(c => c.role)
            .FirstOrDefaultAsync();

        var jwtClaimsData = new JwtClaimsData
        {
            id = customerId,
            email = customerEmail,
            role = customerRole
        };

        string token = _jwtService.GenerateCustomerToken(jwtClaimsData);

        return token;
    }
}