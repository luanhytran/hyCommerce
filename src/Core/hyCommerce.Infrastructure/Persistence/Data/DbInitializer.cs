using hyCommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace hyCommerce.Infrastructure.Persistence.Data;

public class DbInitializer
{
    public static async Task Initialize(AppDbContext context, UserManager<User> userManager)
    {
        var now = DateTime.UtcNow;
        const string adminUser = "admin";
        
        if (!userManager.Users.Any())
        {
            var user = new User
            {
                UserName = "bob",
                Email = "bob@test.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");

            var admin = new User
            {
                UserName = adminUser,
                Email = "admin@test.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Member", "Admin" });
        }

        if (context.Products.Any()) return;

        var brands = new List<Brand>
        {
            new Brand { Name = "Angular", CreatedBy = adminUser, CreatedAt = now },
            new Brand { Name = "NetCore", CreatedBy = adminUser, CreatedAt = now },
            new Brand { Name = "React", CreatedBy = adminUser, CreatedAt = now },
            new Brand { Name = "TypeScript", CreatedBy = adminUser, CreatedAt = now },
            new Brand { Name = "VS Code", CreatedBy = adminUser, CreatedAt = now },
            new Brand { Name = "Redis", CreatedBy = adminUser, CreatedAt = now }
        };

        var categories = new List<Category>
        {
            new Category { Name = "Boards", CreatedBy = adminUser, CreatedAt = now },
            new Category { Name = "Hats", CreatedBy = adminUser, CreatedAt = now },
            new Category { Name = "Gloves", CreatedBy = adminUser, CreatedAt = now },
            new Category { Name = "Boots", CreatedBy = adminUser, CreatedAt = now }
        };

        context.Brands.AddRange(brands);
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var products = new List<Product>
        {
            new Product
            {
                Name = "Angular Speedster Board 2000",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 20000,
                PictureUrl = "/images/products/sb-ang1.png",
                Brand = brands.First(b => b.Name == "Angular"),
                Category = categories.First(c => c.Name == "Boards"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Green Angular Board 3000",
                Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
                Price = 15000,
                PictureUrl = "/images/products/sb-ang2.png",
                Brand = brands.First(b => b.Name == "Angular"),
                Category = categories.First(c => c.Name == "Boards"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Core Board Speed Rush 3",
                Description = "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                Price = 18000,
                PictureUrl = "/images/products/sb-core1.png",
                Brand = brands.First(b => b.Name == "NetCore"),
                Category = categories.First(c => c.Name == "Boards"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Net Core Super Board",
                Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                Price = 30000,
                PictureUrl = "/images/products/sb-core2.png",
                Brand = brands.First(b => b.Name == "NetCore"),
                Category = categories.First(c => c.Name == "Boards"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "React Board Super Whizzy Fast",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 25000,
                PictureUrl = "/images/products/sb-react1.png",
                Brand = brands.First(b => b.Name == "React"),
                Category = categories.First(c => c.Name == "Boards"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Typescript Entry Board",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 12000,
                PictureUrl = "/images/products/sb-ts1.png",
                Brand = brands.First(b => b.Name == "TypeScript"),
                Category = categories.First(c => c.Name == "Boards"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Core Blue Hat",
                Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 1000,
                PictureUrl = "/images/products/hat-core1.png",
                Brand = brands.First(b => b.Name == "NetCore"),
                Category = categories.First(c => c.Name == "Hats"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Green React Woolen Hat",
                Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 8000,
                PictureUrl = "/images/products/hat-react1.png",
                Brand = brands.First(b => b.Name == "React"),
                Category = categories.First(c => c.Name == "Hats"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Purple React Woolen Hat",
                Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 1500,
                PictureUrl = "/images/products/hat-react2.png",
                Brand = brands.First(b => b.Name == "React"),
                Category = categories.First(c => c.Name == "Hats"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Blue Code Gloves",
                Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 1800,
                PictureUrl = "/images/products/glove-code1.png",
                Brand = brands.First(b => b.Name == "VS Code"),
                Category = categories.First(c => c.Name == "Gloves"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Green Code Gloves",
                Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 1500,
                PictureUrl = "/images/products/glove-code2.png",
                Brand = brands.First(b => b.Name == "VS Code"),
                Category = categories.First(c => c.Name == "Gloves"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Purple React Gloves",
                Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 1600,
                PictureUrl = "/images/products/glove-react1.png",
                Brand = brands.First(b => b.Name == "React"),
                Category = categories.First(c => c.Name == "Gloves"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Green React Gloves",
                Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 1400,
                PictureUrl = "/images/products/glove-react2.png",
                Brand = brands.First(b => b.Name == "React"),
                Category = categories.First(c => c.Name == "Gloves"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Redis Red Boots",
                Description = "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                Price = 25000,
                PictureUrl = "/images/products/boot-redis1.png",
                Brand = brands.First(b => b.Name == "Redis"),
                Category = categories.First(c => c.Name == "Boots"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Core Red Boots",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                Price = 18999,
                PictureUrl = "/images/products/boot-core2.png",
                Brand = brands.First(b => b.Name == "NetCore"),
                Category = categories.First(c => c.Name == "Boots"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Core Purple Boots",
                Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                Price = 19999,
                PictureUrl = "/images/products/boot-core1.png",
                Brand = brands.First(b => b.Name == "NetCore"),
                Category = categories.First(c => c.Name == "Boots"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Angular Purple Boots",
                Description = "Aenean nec lorem. In porttitor. Donec laoreet nonummy augue.",
                Price = 15000,
                PictureUrl = "/images/products/boot-ang2.png",
                Brand = brands.First(b => b.Name == "Angular"),
                Category = categories.First(c => c.Name == "Boots"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
            new Product
            {
                Name = "Angular Blue Boots",
                Description = "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                Price = 18000,
                PictureUrl = "/images/products/boot-ang1.png",
                Brand = brands.First(b => b.Name == "Angular"),
                Category = categories.First(c => c.Name == "Boots"),
                QuantityInStock = 100,
                IsDeleted = false,
                CreatedBy = adminUser,
                CreatedAt = now
            },
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}