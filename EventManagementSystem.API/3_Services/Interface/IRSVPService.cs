using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;

namespace EventManagementSystem.API.Service;

public interface IRSVPService
{
    Task<RSVP> RSVPToEvent(RSVPRequestDto rsvpRequest);
    Task<IEnumerable<RSVP>> GetRSVPsForEvent(int eventId);
    Task<IEnumerable<RSVP>> GetRSVPsForUser(int userId);
    Task<RSVP> UpdateRSVP(int userId, int eventId, string newStatus);
    Task CancelRSVP(int userId, int eventId);
}