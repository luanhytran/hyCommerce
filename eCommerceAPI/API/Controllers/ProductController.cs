using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;
using eCommerceAPI.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.API.Controllers;

public class ProductController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts([FromQuery] ProductParams productParams)
    {
        var result = await _productService.GetProducts(productParams);
        if (result.IsSuccess) return Ok(result.Data);
        return NotFound(result.ErrorMessage);
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var result = await _productService.GetProduct(id);
        if (result.IsSuccess) return Ok(result.Data);
        return NotFound(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var result = await _productService.CreateProduct(product);
        if (result.IsSuccess)
            return CreatedAtRoute("GetProduct", new { id = result?.Data?.Id }, result?.Data);
        return BadRequest(result.ErrorMessage);
    }
}