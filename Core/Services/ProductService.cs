using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Common;
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

    public async Task<Result<List<Product>>> GetProducts(ProductParams productParams)
    {
        var products = await _productRepository.GetProducts(productParams);
        
        return products.Count > 0 ? 
            Result<List<Product>>.Success(products) : 
            Result<List<Product>>.Failure("Get products failed");
    }

    public async Task<Result<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetProduct(id);

        return product != null ? 
            Result<Product>.Success(product) : 
            Result<Product>.Failure("Get product failed");
    }

    public async Task<Result<Product>> CreateProduct(Product product)
    {
        var createdProduct = await _productRepository.CreateProduct(product);
        
        return createdProduct != null ? 
            Result<Product>.Success(createdProduct) : 
            Result<Product>.Failure("Create product failed");
    }
}