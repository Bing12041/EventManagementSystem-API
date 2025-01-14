using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;
using EventManagementSystem.API.Service;
using Moq;

namespace EventManagementSystem.API.Tests.Service
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly ICategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { CategoryID = 1, Name = "Category 1" },
                new Category { CategoryID = 2, Name = "Category 2" }
            };

            _mockRepository.Setup(r => r.GetAllCategory())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategories();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCategory_ValidId_ReturnsCategory()
        {
            // Arrange
            var categoryId = 1;
            var expectedCategory = new Category { CategoryID = categoryId, Name = "Test Category" };

            _mockRepository.Setup(r => r.GetCategoryById(categoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _categoryService.GetCategory(categoryId);

            // Assert
            Assert.Equal(expectedCategory.CategoryID, result.CategoryID);
            Assert.Equal(expectedCategory.Name, result.Name);
        }

        [Fact]
        public async Task GetCategory_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var categoryId = 1;
            _mockRepository.Setup(r => r.GetCategoryById(categoryId))
                .ReturnsAsync((Category)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _categoryService.GetCategory(categoryId));
        }

        [Fact]
        public async Task CreateCategory_ValidInput_ReturnsCategory()
        {
            // Arrange
            var categoryDto = new CategoryDto { Name = "New Category" };
            var expectedCategory = new Category { CategoryID = 1, Name = categoryDto.Name };

            _mockRepository.Setup(r => r.AddCategory(It.IsAny<Category>()))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _categoryService.CreateCategory(categoryDto);

            // Assert
            Assert.Equal(expectedCategory.Name, result.Name);
            _mockRepository.Verify(r => r.AddCategory(It.Is<Category>(c => c.Name == categoryDto.Name)), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_ValidInput_UpdatesCategory()
        {
            // Arrange
            var categoryId = 1;
            var categoryDto = new CategoryDto { Name = "Updated Name" };
            var existingCategory = new Category { CategoryID = categoryId, Name = "Old Name" };

            _mockRepository.Setup(r => r.GetCategoryById(categoryId))
                .ReturnsAsync(existingCategory);

            _mockRepository.Setup(r => r.UpdateCategory(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _categoryService.UpdateCategory(categoryId, categoryDto);

            // Assert
            Assert.Equal(categoryDto.Name, result.Name);
            _mockRepository.Verify(r => r.GetCategoryById(categoryId), Times.Once);
            _mockRepository.Verify(r => r.UpdateCategory(It.Is<Category>(c => 
                c.CategoryID == categoryId && 
                c.Name == categoryDto.Name)), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_ValidId_DeletesCategory()
        {
            // Arrange
            var categoryId = 1;

            _mockRepository.Setup(r => r.GetCategoryById(categoryId))
                .ReturnsAsync(new Category { CategoryID = categoryId });

            _mockRepository.Setup(r => r.DeleteCategoryById(categoryId))
                .Returns(Task.CompletedTask);

            // Act & Assert
            await _categoryService.DeleteCategory(categoryId);

            _mockRepository.Verify(r => r.GetCategoryById(categoryId), Times.Once);
            _mockRepository.Verify(r => r.DeleteCategoryById(categoryId), Times.Once);
        }
    }
}