using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;
using EventManagementSystem.API.Service;
using Moq;

namespace EventManagementSystem.API.Tests.Service
{
    public class LocationServiceTests
    {
        private readonly Mock<ILocationRepository> _mockRepository;
        private readonly ILocationService _locationService;

        public LocationServiceTests()
        {
            _mockRepository = new Mock<ILocationRepository>();
            _locationService = new LocationService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateLocation_ValidInput_ReturnsLocation()
        {
            // Arrange
            var locationDto = new LocationDto
            {
                Address = "123 Test St",
                City = "Test City",
                State = "TS",
                Country = "Test Country"
            };
            var expectedLocation = new Location
            {
                LocationID = 1,
                Address = locationDto.Address,
                City = locationDto.City,
                State = locationDto.State,
                Country = locationDto.Country
            };

            // Since AddLocation returns Task, not Task<Location>, we need to setup differently
            _mockRepository.Setup(r => r.AddLocation(It.IsAny<Location>()))
                .Returns(Task.CompletedTask);

            // If we need to return the added location, we'll have to do it outside the setup
            Location addedLocation = null;
            _mockRepository.Setup(r => r.AddLocation(It.IsAny<Location>()))
                .Callback<Location>(l => addedLocation = l);

            // Act
            var result = await _locationService.CreateLocation(locationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLocation.Address, result.Address);
            Assert.Equal(expectedLocation.City, result.City);
            Assert.Equal(expectedLocation.State, result.State);
            Assert.Equal(expectedLocation.Country, result.Country);

            // Verify that AddLocation was called with the expected location
            _mockRepository.Verify(r => r.AddLocation(It.Is<Location>(l =>
                l.Address == locationDto.Address &&
                l.City == locationDto.City &&
                l.State == locationDto.State &&
                l.Country == locationDto.Country)), Times.Once);
        }

        [Fact]
        public async Task GetLocation_ValidId_ReturnsLocation()
        {
            // Arrange
            var locationId = 1;
            var expectedLocation = new Location { LocationID = locationId, Address = "Some Address" };

            _mockRepository.Setup(r => r.GetLocationById(locationId))
                .ReturnsAsync(expectedLocation);

            // Act
            var result = await _locationService.GetLocation(locationId);

            // Assert
            Assert.Equal(expectedLocation.LocationID, result.LocationID);
            Assert.Equal(expectedLocation.Address, result.Address);
            _mockRepository.Verify(r => r.GetLocationById(locationId), Times.Once);
        }

        [Fact]
        public async Task GetLocation_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var locationId = 1;
            _mockRepository.Setup(r => r.GetLocationById(locationId))
                .ReturnsAsync((Location)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _locationService.GetLocation(locationId));

            _mockRepository.Verify(r => r.GetLocationById(locationId), Times.Once);
        }

        [Fact]
        public async Task UpdateLocation_ValidInput_UpdatesLocation()
        {
            // Arrange
            var locationId = 1;
            var locationDto = new LocationDto
            {
                Address = "New Address",
                City = "New City",
                State = "NC",
                Country = "New Country"
            };

            var existingLocation = new Location { LocationID = locationId, Address = "Old Address" };
            _mockRepository.Setup(r => r.GetLocationById(locationId))
                .ReturnsAsync(existingLocation);

            _mockRepository.Setup(r => r.UpdateLocation(It.IsAny<Location>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _locationService.UpdateLocation(locationId, locationDto);

            // Assert
            Assert.Equal(locationDto.Address, result.Address);
            Assert.Equal(locationDto.City, result.City);
            Assert.Equal(locationDto.State, result.State);
            Assert.Equal(locationDto.Country, result.Country);
            _mockRepository.Verify(r => r.GetLocationById(locationId), Times.Once);
            _mockRepository.Verify(r => r.UpdateLocation(It.Is<Location>(l =>
                l.LocationID == locationId &&
                l.Address == locationDto.Address &&
                l.City == locationDto.City &&
                l.State == locationDto.State &&
                l.Country == locationDto.Country)), Times.Once);
        }

        [Fact]
        public async Task DeleteLocation_ValidId_DeletesLocation()
        {
            // Arrange
            var locationId = 1;
            var locationToDelete = new Location { LocationID = locationId };

            _mockRepository.Setup(r => r.GetLocationById(locationId))
                .ReturnsAsync(locationToDelete);

            _mockRepository.Setup(r => r.DeleteLocation(locationToDelete))
                .Returns(Task.CompletedTask);

            // Act
            await _locationService.DeleteLocation(locationId);

            // Assert
            _mockRepository.Verify(r => r.GetLocationById(locationId), Times.Once);
            _mockRepository.Verify(r => r.DeleteLocation(It.Is<Location>(l => l.LocationID == locationId)), Times.Once);
        }
    }
}