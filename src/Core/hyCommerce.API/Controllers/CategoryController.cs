using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hyCommerce.API.Controllers;

public class CategoryController(ICategoryRepository categoryRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Category>> GetCategories()
    {
        var categories = await categoryRepository.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        return Ok(category);
    }
}