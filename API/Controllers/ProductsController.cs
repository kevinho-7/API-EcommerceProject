using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productService;

    public ProductsController(ProductsService productsService)
    {
        _productService = productsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        var returnedProducts = await _productService.GetAllAsync();
        return Ok(returnedProducts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
        var returnedById = await _productService.GetByIdAsync(id);
        return Ok(returnedById);
    }
}