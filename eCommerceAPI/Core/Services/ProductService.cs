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
        try
        {
            var products = await _productRepository.GetProducts(productParams);

            return products.Count != 0
                ? Result<List<Product>>.Success(products)
                : Result<List<Product>>.Failure("No products found");
        }
        catch (Exception ex)
        {
            return Result<List<Product>>.Failure($"Error retrieving products: {ex.Message}");
        }
    }

    public async Task<Result<Product>> GetProduct(int id)
    {
        try
        {
            var product = await _productRepository.GetProduct(id);

            return product != null ? 
                Result<Product>.Success(product) : 
                Result<Product>.Failure("No product found");
        }
        catch(Exception ex)
        {
            return Result<Product>.Failure($"Error retrieving product: {ex.Message}");
        }
    }

    public async Task<Result<Product>> CreateProduct(Product product)
    {
        try
        {
            var createdProduct = await _productRepository.CreateProduct(product);

            return createdProduct != null
                ? Result<Product>.Success(createdProduct)
                : Result<Product>.Failure("Failed to create product. Please ensure all required fields are provided.");
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure($"Error creating product: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteProduct(int id)
    {
        var product = await _productRepository.GetProduct(id);

        if (product == null) return Result<bool>.Failure("Product not found.");

        var result = await _productRepository.DeleteProduct(id);

        return result ? Result<bool>.Success(result) : Result<bool>.Failure("Failed to delete product.");
    }
}