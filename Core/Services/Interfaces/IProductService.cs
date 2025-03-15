using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;

namespace eCommerceAPI.Core.Services.Interfaces;

public interface IProductService
{
    public Task<List<Product>> GetProducts(ProductParams productParams);
    public Task<Product> GetProduct(int id);
    public Task<Product> CreateProduct(Product product);
}