using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;

namespace EventManagementSystem.API.Service;

public class CategoryService : ICategoryService
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<Category> CreateCategory(CategoryDto categoryDto)
    {
        var category = new Category { Name = categoryDto.Name };
        await _categoryRepository.AddCategory(category);
        return category;
    }

    public async Task DeleteCategory(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryById(categoryId);

        if (category == null)
        {
            throw new ArgumentException($"Category with id {categoryId} not found.");
        }

        await _categoryRepository.DeleteCategoryById(categoryId);
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _categoryRepository.GetAllCategory();
    }

    public async Task<Category> GetCategory(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryById(categoryId);

        if (category == null)
        {
            throw new ArgumentException($"Category with id {categoryId} not found.");
        }

        return category;
    }

    public async Task<Category> UpdateCategory(int categoryId, CategoryDto categoryDto)
    {
        var existingCategory = await _categoryRepository.GetCategoryById(categoryId);

        if (existingCategory == null)
        {
            throw new ArgumentException($"Category with id {categoryId} not found.");
        }

        existingCategory.Name = categoryDto.Name;
        await _categoryRepository.UpdateCategory(existingCategory);
        return existingCategory;
    }
}