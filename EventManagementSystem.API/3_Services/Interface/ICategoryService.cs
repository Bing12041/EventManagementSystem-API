using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;

namespace EventManagementSystem.API.Service;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategories();
    Task<Category> GetCategory(int categoryId);
    Task<Category> CreateCategory(CategoryDto categoryDto);
    Task<Category> UpdateCategory(int categoryId, CategoryDto categoryDto);
    Task DeleteCategory(int categoryId);
}