using eCommerceAPI.Core.Models;
using eCommerceAPI.Core.Models.Order;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace eCommerceAPI.Infrastructures.Data;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasOne(a => a.Address)
            .WithOne()
            .HasForeignKey<UserAddress>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
            );
    }
}