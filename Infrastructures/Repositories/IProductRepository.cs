using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;

namespace eCommerceAPI.Infrastructures.Repositories;

public interface IProductRepository
{
    public Task<List<Product>> GetProducts(ProductParams productParams);
    public Task<Product> GetProduct(int id);
    public Task<Product> CreateProduct(Product product);
}