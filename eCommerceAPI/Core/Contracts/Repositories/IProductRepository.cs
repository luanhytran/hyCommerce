using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;

namespace eCommerceAPI.Core.Contracts.Repositories;

public interface IProductRepository
{
    public Task<List<Product>> GetProducts(ProductParams productParams);
    public Task<Product?> GetProduct(int id);
    public Task<Product?> CreateProduct(Product product);
    public Task<bool> DeleteProduct(int id);
}