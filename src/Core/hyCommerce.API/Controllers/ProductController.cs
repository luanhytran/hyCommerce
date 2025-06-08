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

        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var result = await productService.GetProduct(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var result = await productService.CreateProduct(product);
        
        return CreatedAtRoute("GetProduct", new { id = result?.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        await productService.DeleteProduct(id);

        return NoContent();
    }
}
