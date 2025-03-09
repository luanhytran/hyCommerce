using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.Infrastructures.Repositories;

public interface IProductRepository
{
    public Task<ActionResult<List<Product>>> GetProducts(ProductParams productParams);
    public Task<ActionResult<Product>> GetProduct(int id);
}