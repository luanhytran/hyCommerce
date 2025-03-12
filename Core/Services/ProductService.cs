using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;
using eCommerceAPI.Core.Services.Interfaces;
using eCommerceAPI.Infrastructures.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ActionResult<List<Product>>> GetProducts(ProductParams productParams)
    {
        return await _productRepository.GetProducts(productParams);
    }

    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        return await _productRepository.GetProduct(id);
    }
}