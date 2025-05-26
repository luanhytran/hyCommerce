using hyCommerce.Domain;
using hyCommerce.Domain.Common;
using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Entities.Helpers;
using hyCommerce.Domain.Repositories;

namespace hyCommerce.Application.Services;

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
        return await productRepository.GetProducts(productParams);
    }

    public async Task<Result<Product>> GetProduct(int id)
    {
        return await productRepository.GetProduct(id);
    }

    public async Task<Result<Product>> CreateProduct(Product product)
    {
        var createdProduct = await productRepository.CreateProduct(product);

        if (createdProduct == null)
        {
            return Result.Failure<Product>(
                new Error("ProductErrors.CreationFailed", "Failed to create product"));
        }

        await unitOfWork.SaveAsync();

        return Result.Success(createdProduct);
    }

    public async Task<Result<bool>> DeleteProduct(int id)
    {
        var product = await productRepository.GetProduct(id);

        if (product == null)
            return Result.Failure<bool>(
                new Error("ProductErrors.NotFound", "Product not found"));

        var result = await productRepository.DeleteProduct(id);

        return result
            ? Result.Success(result)
            : Result.Failure<bool>(
                new Error("ProductErrors.DeletionFailed", "Failed to delete product"));
    }
}