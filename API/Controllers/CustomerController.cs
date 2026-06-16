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
    [HttpPost("post")]
    public async Task<ActionResult<Customer>> RegisterCustomer(Customer customer)
    {
        var registerCustomer = await _customerService.RegisterCustomerAsync(customer);
        return Ok(new
        {
           message = "Registro feito com sucesso",
           customer = registerCustomer 
        });
    }

}