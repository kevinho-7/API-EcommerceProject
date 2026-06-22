using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AdminService _admService;

    public AdminController(AdminService adminService)
    {
        _admService = adminService;
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

}