using API.Models;
using API.DTOS;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;

namespace API.Services;

public class CustomerService
{
    private readonly ApiDbContext _context;
    private readonly RegisterValidator _registerValidator;
    private readonly LoginValidation _loginValidator;
    private readonly JwtService _jwtService;

    public CustomerService(ApiDbContext context, RegisterValidator registerValidator, LoginValidation loginValidator, JwtService jwtService)
    {
        _context = context;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _jwtService = jwtService;

    }

    // GET Customer by Id
    public async Task<Customer?> GetByIdAsync(string customerId)
    {
        return await _context.customers
            .FirstOrDefaultAsync(c => c.id == Guid.Parse(customerId));
    }
    
    // POST (register) "Customer"
    public async Task<Customer> RegisterAsync(RegisterDto dto)
    {
        var validation = _registerValidator.Validate(dto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        var emailExists = await _context.customers.AnyAsync(c => c.email == dto.email);
        if (emailExists)
        {
            throw new ConflictException("This email address is already registered.");
        }

        var password = BCrypt.Net.BCrypt.HashPassword(dto.password);

        
        Customer newCustomer = new Customer
        {   
            first_name = dto.first_name,
            last_name = dto.last_name,
            email = dto.email,
            password = password,
        };

        await _context.customers.AddAsync(newCustomer);
        await _context.SaveChangesAsync();

        return newCustomer;
    }

    // POST (login) "Customer"
    public async Task<string> LoginAsync(LoginDto dto)
    {
        var validation = _loginValidator.Validate(dto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        var emailExists = await _context.customers
            .AnyAsync(l => l.email == dto.email);

        var password = await _context.customers
            .Where(l => l.email == dto.email)
            .Select(l => l.password)
            .FirstOrDefaultAsync();


        if (!emailExists || password == null)
        {
            throw new ConflictException("Invalid Email or Password");
        }

        bool IsValidPassword = BCrypt.Net.BCrypt.Verify(
            dto.password,
            password
        );

        if (!IsValidPassword)
        {
            throw new ConflictException("Incorrect Password");
        }

        var id = await _context.customers
            .Where(c => c.email == dto.email)
            .Select(c => c.id)
            .FirstOrDefaultAsync();

        var email = await _context.customers
            .Where(c => c.email == dto.email)
            .Select(c => c.email)
            .FirstOrDefaultAsync();

        var role = await _context.customers
            .Where(c => c.email == dto.email)
            .Select(c => c.role)
            .FirstOrDefaultAsync();

        var jwtClaimsData = new JwtClaimsData
        {
            id = id,
            email = email,
            role = role
        };

        string token = _jwtService.GenToken(jwtClaimsData);

        return token;
    }
}