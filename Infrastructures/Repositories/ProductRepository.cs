using eCommerceAPI.API.Extensions;
using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;
using eCommerceAPI.Infrastructures.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Infrastructures.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<ActionResult<List<Product>>> GetProducts(ProductParams productParams)
    {
        var query = _context.Products.
            Include(p => p.Category).
            Include(p => p.Brand).
            AsQueryable();

        query = query.Sort(productParams.OrderBy);
        query = query.Search(productParams.SearchTerm);
        query = query.Filter(productParams.Category, productParams.Brand);

        var products = await query.
            Skip(productParams.PageSize * (productParams.PageNumber - 1)).
            Take(productParams.PageSize).
            ToListAsync();

        return products;
    }

    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.
            Include(p => p.Category).
            Include(p => p.Brand).
            FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return new NotFoundResult();

        return product;
    }
}