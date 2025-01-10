using EventManagementSystem.API.Data;

namespace EventManagementSystem.API.Repository;

public class CategoryRepository : ICategoryRepository
{
    public Task<Category> AddCategory(Category category)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategoryById(int categoryId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Category>> GetAllRepository()
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetRepositoryById(int categoryId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCategory(Category category)
    {
        throw new NotImplementedException();
    }
}