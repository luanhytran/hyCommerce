using hyCommerce.Domain;
using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Entities.Helpers;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Extensions.Exceptions;

namespace hyCommerce.Application.Services;

public interface IProductService
{
    public Task<List<Product>> GetProducts(ProductParams productParams);
    public Task<Product?> GetProduct(int id);
    public Task<Product> CreateProduct(Product product);
    public Task<bool> DeleteProduct(int id);
}

public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    : IProductService
{
    public async Task<List<Product>> GetProducts(ProductParams productParams)
    {
        return await productRepository.GetProducts(productParams);
    }

    public async Task<Product?> GetProduct(int id)
    {
        return await productRepository.GetProduct(id) ?? throw new NotFoundException($"Product {id} not found");
    }

    public async Task<Product> CreateProduct(Product product)
    {
        var createdProduct = await productRepository.CreateProduct(product);

        if (createdProduct == null)
            throw new InvalidOperationException("Failed to create product");

        await unitOfWork.SaveAsync();

        return createdProduct;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var deleted = await productRepository.DeleteProduct(id);
        
        if (!deleted)
            throw new NotFoundException("Product not found or already deleted");
        
        return true;
    }
}