using hyCommerce.Application.Services;
using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Entities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace hyCommerce.API.Controllers;

public class ProductController(IProductService productService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts([FromQuery] ProductParams productParams)
    {
        var result = await productService.GetProducts(productParams);

        if (result.IsSuccess) 
            return Ok(result.Data);

        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var result = await productService.GetProduct(id);

        if (result.IsSuccess) 
            return Ok(result.Data);

        return NotFound(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var result = await productService.CreateProduct(product);

        if (result.IsSuccess)
            return CreatedAtRoute("GetProduct", new { id = result.Data?.Id }, result.Data);

        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await productService.DeleteProduct(id);

        if (result.IsSuccess)
            return NoContent();

        return result.ErrorMessage?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
            ? NotFound(result.ErrorMessage)
            : BadRequest(result.ErrorMessage);
    }
}
