using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetAll()
    {
        try
        {
            var returnedCategories = await _categoryService.GetCategoriesAsync();
            return Ok(returnedCategories);
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}