using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;
using EventManagementSystem.API.Service;
using Moq;
using Microsoft.Extensions.Configuration;

namespace EventManagementSystem.API.Tests.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _userService = new UserService(_mockRepository.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task CreateUser_ValidInput_ReturnsUser()
        {
            // Arrange
            var userDto = new UserRegistrationDto { Username = "testuser", Email = "test@example.com", Password = "password", Name = "Test User" };
            var expectedUser = new User { UserID = 1, Username = "testuser", Email = "test@example.com", Name = "Test User", PasswordHash = "hashedPassword" };

            _mockRepository.Setup(r => r.AddUser(It.IsAny<User>()))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.CreateUser(userDto);

            // Assert
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.Email, result.Email);
            Assert.Equal(expectedUser.Name, result.Name);
            _mockRepository.Verify(r => r.AddUser(It.Is<User>(u =>
                u.Username == userDto.Username &&
                u.Email == userDto.Email &&
                u.Name == userDto.Name)), Times.Once);
        }

        [Fact]
        public async Task CreateUser_DuplicateEmail_ThrowsArgumentException()
        {
            // Arrange
            var userDto = new UserRegistrationDto { Username = "testuser", Email = "test@example.com", Password = "password", Name = "Test User" };
            _mockRepository.Setup(r => r.GetUserByEmail(userDto.Email))
                .ReturnsAsync(new User { Email = userDto.Email });

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUser(userDto));
        }

        [Fact]
        public async Task AuthenticateUser_InvalidCredentials_ThrowsArgumentException()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongPassword";
            _mockRepository.Setup(r => r.GetUserByEmail(email))
                .ReturnsAsync(new User { Email = email, PasswordHash = "hashedPassword" });

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AuthenticateUser(email, password));
        }

        [Fact]
        public async Task GetUser_ValidId_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { UserID = userId, Username = "testuser" };

            _mockRepository.Setup(r => r.GetUserById(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUser(userId);

            // Assert
            Assert.Equal(expectedUser.UserID, result.UserID);
            Assert.Equal(expectedUser.Username, result.Username);
        }

        [Fact]
        public async Task GetUser_InvalidId_ReturnsNull()
        {
            // Arrange
            var userId = 1;
            _mockRepository.Setup(r => r.GetUserById(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUser(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUser_ValidInput_UpdatesUser()
        {
            // Arrange
            var userId = 1;
            var userUpdateDto = new UserUpdateDto { Username = "newUsername", Email = "newEmail@example.com", Name = "New Name" };
            var existingUser = new User { UserID = userId, Username = "oldUsername", Email = "oldEmail@example.com", Name = "Old Name" };

            _mockRepository.Setup(r => r.GetUserById(userId))
                .ReturnsAsync(existingUser);

            _mockRepository.Setup(r => r.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.UpdateUser(userId, userUpdateDto);

            // Assert
            Assert.Equal(userUpdateDto.Username, result.Username);
            Assert.Equal(userUpdateDto.Email, result.Email);
            Assert.Equal(userUpdateDto.Name, result.Name);
            _mockRepository.Verify(r => r.GetUserById(userId), Times.Once);
            _mockRepository.Verify(r => r.UpdateUser(It.Is<User>(u =>
                u.UserID == userId &&
                u.Username == userUpdateDto.Username &&
                u.Email == userUpdateDto.Email &&
                u.Name == userUpdateDto.Name)), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ValidId_DeletesUser()
        {
            // Arrange
            var userId = 1;
            var userToDelete = new User { UserID = userId };

            _mockRepository.Setup(r => r.GetUserById(userId))
                .ReturnsAsync(userToDelete);

            _mockRepository.Setup(r => r.DeleteUser(userToDelete))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUser(userId);

            // Assert
            _mockRepository.Verify(r => r.GetUserById(userId), Times.Once);
            _mockRepository.Verify(r => r.DeleteUser(userToDelete), Times.Once);
        }
    }
}