using eCommerceAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Infrastructures.Data;

public class eCommerceContext : DbContext
{
    public eCommerceContext(DbContextOptions<eCommerceContext> options) : base(options)
    {
    }

    DbSet<Product> Products { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Brand> Brands { get; set; }
    DbSet<Order> Orders { get; set; }
    DbSet<OrderItem> OrderItems { get; set; }
    DbSet<Basket> Baskets { get; set; }
    DbSet<BasketItem> BasketItems { get; set; }
    DbSet<Likes> Likes { get; set; }
    DbSet<Discount> Discounts { get; set; }
}