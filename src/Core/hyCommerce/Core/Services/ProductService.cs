using hyCommerce.API.RequestHelpers;
using hyCommerce.Core.Common;
using hyCommerce.Core.Models;
using hyCommerce.Infrastructures.Persistence;
using hyCommerce.Infrastructures.Persistence.Repositories;

namespace hyCommerce.Core.Services;

public interface IProductService
{
    public Task<Result<List<Product>>> GetProducts(ProductParams productParams);
    public Task<Result<Product>> GetProduct(int id);
    public Task<Result<Product>> CreateProduct(Product product);
    public Task<Result<bool>> DeleteProduct(int id);
}

public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    : IProductService
{
    public async Task<Result<List<Product>>> GetProducts(ProductParams productParams)
    {
        try
        {
            var products = await productRepository.GetProducts(productParams);

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
            var product = await productRepository.GetProduct(id);

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
            var createdProduct = await productRepository.CreateProduct(product);

            if (createdProduct == null)
                return Result<Product>.Failure("Failed to create product");

            await unitOfWork.SaveAsync();

            return Result<Product>.Success(createdProduct);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure($"Error creating product: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteProduct(int id)
    {
        var product = await productRepository.GetProduct(id);

        if (product == null) 
            return Result<bool>.Failure("Product not found");

        var result = await productRepository.DeleteProduct(id);

        return result
            ? Result<bool>.Success(result) 
            : Result<bool>.Failure("Failed to delete product");
    }
}   