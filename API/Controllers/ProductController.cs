using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productsService)
    {
        _productService = productsService;
    }

    // GET all products
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        var result = await _productService.GetProductsAsync();
        return Ok(result);
    }

    // GET product by product_id
    [HttpGet("{product_id}")]
    public async Task<ActionResult<Product>> GetById(Guid product_id)
    {
        var result = await _productService.GetProductAsync(product_id);
        
        return Ok(result);
    }

    // GET products by company_id
    [HttpGet("company/{company_id}")]
    public async Task<ActionResult<List<Product>>> GetProductsByCompanyId(Guid company_id)
    {
        var result = await _productService.GetProductsByCompanyIdAsync(company_id);
        return Ok(result);
    }    

    // GET product by company_id
    [HttpGet("company/{company_id}/{product_id}")]
    public async Task<ActionResult<Product>> GetProductByCompanyId(Guid company_id, Guid product_id)
    {
        var result = await _productService.GetProductByCompanyIdAsync(company_id, product_id);
        return Ok(result);
    }
    
    // POST new product
    [HttpPost("add")]
    public async Task<ActionResult<Guid>> CreateProduct(Product product)
    {
        
        var newProductId =  await _productService.CreateProductAsync(product);
        return Ok(new
        {
            message = $"Produto adicionado com sucesso",
            product_id = newProductId
        });
    }

}
