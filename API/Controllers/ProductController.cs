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
    public async Task<ActionResult<Product>> AddProduct(Product product)
    {
        
        var newProduct =  await _productService.AddProductAsync(product);
        return Ok(new
        {
            message = $"Produto adicionado com sucesso",
            product_id = newProduct
        });
    }

    // PUT existing product
    [HttpPut("up/{product_id}")]
    public async Task<ActionResult<Product>> UpdateProduct(Guid product_id, Product product)
    {
        var productUpdated = await _productService.UpdateProductAsync(product_id, product);
        return Ok(new
        {
            message = "Produto alterado com sucesso",
            product = productUpdated
        });
    }

    // DELETE existing product
    [HttpDelete ("del/{product_id}")]
    public async Task<ActionResult<Product>> DeleteProduct(Guid product_id)
    {
        var DeleteProduct = await _productService.DeleteProductAsync(product_id);
        return Ok(new
        {
            message = "Produto removido com sucesso",
            product = DeleteProduct
        });
    }

}
