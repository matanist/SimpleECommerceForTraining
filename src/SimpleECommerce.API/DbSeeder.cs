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
            new() { Name = "Sports & Outdoors", Description = "Sports equipment and outdoor gear" },
            new() { Name = "Toys & Games", Description = "Toys, games and entertainment" },
            new() { Name = "Health & Beauty", Description = "Health, beauty and personal care" },
            new() { Name = "Automotive", Description = "Car parts and accessories" },
            new() { Name = "Food & Beverages", Description = "Food, drinks and groceries" },
            new() { Name = "Office Supplies", Description = "Office and school supplies" }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var categories = await context.Categories.ToListAsync();
        var electronics = categories.First(c => c.Name == "Electronics");
        var clothing = categories.First(c => c.Name == "Clothing");
        var books = categories.First(c => c.Name == "Books");
        var homeGarden = categories.First(c => c.Name == "Home & Garden");
        var sports = categories.First(c => c.Name == "Sports & Outdoors");
        var toys = categories.First(c => c.Name == "Toys & Games");
        var health = categories.First(c => c.Name == "Health & Beauty");
        var automotive = categories.First(c => c.Name == "Automotive");
        var food = categories.First(c => c.Name == "Food & Beverages");
        var office = categories.First(c => c.Name == "Office Supplies");

        var products = new List<Product>();
        var random = new Random(42); // Fixed seed for consistent data

        // Electronics (50 products)
        var electronicsProducts = new[]
        {
            ("Wireless Bluetooth Headphones", "High-quality wireless headphones with noise cancellation", 79.99m),
            ("Smartphone 128GB", "Latest model smartphone with 128GB storage", 699.99m),
            ("Laptop 15.6 inch", "Powerful laptop for work and gaming", 1199.99m),
            ("Wireless Mouse", "Ergonomic wireless mouse with long battery life", 29.99m),
            ("Mechanical Keyboard", "RGB mechanical keyboard with Cherry MX switches", 149.99m),
            ("4K Monitor 27 inch", "Ultra HD monitor with HDR support", 449.99m),
            ("Tablet 10 inch", "Lightweight tablet with stylus support", 349.99m),
            ("Smart Watch", "Fitness tracking smartwatch with heart rate monitor", 199.99m),
            ("Wireless Earbuds", "True wireless earbuds with charging case", 129.99m),
            ("External SSD 1TB", "Portable solid state drive with USB-C", 119.99m),
            ("Webcam HD 1080p", "Full HD webcam for video conferencing", 69.99m),
            ("USB-C Hub", "7-in-1 USB-C hub with HDMI and card reader", 49.99m),
            ("Portable Charger 20000mAh", "High capacity power bank with fast charging", 39.99m),
            ("Gaming Headset", "Surround sound gaming headset with microphone", 89.99m),
            ("Bluetooth Speaker", "Waterproof portable Bluetooth speaker", 59.99m),
            ("Action Camera 4K", "Waterproof action camera with accessories", 249.99m),
            ("Drone with Camera", "Foldable drone with 4K camera", 599.99m),
            ("E-Reader", "E-ink display e-reader with backlight", 129.99m),
            ("Wireless Charging Pad", "Fast wireless charger for smartphones", 29.99m),
            ("Smart Home Hub", "Voice-controlled smart home hub", 99.99m),
            ("Security Camera", "WiFi security camera with night vision", 79.99m),
            ("Smart Doorbell", "Video doorbell with motion detection", 149.99m),
            ("Robot Vacuum", "Smart robot vacuum with mapping", 399.99m),
            ("Air Purifier", "HEPA air purifier for large rooms", 199.99m),
            ("Smart Thermostat", "WiFi enabled smart thermostat", 179.99m),
            ("Streaming Device", "4K streaming media player", 49.99m),
            ("VR Headset", "Virtual reality headset for gaming", 299.99m),
            ("Graphics Card", "High-performance graphics card for gaming", 549.99m),
            ("Gaming Console", "Next-gen gaming console", 499.99m),
            ("Portable Monitor", "15.6 inch portable USB-C monitor", 229.99m),
            ("NAS Storage 4TB", "Network attached storage device", 349.99m),
            ("WiFi Router", "Tri-band WiFi 6 router", 199.99m),
            ("Mesh WiFi System", "Whole home mesh WiFi system", 299.99m),
            ("Smart Light Bulbs", "Color changing smart LED bulbs (4-pack)", 49.99m),
            ("Smart Plug", "WiFi smart plug with energy monitoring", 24.99m),
            ("Digital Photo Frame", "10 inch digital photo frame with WiFi", 89.99m),
            ("Projector Full HD", "Home theater projector with 1080p", 349.99m),
            ("Soundbar", "2.1 channel soundbar with subwoofer", 179.99m),
            ("Record Player", "Vintage style turntable with Bluetooth", 129.99m),
            ("Digital Voice Recorder", "Professional voice recorder with 16GB", 59.99m),
            ("Calculator Scientific", "Advanced scientific calculator", 24.99m),
            ("Label Maker", "Portable label maker with keyboard", 39.99m),
            ("Document Scanner", "Portable document scanner", 149.99m),
            ("Laminator", "A4 laminating machine", 34.99m),
            ("Paper Shredder", "Cross-cut paper shredder", 79.99m),
            ("UPS Battery Backup", "Uninterruptible power supply 1000VA", 129.99m),
            ("Surge Protector", "8-outlet surge protector with USB", 29.99m),
            ("HDMI Cable 6ft", "High-speed HDMI cable with Ethernet", 14.99m),
            ("USB Flash Drive 64GB", "USB 3.0 flash drive", 12.99m),
            ("Memory Card 128GB", "High-speed microSD card", 24.99m)
        };

        foreach (var (name, desc, price) in electronicsProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(20, 200),
                CategoryId = electronics.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "")}/300/300"
            });
        }

        // Clothing (40 products)
        var clothingProducts = new[]
        {
            ("Men's Cotton T-Shirt", "Comfortable cotton t-shirt for everyday wear", 19.99m),
            ("Women's Denim Jeans", "Classic fit denim jeans", 49.99m),
            ("Winter Jacket", "Warm winter jacket with waterproof material", 129.99m),
            ("Running Sneakers", "Breathable running shoes", 89.99m),
            ("Leather Belt", "Genuine leather belt with silver buckle", 34.99m),
            ("Wool Sweater", "Soft merino wool sweater", 69.99m),
            ("Casual Dress Shirt", "Button-down dress shirt for office", 44.99m),
            ("Summer Dress", "Floral print summer dress", 54.99m),
            ("Cargo Pants", "Multi-pocket cargo pants", 59.99m),
            ("Hoodie Sweatshirt", "Comfortable pullover hoodie", 49.99m),
            ("Formal Suit Jacket", "Slim fit suit jacket", 199.99m),
            ("Silk Tie", "Handmade silk necktie", 39.99m),
            ("Polo Shirt", "Classic polo shirt with logo", 34.99m),
            ("Yoga Pants", "High-waist yoga leggings", 44.99m),
            ("Denim Jacket", "Classic blue denim jacket", 79.99m),
            ("Raincoat", "Waterproof rain jacket with hood", 69.99m),
            ("Swim Shorts", "Quick-dry swim trunks", 29.99m),
            ("Sports Bra", "High-support sports bra", 34.99m),
            ("Thermal Underwear", "Thermal base layer set", 39.99m),
            ("Flannel Shirt", "Plaid flannel shirt", 44.99m),
            ("Linen Pants", "Lightweight linen trousers", 54.99m),
            ("Cardigan", "Button-up knit cardigan", 59.99m),
            ("Tank Top", "Athletic tank top", 19.99m),
            ("Shorts Chino", "Cotton chino shorts", 34.99m),
            ("Maxi Skirt", "Flowy maxi skirt", 44.99m),
            ("Blazer", "Casual unstructured blazer", 89.99m),
            ("Turtleneck Sweater", "Ribbed turtleneck sweater", 54.99m),
            ("Parka Coat", "Insulated parka with fur hood", 179.99m),
            ("Leather Jacket", "Classic leather motorcycle jacket", 249.99m),
            ("Pajama Set", "Cotton pajama top and bottom", 39.99m),
            ("Bathrobe", "Plush terry cloth bathrobe", 49.99m),
            ("Scarf Cashmere", "Soft cashmere scarf", 79.99m),
            ("Winter Gloves", "Touchscreen compatible gloves", 24.99m),
            ("Beanie Hat", "Warm knit beanie", 19.99m),
            ("Baseball Cap", "Adjustable baseball cap", 24.99m),
            ("Socks Pack", "Cotton socks 6-pack", 19.99m),
            ("Underwear Pack", "Cotton underwear 3-pack", 29.99m),
            ("Flip Flops", "Comfortable beach flip flops", 14.99m),
            ("Dress Shoes", "Oxford dress shoes", 129.99m),
            ("Ankle Boots", "Leather ankle boots", 109.99m)
        };

        foreach (var (name, desc, price) in clothingProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(30, 150),
                CategoryId = clothing.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "").Replace("'", "")}/300/300"
            });
        }

        // Books (35 products)
        var booksProducts = new[]
        {
            ("Clean Code", "A Handbook of Agile Software Craftsmanship by Robert C. Martin", 39.99m),
            ("Design Patterns", "Elements of Reusable Object-Oriented Software", 54.99m),
            ("The Pragmatic Programmer", "Your Journey to Mastery, 20th Anniversary Edition", 44.99m),
            ("Code Complete", "A Practical Handbook of Software Construction", 49.99m),
            ("Introduction to Algorithms", "Comprehensive algorithms textbook", 89.99m),
            ("Head First Design Patterns", "A Brain-Friendly Guide", 44.99m),
            ("Refactoring", "Improving the Design of Existing Code", 49.99m),
            ("Domain-Driven Design", "Tackling Complexity in the Heart of Software", 59.99m),
            ("The Clean Coder", "A Code of Conduct for Professional Programmers", 34.99m),
            ("Effective Java", "Best Practices for the Java Platform", 44.99m),
            ("JavaScript The Good Parts", "Unearthing the Excellence in JavaScript", 29.99m),
            ("You Don't Know JS", "Scope and Closures", 24.99m),
            ("Python Crash Course", "A Hands-On Introduction to Programming", 34.99m),
            ("Learning Python", "Powerful Object-Oriented Programming", 54.99m),
            ("C# in Depth", "Fourth Edition", 44.99m),
            ("Pro ASP.NET Core", "Develop Cloud-Ready Web Apps", 49.99m),
            ("Docker Deep Dive", "Zero to Docker in a single book", 39.99m),
            ("Kubernetes Up & Running", "Dive into the Future of Infrastructure", 44.99m),
            ("Site Reliability Engineering", "How Google Runs Production Systems", 49.99m),
            ("The Phoenix Project", "A Novel About IT, DevOps, and Helping Your Business Win", 24.99m),
            ("Continuous Delivery", "Reliable Software Releases", 54.99m),
            ("Microservices Patterns", "With examples in Java", 49.99m),
            ("Building Microservices", "Designing Fine-Grained Systems", 44.99m),
            ("Release It!", "Design and Deploy Production-Ready Software", 44.99m),
            ("System Design Interview", "An insider's guide", 39.99m),
            ("Cracking the Coding Interview", "189 Programming Questions and Solutions", 34.99m),
            ("The Algorithm Design Manual", "Second Edition", 69.99m),
            ("Grokking Algorithms", "An illustrated guide for programmers", 34.99m),
            ("SQL Performance Explained", "Everything developers need to know", 29.99m),
            ("NoSQL Distilled", "A Brief Guide to the Emerging World of Polyglot Persistence", 34.99m),
            ("MongoDB The Definitive Guide", "Powerful and Scalable Data Storage", 44.99m),
            ("Redis in Action", "Real-world use cases and patterns", 39.99m),
            ("Designing Data-Intensive Applications", "The Big Ideas Behind Reliable Systems", 49.99m),
            ("Web Scalability for Startup Engineers", "Tips & Techniques for Scaling Your Web Application", 39.99m),
            ("High Performance Browser Networking", "What every web developer should know", 44.99m)
        };

        foreach (var (name, desc, price) in booksProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(20, 100),
                CategoryId = books.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "").Replace("'", "")}/300/300"
            });
        }

        // Home & Garden (35 products)
        var homeProducts = new[]
        {
            ("Indoor Plant Pot Set", "Set of 3 ceramic plant pots in various sizes", 34.99m),
            ("LED Desk Lamp", "Adjustable LED desk lamp with USB charging port", 39.99m),
            ("Throw Pillow Set", "Decorative throw pillows 2-pack", 29.99m),
            ("Area Rug 5x7", "Soft shag area rug", 89.99m),
            ("Wall Clock", "Modern minimalist wall clock", 34.99m),
            ("Picture Frame Set", "Gallery wall picture frame set 8-pack", 44.99m),
            ("Curtains Blackout", "Thermal insulated blackout curtains", 39.99m),
            ("Bedding Set Queen", "Cotton bedding set with duvet cover", 79.99m),
            ("Bath Towel Set", "Luxury bath towels 6-piece set", 49.99m),
            ("Kitchen Knife Set", "Stainless steel knife set with block", 89.99m),
            ("Cookware Set", "Non-stick cookware 10-piece set", 129.99m),
            ("Food Storage Containers", "Glass food storage 18-piece set", 44.99m),
            ("Coffee Maker", "12-cup programmable coffee maker", 59.99m),
            ("Blender", "High-speed countertop blender", 79.99m),
            ("Air Fryer", "Digital air fryer 5.8 quart", 99.99m),
            ("Instant Pot", "Multi-use pressure cooker 6 quart", 89.99m),
            ("Toaster 4-Slice", "Wide slot toaster with bagel function", 49.99m),
            ("Electric Kettle", "Stainless steel electric kettle", 34.99m),
            ("Vacuum Cleaner", "Bagless upright vacuum cleaner", 149.99m),
            ("Steam Mop", "Lightweight steam mop for hard floors", 79.99m),
            ("Iron Steam", "Steam iron with anti-drip", 39.99m),
            ("Laundry Basket", "Collapsible laundry hamper", 24.99m),
            ("Shoe Rack", "5-tier shoe organizer", 34.99m),
            ("Coat Rack", "Standing coat rack with hooks", 44.99m),
            ("Bookshelf", "5-shelf bookcase", 79.99m),
            ("Desk Organizer", "Desktop organizer with drawers", 29.99m),
            ("Garden Hose 50ft", "Expandable garden hose with nozzle", 34.99m),
            ("Lawn Mower", "Electric push lawn mower", 249.99m),
            ("Hedge Trimmer", "Cordless hedge trimmer", 89.99m),
            ("Outdoor String Lights", "LED patio string lights 48ft", 39.99m),
            ("Patio Chair Set", "Outdoor dining chairs 4-pack", 199.99m),
            ("BBQ Grill", "Propane gas grill with side burner", 349.99m),
            ("Fire Pit", "Outdoor fire pit with cover", 149.99m),
            ("Planters Set", "Outdoor planters 3-pack", 59.99m),
            ("Welcome Mat", "Durable outdoor doormat", 24.99m)
        };

        foreach (var (name, desc, price) in homeProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(25, 120),
                CategoryId = homeGarden.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "")}/300/300"
            });
        }

        // Sports & Outdoors (30 products)
        var sportsProducts = new[]
        {
            ("Yoga Mat", "Non-slip yoga mat with carrying strap", 24.99m),
            ("Running Shoes Pro", "Lightweight running shoes with cushioned sole", 89.99m),
            ("Water Bottle 1L", "Insulated stainless steel water bottle", 19.99m),
            ("Dumbbell Set", "Adjustable dumbbell set 5-25 lbs", 149.99m),
            ("Resistance Bands", "Exercise resistance bands 5-pack", 24.99m),
            ("Jump Rope", "Speed jump rope with ball bearings", 14.99m),
            ("Foam Roller", "High-density foam roller for recovery", 29.99m),
            ("Pull-Up Bar", "Doorway pull-up bar", 34.99m),
            ("Kettlebell 20lb", "Cast iron kettlebell", 44.99m),
            ("Ab Roller", "Ab wheel roller with knee pad", 19.99m),
            ("Fitness Tracker", "Activity tracker with heart rate", 79.99m),
            ("Gym Bag", "Large sports duffel bag", 39.99m),
            ("Tennis Racket", "Adult tennis racket with cover", 69.99m),
            ("Basketball", "Official size indoor/outdoor basketball", 29.99m),
            ("Soccer Ball", "FIFA approved match soccer ball", 34.99m),
            ("Football", "Official size leather football", 29.99m),
            ("Baseball Glove", "Leather baseball glove", 49.99m),
            ("Golf Club Set", "Complete golf club set with bag", 399.99m),
            ("Bicycle Helmet", "Adult cycling helmet", 49.99m),
            ("Bike Lock", "Heavy duty bike lock", 29.99m),
            ("Camping Tent", "4-person waterproof camping tent", 149.99m),
            ("Sleeping Bag", "3-season sleeping bag", 59.99m),
            ("Hiking Backpack", "50L hiking backpack with rain cover", 89.99m),
            ("Trekking Poles", "Adjustable trekking poles pair", 44.99m),
            ("Camping Stove", "Portable camping stove", 39.99m),
            ("Cooler Bag", "Insulated cooler bag 24-can", 34.99m),
            ("Fishing Rod", "Spinning fishing rod and reel combo", 59.99m),
            ("Kayak Paddle", "Lightweight kayak paddle", 69.99m),
            ("Snorkel Set", "Mask and snorkel set with fins", 49.99m),
            ("Skateboard", "Complete skateboard for beginners", 59.99m)
        };

        foreach (var (name, desc, price) in sportsProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(30, 150),
                CategoryId = sports.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "")}/300/300"
            });
        }

        // Toys & Games (25 products)
        var toysProducts = new[]
        {
            ("LEGO Classic Set", "Creative building blocks 900 pieces", 49.99m),
            ("Board Game Collection", "Classic board games 10-in-1", 34.99m),
            ("Remote Control Car", "RC racing car with rechargeable battery", 44.99m),
            ("Puzzle 1000 Pieces", "Challenging jigsaw puzzle", 19.99m),
            ("Action Figure Set", "Superhero action figures 6-pack", 29.99m),
            ("Dollhouse", "Wooden dollhouse with furniture", 89.99m),
            ("Train Set", "Electric train set with tracks", 69.99m),
            ("Building Blocks", "Magnetic building tiles 100-piece", 39.99m),
            ("Play-Doh Set", "Play-Doh modeling compound 20-pack", 19.99m),
            ("Art Supplies Kit", "Kids art supplies with case", 34.99m),
            ("Science Kit", "Chemistry science experiment kit", 29.99m),
            ("Telescope Kids", "Beginner telescope for kids", 49.99m),
            ("Microscope Set", "Kids microscope with slides", 39.99m),
            ("Drone Mini", "Mini drone for beginners", 39.99m),
            ("Video Game", "Popular video game title", 59.99m),
            ("Gaming Controller", "Wireless gaming controller", 49.99m),
            ("Card Game Pack", "Popular card games bundle", 24.99m),
            ("Chess Set", "Wooden chess set with board", 34.99m),
            ("Nerf Blaster", "Foam dart blaster with darts", 29.99m),
            ("Water Gun", "Super soaker water gun", 19.99m),
            ("Kite", "Rainbow delta kite", 14.99m),
            ("Frisbee", "Professional flying disc", 12.99m),
            ("Stuffed Animal", "Giant teddy bear 4ft", 49.99m),
            ("Baby Doll", "Interactive baby doll", 39.99m),
            ("Toy Kitchen", "Play kitchen set with accessories", 129.99m)
        };

        foreach (var (name, desc, price) in toysProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(40, 200),
                CategoryId = toys.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "")}/300/300"
            });
        }

        // Health & Beauty (25 products)
        var healthProducts = new[]
        {
            ("Electric Toothbrush", "Rechargeable sonic toothbrush", 49.99m),
            ("Hair Dryer", "Ionic hair dryer with diffuser", 39.99m),
            ("Shampoo & Conditioner Set", "Sulfate-free hair care set", 24.99m),
            ("Face Moisturizer", "Daily hydrating face cream", 29.99m),
            ("Sunscreen SPF 50", "Broad spectrum sunscreen", 14.99m),
            ("Vitamin D Supplements", "Vitamin D3 5000 IU 180 capsules", 19.99m),
            ("Protein Powder", "Whey protein powder 2lb", 34.99m),
            ("Essential Oils Set", "Aromatherapy oils 8-pack", 29.99m),
            ("Massage Gun", "Deep tissue massage gun", 99.99m),
            ("Blood Pressure Monitor", "Digital blood pressure cuff", 39.99m),
            ("Thermometer Digital", "Instant read digital thermometer", 14.99m),
            ("First Aid Kit", "Complete first aid kit 300 pieces", 29.99m),
            ("Humidifier", "Cool mist humidifier for bedroom", 44.99m),
            ("Heating Pad", "Electric heating pad for pain relief", 29.99m),
            ("Sleep Mask", "Silk sleep mask with earplugs", 14.99m),
            ("Nail Care Set", "Manicure pedicure kit", 24.99m),
            ("Makeup Brush Set", "Professional makeup brushes 12-piece", 29.99m),
            ("Perfume", "Designer fragrance 3.4 oz", 79.99m),
            ("Cologne", "Men's cologne 3.4 oz", 69.99m),
            ("Razor Electric", "Men's electric shaver", 79.99m),
            ("Hair Straightener", "Ceramic flat iron", 44.99m),
            ("Curling Iron", "Professional curling wand", 39.99m),
            ("Facial Cleanser", "Gentle foaming cleanser", 19.99m),
            ("Body Lotion", "Moisturizing body lotion 16 oz", 14.99m),
            ("Deodorant Pack", "Natural deodorant 3-pack", 19.99m)
        };

        foreach (var (name, desc, price) in healthProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(50, 250),
                CategoryId = health.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "").Replace("&", "")}/300/300"
            });
        }

        // Automotive (20 products)
        var autoProducts = new[]
        {
            ("Car Phone Mount", "Magnetic phone holder for car", 19.99m),
            ("Dash Cam", "1080p dash camera with night vision", 69.99m),
            ("Jump Starter", "Portable car jump starter", 79.99m),
            ("Tire Inflator", "Portable air compressor", 39.99m),
            ("Car Vacuum", "Handheld car vacuum cleaner", 34.99m),
            ("Seat Covers Set", "Universal car seat covers", 49.99m),
            ("Floor Mats", "All-weather car floor mats", 39.99m),
            ("Steering Wheel Cover", "Leather steering wheel cover", 19.99m),
            ("Car Charger", "Dual USB fast car charger", 14.99m),
            ("Bluetooth FM Transmitter", "Wireless Bluetooth car adapter", 24.99m),
            ("Car Wax", "Premium carnauba car wax", 19.99m),
            ("Car Wash Kit", "Complete car washing kit", 34.99m),
            ("Microfiber Towels", "Car detailing towels 12-pack", 19.99m),
            ("Windshield Sunshade", "Foldable car sunshade", 14.99m),
            ("Trunk Organizer", "Collapsible trunk storage", 29.99m),
            ("Emergency Kit", "Roadside emergency kit", 49.99m),
            ("Motor Oil 5W-30", "Synthetic motor oil 5 quart", 29.99m),
            ("Air Freshener Pack", "Car air fresheners 6-pack", 9.99m),
            ("LED Headlights", "LED headlight bulbs pair", 44.99m),
            ("Wiper Blades", "All-season wiper blades pair", 24.99m)
        };

        foreach (var (name, desc, price) in autoProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(40, 180),
                CategoryId = automotive.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "")}/300/300"
            });
        }

        // Food & Beverages (20 products)
        var foodProducts = new[]
        {
            ("Coffee Beans 1kg", "Premium arabica coffee beans", 24.99m),
            ("Green Tea Pack", "Organic green tea bags 100-count", 14.99m),
            ("Honey Organic", "Raw organic honey 32 oz", 19.99m),
            ("Olive Oil Extra Virgin", "Italian extra virgin olive oil 1L", 24.99m),
            ("Protein Bars Box", "High protein bars 12-pack", 29.99m),
            ("Mixed Nuts", "Roasted mixed nuts 2 lb", 19.99m),
            ("Dark Chocolate", "Premium dark chocolate bars 6-pack", 14.99m),
            ("Pasta Pack", "Italian pasta variety 6-pack", 12.99m),
            ("Rice Basmati", "Premium basmati rice 5 lb", 14.99m),
            ("Maple Syrup", "Pure maple syrup 32 oz", 19.99m),
            ("Hot Sauce Set", "Gourmet hot sauce collection 6-pack", 24.99m),
            ("Spice Set", "Gourmet spice collection 20-piece", 34.99m),
            ("Energy Drinks", "Energy drinks 12-pack", 24.99m),
            ("Sparkling Water", "Sparkling water variety 24-pack", 19.99m),
            ("Beef Jerky", "Premium beef jerky 1 lb", 24.99m),
            ("Trail Mix", "Organic trail mix 2 lb", 14.99m),
            ("Peanut Butter", "Natural peanut butter 2-pack", 12.99m),
            ("Coconut Oil", "Organic virgin coconut oil 32 oz", 14.99m),
            ("Oatmeal Pack", "Instant oatmeal variety 48-count", 19.99m),
            ("Dried Fruit Mix", "Dried fruit variety 2 lb", 16.99m)
        };

        foreach (var (name, desc, price) in foodProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(60, 300),
                CategoryId = food.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "")}/300/300"
            });
        }

        // Office Supplies (20 products)
        var officeProducts = new[]
        {
            ("Notebook Pack", "College ruled notebooks 5-pack", 14.99m),
            ("Pens Gel", "Gel pens assorted colors 24-pack", 12.99m),
            ("Highlighters", "Highlighter markers 12-pack", 9.99m),
            ("Sticky Notes", "Post-it notes variety 12-pack", 14.99m),
            ("Binder Clips", "Binder clips assorted sizes 120-pack", 9.99m),
            ("Stapler", "Desktop stapler with staples", 12.99m),
            ("Tape Dispenser", "Desktop tape dispenser with tape", 9.99m),
            ("Scissors Set", "Office scissors 3-pack", 12.99m),
            ("File Folders", "Manila file folders 100-pack", 19.99m),
            ("Desk Calendar", "2024 desk calendar with stand", 14.99m),
            ("Planner", "Weekly planner with goals", 19.99m),
            ("Whiteboard", "Magnetic dry erase board 24x36", 39.99m),
            ("Markers Dry Erase", "Dry erase markers 12-pack", 12.99m),
            ("Printer Paper", "Copy paper 5000 sheets", 44.99m),
            ("Envelopes", "Business envelopes 500-pack", 24.99m),
            ("Rubber Bands", "Rubber bands assorted 1 lb", 6.99m),
            ("Paper Clips", "Paper clips 1000-count", 7.99m),
            ("Pencils Mechanical", "Mechanical pencils 12-pack", 14.99m),
            ("Eraser Pack", "Erasers 24-pack", 6.99m),
            ("Calculator Desktop", "12-digit desktop calculator", 19.99m)
        };

        foreach (var (name, desc, price) in officeProducts)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                StockQuantity = random.Next(80, 400),
                CategoryId = office.Id,
                ImageUrl = $"https://picsum.photos/seed/{name.ToLower().Replace(" ", "").Replace("-", "")}/300/300"
            });
        }

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
