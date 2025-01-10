using EventManagementSystem.API.Data;

namespace EventManagementSystem.API.Repository;

public interface IRSVPRepository{
    Task AddRSVP(RSVP rsvp);
    Task<IEnumerable<RSVP>> GetRSVPsByEventId(int eventId);
    Task<IEnumerable<RSVP>> GetRSVPsByUserId(int userId);
    Task UpdateRSVP(RSVP rsvp);
    Task DeleteRSVP(RSVP rsvp);
}