using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;
using eCommerceAPI.Core.Services.Interfaces;
using eCommerceAPI.Infrastructures.Repositories;

namespace eCommerceAPI.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetProducts(ProductParams productParams)
    {
        return await _productRepository.GetProducts(productParams);
    }

    public async Task<Product> GetProduct(int id)
    {
        return await _productRepository.GetProduct(id);
    }

    public async Task<Product> CreateProduct(Product product)
    {
        return await _productRepository.CreateProduct(product);
    }
}