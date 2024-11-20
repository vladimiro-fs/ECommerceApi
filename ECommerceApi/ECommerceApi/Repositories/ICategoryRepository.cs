namespace ECommerceApi.Repositories
{
    using ECommerceApi.Entities;

    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
    }
}
