namespace ECommerceApi.Repositories
{
    using ECommerceApi.Context;
    using ECommerceApi.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId) 
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetPopularProductsAsync() 
        { 
            return await _context.Products
                .Where(p => p.Popular)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBestSellerProductsAsync() 
        { 
            return await _context.Products
                .Where (p => p.BestSeller)
                .ToListAsync();
        }

        public async Task<Product> GetProductDetailsAsync(int id) 
        {
            var productDetails = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productDetails == null) 
            {
                throw new InvalidOperationException();
            }

            return productDetails;
        }
    }
}
