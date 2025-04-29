using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Entities.Helpers;
using hyCommerce.Domain.Extensions;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace hyCommerce.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<List<Product>> GetProducts(ProductParams productParams)
    {
        var query = context.Products.
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
        return await context.Products.
            Include(p => p.Category).
            Include(p => p.Brand).
            FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> CreateProduct(Product product)
    {
        var newProduct = new Product
        {
            Name = product.Name,
            Description = product.Description,
            PictureUrl = product.PictureUrl,
            Price = product.Price,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId
        };
        
        await context.Products.AddAsync(newProduct);

        return newProduct;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null) 
            return false;
        
        context.Products.Remove(product);
        
        return await context.SaveChangesAsync() > 0;
    }
}