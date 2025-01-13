using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        try
        {
            var category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
    {
        try
        {
            var category = await _categoryService.CreateCategory(categoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryID }, category);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
    {
        try
        {
            var updatedCategory = await _categoryService.UpdateCategory(id, categoryDto);
            return Ok(updatedCategory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
        }
    }
}
