using EventManagementSystem.API.Data;
using EventManagementSystem.API.Repository;
using EventManagementSystem.API.Service;
using Moq;

namespace EventManagementSystem.API.Tests.Service
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly IEventService _eventService;

        public EventServiceTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockLocationRepository = new Mock<ILocationRepository>();

            _eventService = new EventService(
                _mockEventRepository.Object, 
                _mockUserRepository.Object, 
                _mockCategoryRepository.Object, 
                _mockLocationRepository.Object
            );
        }

        [Fact]
        public async Task GetEventsByCategory_ValidCategoryId_ReturnsEvents()
        {
            // Arrange
            var categoryId = 1;
            var events = new List<Event>
            {
                new Event { EventID = 1, Title = "Event 1", CategoryID = categoryId },
                new Event { EventID = 2, Title = "Event 2", CategoryID = categoryId }
            }.AsQueryable();

            _mockEventRepository.Setup(r => r.GetEventsByCategoryId(categoryId))
                .ReturnsAsync(events);

            // Act
            var result = await _eventService.GetEventsByCategory(categoryId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, e => Assert.Equal(categoryId, e.CategoryID));
        }

        [Fact]
        public async Task GetEventsByDate_ValidDate_ReturnsEvents()
        {
            // Arrange
            var date = DateTime.Today;
            var events = new List<Event>
            {
                new Event { EventID = 1, Title = "Event Today", StartDate = date },
                new Event { EventID = 2, Title = "Another Event Today", EndDate = date }
            }.AsQueryable();

            _mockEventRepository.Setup(r => r.GetEventsByDate(date))
                .ReturnsAsync(events);

            // Act
            var result = await _eventService.GetEventsByDate(date);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, e => 
                Assert.True(e.StartDate.Date == date.Date || e.EndDate.Date == date.Date));
        }

        [Fact]
        public async Task GetEventsByLocation_ValidLocationId_ReturnsEvents()
        {
            // Arrange
            var locationId = 1;
            var events = new List<Event>
            {
                new Event { EventID = 1, Title = "Event at Location 1", LocationID = locationId },
                new Event { EventID = 2, Title = "Another Event at Location 1", LocationID = locationId }
            }.AsQueryable();

            _mockEventRepository.Setup(r => r.GetEventsByLocationId(locationId))
                .ReturnsAsync(events);

            // Act
            var result = await _eventService.GetEventsByLocation(locationId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, e => Assert.Equal(locationId, e.LocationID));
        }
    }
}