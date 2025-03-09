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
        return await _context.Products.
            Include(p => p.Category).
            Include(p => p.Brand).
            Skip(productParams.PageSize * (productParams.PageNumber -1)).
            Take(productParams.PageSize).
            ToListAsync();
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