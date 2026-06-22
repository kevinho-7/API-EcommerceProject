using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminService
{
    private readonly ApiDbContext _context;
    private readonly RegisterValidator _registerValidator;
    private readonly LoginValidation _loginValidator;
    private readonly JwtService _jwtService;

    public AdminService(ApiDbContext context, RegisterValidator registerValidator, LoginValidation loginValidator, JwtService jwtService)
    {
        _context = context;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _jwtService = jwtService;
    }

    // POST (register) Admin
    public async Task<Admin> RegisterAsync(RegisterDto dto)
    {
        var validation = _registerValidator.Validate(dto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        var existEmail = await _context.admins.AnyAsync(a => a.email == dto.email);
        if(existEmail)
        {
            throw new ConflictException("This email address is already registered");
        }

        var password = BCrypt.Net.BCrypt.HashPassword(dto.password);

        var newAdmin = new Admin
        {
            first_name = dto.first_name,
            last_name = dto.last_name,
            email = dto.email,
            password = password
        };

        await _context.admins.AddAsync(newAdmin);
        await _context.SaveChangesAsync();

        return newAdmin;
    }


    // POST (login) Admin
    public async Task<string> LoginAsync(LoginDto dto)
    {
        var validation = _loginValidator.Validate(dto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        var emailExists = await _context.admins
            .AnyAsync(a => a.email == dto.email);

        var password = await _context.admins
            .Where(a => a.email == dto.email)
            .Select(a => a.password)
            .FirstOrDefaultAsync();

        if(!emailExists || password == null)
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

        var id = await _context.admins
            .Where(a => a.email == dto.email)
            .Select(a => a.id)
            .FirstOrDefaultAsync();
        
        var email = await _context.admins
            .Where(a => a.email == dto.email)
            .Select(a => a.email)
            .FirstOrDefaultAsync();

        var role = await _context.admins
            .Where(a => a.email == dto.email)
            .Select(a => a.role)
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