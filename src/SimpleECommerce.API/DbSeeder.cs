using Microsoft.EntityFrameworkCore;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.DataAccess.Context;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedRolesAsync(context);
        await SeedAdminUserAsync(context);
        await SeedCategoriesAsync(context);
        await SeedProductsAsync(context);
    }

    private static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        if (await context.Roles.AnyAsync())
            return;

        var roles = new List<Role>
        {
            new() { Name = "Admin", Description = "Administrator with full access" },
            new() { Name = "Customer", Description = "Regular customer" }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminUserAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Email == "admin@simpleecommerce.com"))
            return;

        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (adminRole == null)
            return;

        var adminUser = new User
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@simpleecommerce.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            PhoneNumber = "+90 555 123 4567",
            Address = "Istanbul, Turkey",
            RoleId = adminRole.Id
        };

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var categories = new List<Category>
        {
            new() { Name = "Electronics", Description = "Electronic devices and accessories" },
            new() { Name = "Clothing", Description = "Fashion and apparel" },
            new() { Name = "Books", Description = "Physical and digital books" },
            new() { Name = "Home & Garden", Description = "Home decor and garden supplies" },
            new() { Name = "Sports & Outdoors", Description = "Sports equipment and outdoor gear" }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var electronics = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Electronics");
        var clothing = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Clothing");
        var books = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Books");
        var homeGarden = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Home & Garden");
        var sports = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Sports & Outdoors");

        var products = new List<Product>
        {
            // Electronics
            new()
            {
                Name = "Wireless Bluetooth Headphones",
                Description = "High-quality wireless headphones with noise cancellation",
                Price = 79.99m,
                StockQuantity = 100,
                CategoryId = electronics!.Id,
                ImageUrl = "https://picsum.photos/seed/headphones/300/300"
            },
            new()
            {
                Name = "Smartphone 128GB",
                Description = "Latest model smartphone with 128GB storage",
                Price = 699.99m,
                StockQuantity = 50,
                CategoryId = electronics.Id,
                ImageUrl = "https://picsum.photos/seed/smartphone/300/300"
            },
            new()
            {
                Name = "Laptop 15.6 inch",
                Description = "Powerful laptop for work and gaming",
                Price = 1199.99m,
                StockQuantity = 30,
                CategoryId = electronics.Id,
                ImageUrl = "https://picsum.photos/seed/laptop/300/300"
            },
            new()
            {
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with long battery life",
                Price = 29.99m,
                StockQuantity = 200,
                CategoryId = electronics.Id,
                ImageUrl = "https://picsum.photos/seed/mouse/300/300"
            },

            // Clothing
            new()
            {
                Name = "Men's Cotton T-Shirt",
                Description = "Comfortable cotton t-shirt for everyday wear",
                Price = 19.99m,
                StockQuantity = 150,
                CategoryId = clothing!.Id,
                ImageUrl = "https://picsum.photos/seed/tshirt/300/300"
            },
            new()
            {
                Name = "Women's Denim Jeans",
                Description = "Classic fit denim jeans",
                Price = 49.99m,
                StockQuantity = 80,
                CategoryId = clothing.Id,
                ImageUrl = "https://picsum.photos/seed/jeans/300/300"
            },
            new()
            {
                Name = "Winter Jacket",
                Description = "Warm winter jacket with waterproof material",
                Price = 129.99m,
                StockQuantity = 40,
                CategoryId = clothing.Id,
                ImageUrl = "https://picsum.photos/seed/jacket/300/300"
            },

            // Books
            new()
            {
                Name = "Clean Code",
                Description = "A Handbook of Agile Software Craftsmanship by Robert C. Martin",
                Price = 39.99m,
                StockQuantity = 60,
                CategoryId = books!.Id,
                ImageUrl = "https://picsum.photos/seed/cleancode/300/300"
            },
            new()
            {
                Name = "Design Patterns",
                Description = "Elements of Reusable Object-Oriented Software",
                Price = 54.99m,
                StockQuantity = 45,
                CategoryId = books.Id,
                ImageUrl = "https://picsum.photos/seed/patterns/300/300"
            },
            new()
            {
                Name = "The Pragmatic Programmer",
                Description = "Your Journey to Mastery, 20th Anniversary Edition",
                Price = 44.99m,
                StockQuantity = 55,
                CategoryId = books.Id,
                ImageUrl = "https://picsum.photos/seed/pragmatic/300/300"
            },

            // Home & Garden
            new()
            {
                Name = "Indoor Plant Pot Set",
                Description = "Set of 3 ceramic plant pots in various sizes",
                Price = 34.99m,
                StockQuantity = 70,
                CategoryId = homeGarden!.Id,
                ImageUrl = "https://picsum.photos/seed/plantpot/300/300"
            },
            new()
            {
                Name = "LED Desk Lamp",
                Description = "Adjustable LED desk lamp with USB charging port",
                Price = 39.99m,
                StockQuantity = 85,
                CategoryId = homeGarden.Id,
                ImageUrl = "https://picsum.photos/seed/desklamp/300/300"
            },

            // Sports & Outdoors
            new()
            {
                Name = "Yoga Mat",
                Description = "Non-slip yoga mat with carrying strap",
                Price = 24.99m,
                StockQuantity = 120,
                CategoryId = sports!.Id,
                ImageUrl = "https://picsum.photos/seed/yogamat/300/300"
            },
            new()
            {
                Name = "Running Shoes",
                Description = "Lightweight running shoes with cushioned sole",
                Price = 89.99m,
                StockQuantity = 65,
                CategoryId = sports.Id,
                ImageUrl = "https://picsum.photos/seed/shoes/300/300"
            },
            new()
            {
                Name = "Water Bottle 1L",
                Description = "Insulated stainless steel water bottle",
                Price = 19.99m,
                StockQuantity = 180,
                CategoryId = sports.Id,
                ImageUrl = "https://picsum.photos/seed/bottle/300/300"
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
