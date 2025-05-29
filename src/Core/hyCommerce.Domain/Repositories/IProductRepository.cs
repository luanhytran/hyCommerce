using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Entities.Helpers;

namespace hyCommerce.Domain.Repositories;

public interface IProductRepository
{
    public Task<List<Product>> GetProducts(ProductParams productParams);
    public Task<Product?> GetProduct(int id);
    public Task<Product?> CreateProduct(Product product);
    public Task<bool> DeleteProduct(int id);
}