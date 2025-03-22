using eCommerceAPI.API.Extensions;
using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;
using eCommerceAPI.Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Infrastructures.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<List<Product>> GetProducts(ProductParams productParams)
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

    public async Task<Product?> GetProduct(int id)
    {
        return await _context.Products.
            Include(p => p.Category).
            Include(p => p.Brand).
            FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> CreateProduct(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        var newProduct = new Product
        {
            Name = product.Name,
            Description = product.Description,
            PictureUrl = product.PictureUrl,
            Price = product.Price,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId
        };
        
        _context.Add(newProduct);
        return await _context.SaveChangesAsync() > 0 ? newProduct : null;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return false;
        
        _context.Products.Remove(product);
        
        return await _context.SaveChangesAsync() > 0;
    }
}