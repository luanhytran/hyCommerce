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
            return Ok(result.Value);

        return BadRequest(result.Error.Description);
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var result = await productService.GetProduct(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error.Description);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var result = await productService.CreateProduct(product);

        if (result.IsSuccess)
            return CreatedAtRoute("GetProduct", new { id = result.Value?.Id }, result.Value);

        return BadRequest(result.Error.Description);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await productService.DeleteProduct(id);

        if (result.IsSuccess)
            return NoContent();

        var errorMessage = result.Error.Description;

        return errorMessage?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
            ? NotFound(errorMessage)
            : BadRequest(errorMessage);
    }
}
