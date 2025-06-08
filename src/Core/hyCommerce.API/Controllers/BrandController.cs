using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hyCommerce.API.Controllers;

public class BrandController(IBrandRepository brandRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Brand>> GetBrands()
    {
        var brands = await brandRepository.GetAllAsync();
        return Ok(brands);
    }
    
    [HttpGet("{id:int}", Name = "GetBrand")]
    public async Task<ActionResult<Brand>> GetBrand(int id)
    {
        var brand = await brandRepository.GetByIdAsync(id);
        if (brand == null)
            return NotFound();
        
        return Ok(brand);
    }
}