namespace ECommerceApi.Context
{
    using ECommerceApi.Entities;
    using Microsoft.EntityFrameworkCore;
 
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Snacks", ImageUrl = "lanches1.png" },
                new Category { Id = 2, Name = "Combos", ImageUrl = "combos1.png" },
                new Category { Id = 3, Name = "Naturals", ImageUrl = "naturais1.png" },
                new Category { Id = 4, Name = "Drinks", ImageUrl = "refrigerantes1.png" },
                new Category { Id = 5, Name = "Juices", ImageUrl = "sucos1.png" },
                new Category { Id = 6, Name = "Desserts", ImageUrl = "sobremesas1.png" }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Hamburger", ImageUrl = "hamburger1.jpeg", CategoryId = 1, Price = 15, InStock = 13, Available = true, BestSeller = true, Popular = true, Details = "Plain bread, seasoned beef hamburger, onion, mustard and ketchup." },
                new Product { Id = 2, Name = "CheeseBurger", ImageUrl = "hamburger3.jpeg", CategoryId = 1, Price = 18, InStock = 10, Available = true, BestSeller = false, Popular = true, Details = "Plain bread, seasoned beef hamburger and cheese overall." },
                new Product { Id = 3, Name = "CheeseSalad", ImageUrl = "hamburger4.jpeg", CategoryId = 1, Price = 19, InStock = 13, Available = true, BestSeller = false, Popular = false, Details = "Plain bread, seasoned beef hamburger, onion, lettuce, mustard and ketchup." },
                new Product { Id = 4, Name = "Hamburger, french fries, soda", ImageUrl = "combo1.jpeg", CategoryId = 2, Price = 25, InStock = 10, Available = false, BestSeller = false, Popular = true, Details = "Plain bread, seasoned beef hamburger, cheese, french fries and soda." },
                new Product { Id = 5, Name = "CheeseBurger, french fries, soda", ImageUrl = "combo2.jpeg", CategoryId = 2, Price = 27, InStock = 13, Available = true, BestSeller = false, Popular = false, Details = "Plain bread, beef hamburger, onion, mayonnaise, ketchup, french fries and soda." },
                new Product { Id = 6, Name = "CheeseSalad, french fries, soda", ImageUrl = "combo3.jpeg", CategoryId = 2, Price = 28, InStock = 10, Available = true, BestSeller = false, Popular = true, Details = "Plain bread, beef hamburger, onion, mayonnaise, ketchup, french fries and soda." },
                new Product { Id = 7, Name = "Natural snack with leaves", ImageUrl = "lanche_natural1.jpeg", CategoryId = 3, Price = 14, InStock = 13, Available = true, BestSeller = false, Popular = false, Details = "Whole-corn bread with leaves and tomato." },
                new Product { Id = 8, Name = "Natural snack with cheese", ImageUrl = "lanche_natural2.jpeg", CategoryId = 3, Price = 15, InStock = 10, Available = true, BestSeller = false, Popular = true, Details = "Whole-corn bread, leaves, tomato and cheese." },
                new Product { Id = 9, Name = "Vegan snack", ImageUrl = "lanche_vegano1.jpeg", CategoryId = 3, Price = 25, InStock = 18, Available = true, BestSeller = false, Popular = false, Details = "Vegan snack with healthy ingredients." },
                new Product { Id = 10, Name = "Coca-Cola", ImageUrl = "coca_cola1.jpeg", CategoryId = 4, Price = 21, InStock = 7, Available = true, BestSeller = false, Popular = true, Details = "Coca Cola" },
                new Product { Id = 11, Name = "Guaraná", ImageUrl = "guarana1.jpeg", CategoryId = 4, Price = 25, InStock = 6, Available = true, BestSeller = false, Popular = false, Details = "Guaraná" },
                new Product { Id = 12, Name = "Pepsi", ImageUrl = "pepsi1.jpeg", CategoryId = 4, Price = 21, InStock = 6, Available = true, BestSeller = false, Popular = false, Details = "Pepsi" },
                new Product { Id = 13, Name = "Orange juice", ImageUrl = "suco_laranja.jpeg", CategoryId = 5, Price = 11, InStock = 10, Available = true, BestSeller = false, Popular = false, Details = "Nutritious and tasty orange juice." },
                new Product { Id = 14, Name = "Strawberry juice", ImageUrl = "suco_morango1.jpeg", CategoryId = 5, Price = 15, InStock = 13, Available = true, BestSeller = false, Popular = false, Details = "Fresh strawberries juice." },
                new Product { Id = 15, Name = "Grape juice", ImageUrl = "suco_uva1.jpeg", CategoryId = 5, Price = 13, InStock = 10, Available = true, BestSeller = false, Popular = false, Details = "Natural and sugarless grape juice." },
                new Product { Id = 16, Name = "Water", ImageUrl = "agua_mineral1.jpeg", CategoryId = 4, Price = 5, InStock = 10, Available = true, BestSeller = false, Popular = false, Details = "Fresh mineral water." },
                new Product { Id = 17, Name = "Chocolate cookies", ImageUrl = "cookie1.jpeg", CategoryId = 6, Price = 8, InStock = 10, Available = true, BestSeller = false, Popular = true, Details = "Chocolate cookies with chocolate chips." },
                new Product { Id = 18, Name = "Vanilla cookies", ImageUrl = "cookie2.jpeg", CategoryId = 6, Price = 8, InStock = 13, Available = true, BestSeller = true, Popular = false, Details = "Crunchy and tasty vanilla cookies." },
                new Product { Id = 19, Name = "Swiss tort", ImageUrl = "torta_suica1.jpeg", CategoryId = 6, Price = 10, InStock = 10, Available = true, BestSeller = false, Popular = true, Details = "Swiss tort with filling." }
                );
        }
    }
}
