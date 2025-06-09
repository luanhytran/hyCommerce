using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Entities.Cart;
using hyCommerce.Domain.Entities.Order;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace hyCommerce.Infrastructure.Persistence.Data;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Likes> Likes { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasOne(a => a.Address)
            .WithOne()
            .HasForeignKey<UserAddress>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RefreshToken>()
            .HasKey(rt => rt.Id);

        builder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
            );

        builder.Entity<OrderItem>()
            .OwnsOne(oi => oi.ProductItemOrdered, pio => { pio.WithOwner(); });

        builder.Entity<Order>()
            .OwnsOne(o => o.ShippingAdress, sa => { sa.WithOwner(); });
    }
}