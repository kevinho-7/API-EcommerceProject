using API.Models;
using API.Services;
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

    // GET all users
    [HttpGet("get")]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        var returnedUsers = await _customerService.GetCustomersAsync();
        return Ok(returnedUsers);
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

    //POST (login) "Customer"
    [HttpPost("auth/login")]
    public async Task<ActionResult<Customer>> LoginCustomer(Customer login)
    {
        var loginCustomer = await _customerService.LoginCustomerAsync(login);
        return Ok(new
        {
            message = "deu certo mano",
        });
    }

}