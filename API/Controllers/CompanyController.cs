using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
    private readonly CompanyService _companyService;

    public CompanyController(CompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Company>>> GetAll()
    {
        try
        {
            var returnedCompanies = await _companyService.GetCompaniesAsync();
            return Ok(returnedCompanies);
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}