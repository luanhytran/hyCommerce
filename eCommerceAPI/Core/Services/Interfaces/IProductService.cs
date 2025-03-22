using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Common;
using eCommerceAPI.Core.Models;

namespace eCommerceAPI.Core.Services.Interfaces;

public interface IProductService
{
    public Task<Result<List<Product>>> GetProducts(ProductParams productParams);
    public Task<Result<Product>> GetProduct(int id);
    public Task<Result<Product>> CreateProduct(Product product);
    public Task<Result<bool>> DeleteProduct(int id);
}