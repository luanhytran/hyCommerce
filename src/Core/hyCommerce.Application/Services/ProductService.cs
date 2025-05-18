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
        var products = await productRepository.GetProducts(productParams);

        return Result<List<Product>>.Success(products);
    }

    public async Task<Result<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetProduct(id);

        return product != null
            ? Result<Product>.Success(product)
            : Result<Product>.Failure(message: "No product found");
    }

    public async Task<Result<Product>> CreateProduct(Product product)
    {
        var createdProduct = await productRepository.CreateProduct(product);

        if (createdProduct == null)
            return Result<Product>.Failure(message: "Failed to create product");

        await unitOfWork.SaveAsync();

        return Result<Product>.Success(createdProduct);
    }

    public async Task<Result<bool>> DeleteProduct(int id)
    {
        var product = await productRepository.GetProduct(id);

        if (product == null)
            return Result<bool>.Failure(message: "Product not found");

        var result = await productRepository.DeleteProduct(id);

        return result
            ? Result<bool>.Success(result)
            : Result<bool>.Failure(message: "Failed to delete product");
    }
}