using System.Security.Claims;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/customer")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly ICurrentUserService _currentUserService; 

    public CustomerController(CustomerService customerService, ICurrentUserService currentUserService)
    {
        _customerService = customerService;
        _currentUserService = currentUserService;
    }

    // GET customer by Id
    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<Customer>> GetCustomerById()
    {
        var customerId = _currentUserService.GetUserId();
        var res = await _customerService.GetByIdAsync(customerId!);

        return Ok(new
        {
            success = true,
            customer = res
        });
    }

    // POST (register) "Customer"
    [HttpPost("register")]
    public async Task<ActionResult<Customer>> RegisterCustomer(RegisterDto dto)
    {
        var newCustomer = await _customerService.RegisterAsync(dto);
        return Ok(new
        {
           success = true,
           customer = newCustomer 
        });
    }

    // POST (login) "Customer"
    [HttpPost("auth/login")]
    public async Task<ActionResult<LoginDto>> LoginCustomer(LoginDto dto)
    {
        var token = await _customerService.LoginAsync(dto);

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

    // POST (logout) Customer
    [HttpPost("auth/logout")]
    public async Task<ActionResult> LogoutCustomer()
    {
        Response.Cookies.Delete("jwt");

        return Ok();
    }

}