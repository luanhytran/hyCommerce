using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.Core.Services;

public interface IProductService
{
    public Task<ActionResult<List<Product>>> GetProducts(ProductParams productParams);
    public Task<ActionResult<Product>> GetProduct(int id);
}