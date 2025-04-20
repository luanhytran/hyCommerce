using hyCommerce.API.RequestHelpers;
using hyCommerce.Core.Common;
using hyCommerce.Core.Models;

namespace hyCommerce.Core.Contracts.Services;

public interface IProductService
{
    public Task<Result<List<Product>>> GetProducts(ProductParams productParams);
    public Task<Result<Product>> GetProduct(int id);
    public Task<Result<Product>> CreateProduct(Product product);
    public Task<Result<bool>> DeleteProduct(int id);
}