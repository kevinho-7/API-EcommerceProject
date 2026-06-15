using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // GET all users
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var returnedUsers = await _userService.GetUsersAsync();
        return Ok(returnedUsers);
    }

    [HttpPost("register/customer")]
    public async Task<ActionResult<User>> RegisterCustomer(User customer)
    {
        var registerCustomer = await _userService.RegisterCustomerAsync(customer);
        return Ok(new
        {
           message = "Registro feito com sucesso",
           customer = registerCustomer 
        });
    }

}