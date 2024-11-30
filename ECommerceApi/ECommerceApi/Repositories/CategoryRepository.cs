namespace ECommerceApi.Repositories
{
    using ECommerceApi.Context;
    using ECommerceApi.Entities;
    using Microsoft.EntityFrameworkCore;

    public class CategoryRepository : ICategoryRepository
    { 
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories() 
        {
            return await _context.Categories.AsNoTracking().ToListAsync();    
        }
    }
}
