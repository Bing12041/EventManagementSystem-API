using EventManagementSystem.API.Data;

namespace EventManagementSystem.API.Repository;

public interface ICategoryRepository{
    Task<IEnumerable<Category>> GetAllRepository();
    Task<Category> GetRepositoryById(int categoryId);
    Task<Category> AddCategory(Category category);
    Task UpdateCategory(Category category);
    Task DeleteCategoryById(int categoryId);
}