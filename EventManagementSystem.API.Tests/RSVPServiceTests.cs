using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;
using EventManagementSystem.API.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EventManagementSystem.API.Tests.Service
{
    public class RSVPServiceTests
    {
        private readonly Mock<IRSVPRepository> _mockRSVPRepository;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IRSVPService _rsvpService;

        public RSVPServiceTests()
        {
            _mockRSVPRepository = new Mock<IRSVPRepository>();
            _mockEventRepository = new Mock<IEventRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _rsvpService = new RSVPService(
                _mockRSVPRepository.Object,
                _mockEventRepository.Object,
                _mockUserRepository.Object
            );
        }

        [Fact]
        public async Task RSVPToEvent_ValidInput_ReturnsRSVP()
        {
            // Arrange
            var rsvpRequest = new RSVPRequestDto { UserID = 1, EventID = 1, Status = "Attending" };
            var expectedRSVP = new RSVP { UserID = rsvpRequest.UserID, EventID = rsvpRequest.EventID, Status = rsvpRequest.Status };

            // Mock the event repository to return a valid event
            _mockEventRepository.Setup(r => r.GetEventById(rsvpRequest.EventID))
                .ReturnsAsync(new Event { EventID = rsvpRequest.EventID });

            // Mock the user repository to return a valid user
            _mockUserRepository.Setup(r => r.GetUserById(rsvpRequest.UserID))
                .ReturnsAsync(new User { UserID = rsvpRequest.UserID });

            // Since AddRSVP returns Task, not Task<RSVP>, we'll use a callback to capture the added RSVP
            RSVP addedRSVP = null;
            _mockRSVPRepository.Setup(r => r.AddRSVP(It.IsAny<RSVP>()))
                .Returns(Task.CompletedTask)
                .Callback<RSVP>(rsvp => addedRSVP = rsvp);

            // Act
            var result = await _rsvpService.RSVPToEvent(rsvpRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRSVP.UserID, result.UserID);
            Assert.Equal(expectedRSVP.EventID, result.EventID);
            Assert.Equal(expectedRSVP.Status, result.Status);
            _mockRSVPRepository.Verify(r => r.AddRSVP(It.Is<RSVP>(rsvp =>
                rsvp.UserID == rsvpRequest.UserID &&
                rsvp.EventID == rsvpRequest.EventID &&
                rsvp.Status == rsvpRequest.Status)), Times.Once);

            // Additional assertion to check if the callback captured the correct RSVP
            Assert.NotNull(addedRSVP);
            Assert.Equal(expectedRSVP.UserID, addedRSVP.UserID);
            Assert.Equal(expectedRSVP.EventID, addedRSVP.EventID);
            Assert.Equal(expectedRSVP.Status, addedRSVP.Status);
        }

        [Fact]
        public async Task GetRSVPsForEvent_ValidEventId_ReturnsRSVPs()
        {
            // Arrange
            var eventId = 1;
            var rsvps = new List<RSVP>
    {
        new RSVP { UserID = 1, EventID = eventId, Status = "Attending" },
        new RSVP { UserID = 2, EventID = eventId, Status = "Maybe" }
    }.AsQueryable();

            // Mock the event repository to return a valid event
            _mockEventRepository.Setup(r => r.GetEventById(eventId))
                .ReturnsAsync(new Event { EventID = eventId });

            _mockRSVPRepository.Setup(r => r.GetRSVPsByEventId(eventId))
                .ReturnsAsync(rsvps);

            // Act
            var result = await _rsvpService.GetRSVPsForEvent(eventId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Equal(eventId, r.EventID));
        }

        [Fact]
        public async Task GetRSVPsForUser_ValidUserId_ReturnsRSVPs()
        {
            // Arrange
            var userId = 1;
            var rsvps = new List<RSVP>
            {
                new RSVP { UserID = userId, EventID = 1, Status = "Attending" },
                new RSVP { UserID = userId, EventID = 2, Status = "Maybe" }
            }.AsQueryable();

            _mockRSVPRepository.Setup(r => r.GetRSVPsByUserId(userId))
                .ReturnsAsync(rsvps);

            // Act
            var result = await _rsvpService.GetRSVPsForUser(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Equal(userId, r.UserID));
        }

        [Fact]
        public async Task UpdateRSVP_ValidInput_UpdatesRSVP()
        {
            // Arrange
            var userId = 1;
            var eventId = 1;
            var newStatus = "Not Attending";
            var existingRSVP = new RSVP { UserID = userId, EventID = eventId, Status = "Attending" };

            _mockRSVPRepository.Setup(r => r.GetRSVPsByEventId(eventId))
                .ReturnsAsync(new List<RSVP> { existingRSVP }.AsQueryable());

            _mockRSVPRepository.Setup(r => r.UpdateRSVP(It.IsAny<RSVP>()))
                .Returns(Task.CompletedTask); // Use this for methods returning Task

            // Act
            var result = await _rsvpService.UpdateRSVP(userId, eventId, newStatus);

            // Assert
            Assert.Equal(newStatus, result.Status);
            _mockRSVPRepository.Verify(r => r.GetRSVPsByEventId(eventId), Times.Once);
            _mockRSVPRepository.Verify(r => r.UpdateRSVP(It.Is<RSVP>(rsvp =>
                rsvp.UserID == userId &&
                rsvp.EventID == eventId &&
                rsvp.Status == newStatus)), Times.Once);
        }

        [Fact]
        public async Task CancelRSVP_ValidInput_CancelsRSVP()
        {
            // Arrange
            var userId = 1;
            var eventId = 1;
            var rsvpToCancel = new RSVP { UserID = userId, EventID = eventId, Status = "Attending" };

            _mockRSVPRepository.Setup(r => r.GetRSVPsByEventId(eventId))
                .ReturnsAsync(new List<RSVP> { rsvpToCancel }.AsQueryable());

            _mockRSVPRepository.Setup(r => r.DeleteRSVP(rsvpToCancel))
                .Returns(Task.CompletedTask); // Use this for methods returning Task

            // Act
            await _rsvpService.CancelRSVP(userId, eventId);

            // Assert
            _mockRSVPRepository.Verify(r => r.GetRSVPsByEventId(eventId), Times.Once);
            _mockRSVPRepository.Verify(r => r.DeleteRSVP(It.Is<RSVP>(rsvp =>
                rsvp.UserID == userId &&
                rsvp.EventID == eventId)), Times.Once);
        }
    }
}