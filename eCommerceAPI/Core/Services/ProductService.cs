using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Common;
using eCommerceAPI.Core.Contracts;
using eCommerceAPI.Core.Contracts.Repositories;
using eCommerceAPI.Core.Contracts.Services;
using eCommerceAPI.Core.Models;

namespace eCommerceAPI.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<Product>>> GetProducts(ProductParams productParams)
    {
        try
        {
            var products = await _productRepository.GetProducts(productParams);

            return Result<List<Product>>.Success(products);
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

            return product != null 
                ? Result<Product>.Success(product) 
                : Result<Product>.Failure("No product found");
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

            if (createdProduct == null)
                return Result<Product>.Failure("Failed to create product");

            await _unitOfWork.SaveAsync();

            return Result<Product>.Success(createdProduct);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure($"Error creating product: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteProduct(int id)
    {
        var product = await _productRepository.GetProduct(id);

        if (product == null) 
            return Result<bool>.Failure("Product not found");

        var result = await _productRepository.DeleteProduct(id);

        return result
            ? Result<bool>.Success(result) 
            : Result<bool>.Failure("Failed to delete product");
    }
}