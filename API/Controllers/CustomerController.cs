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

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET all customer
    [Authorize(Roles = "Admin")]
    [HttpGet("get")]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        var returnedUsers = await _customerService.GetCustomersAsync();
        return Ok(returnedUsers);
    }

    // GET customer by Id
    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<Customer>> GetCustomerById()
    {
        var customerId = User.FindFirst("id")?.Value;
        var res = await _customerService.GetCustomerAsync(customerId!);

        return Ok(new
        {
            success = true,
            customer = res
        });
    }

    // POST (register) "Customer"
    [HttpPost("post/register")]
    public async Task<ActionResult<Customer>> RegisterCustomer(Customer register)
    {
        var registerCustomer = await _customerService.RegisterCustomerAsync(register);
        return Ok(new
        {
           message = "Registro feito com sucesso",
           customer = registerCustomer 
        });
    }

    // POST (login) "Customer"
    [HttpPost("auth/login")]
    public async Task<ActionResult<JwtClaimsData>> LoginCustomer(JwtClaimsData req)
    {
        var token = await _customerService.LoginAsync(req);

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