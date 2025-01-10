using EventManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.API.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly EventDbContext _context;

    public CategoryRepository(EventDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Category> AddCategory(Category category)
    {
        var categoryAdded = await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return categoryAdded.Entity;
    }

    public async Task DeleteCategoryById(int categoryId)
    {
        var categoryToDelete = await _context.Categories.FindAsync(categoryId);

        if (categoryToDelete != null)
        {
            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Category>> GetAllCategory()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryById(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);

        if (category == null)
        {
            throw new InvalidOperationException($"Category with id {categoryId} not found.");
        }

        return category;
    }

    public async Task UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }
}