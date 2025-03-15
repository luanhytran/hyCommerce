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
        return await _productService.GetProducts(productParams);
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        return await _productService.GetProduct(id);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var newProduct = await _productService.CreateProduct(product);

        return CreatedAtRoute("GetProduct", new { id = newProduct?.Id }, newProduct);
    }
}