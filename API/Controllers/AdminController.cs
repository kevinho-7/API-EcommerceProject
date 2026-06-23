using API.DTOS;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AdminService _admService;
    private readonly ICurrentUserService _currentUserService; 

    public AdminController(AdminService adminService, ICurrentUserService currentUserService)
    {
        _admService = adminService;
        _currentUserService = currentUserService;
    }

    // Get Admin profile by Id
    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<Admin>> GetById()
    {
        var id = _currentUserService.GetUserId();
        var admin = _admService.GetProfileAsync(id!);

        return Ok(new
        {
            success = true,
            profile = admin
        });
    }

    // POST (register) Admin
    [HttpPost("register")]
    public async Task<ActionResult<Admin>> RegisterAdmin(RegisterDto dto)
    {
        var newAdmin = await _admService.RegisterAsync(dto);

        return Ok(new
        {
           success = true,
           admin = newAdmin 
        });
    }

    // POST Address in Admin Profile
    //[Authorize]
    // [HttpPost("profile/address")]
    // public async

    // POST (login) Admin
    [HttpPost("auth/login")]
    public async Task<ActionResult<LoginDto>> LoginAdmin(LoginDto dto)
    {
        var token = await _admService.LoginAsync(dto);

        Response.Cookies.Append(
            "jwt",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            }
        );

        return Ok();
    }

    // POST (logout) Admin
    [HttpPost("auth/logout")]
    public async Task<ActionResult> LogoutAdmin()
    {
        Response.Cookies.Delete("jwt");

        return Ok();
    }

    // PUT (Update) profile Admin
    [Authorize]
    [HttpPut("update/profile/address")]
    public async Task<ActionResult<Admin>> UpdateProfile(UpdateProfileAddressDto updates)
    {
        var id = _currentUserService.GetUserId();
        var update = await _admService.UpdateProfileAsync(updates, id!);

        return Ok(new
        {
            success = true,
            updates = update
        });
    }

}