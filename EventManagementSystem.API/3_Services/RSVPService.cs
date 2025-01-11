using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;

namespace EventManagementSystem.API.Service;

public class RSVPService : IRSVPService
{
    private readonly IRSVPRepository _rsvpRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;

    public RSVPService(IRSVPRepository rsvpRepository,
                        IEventRepository eventRepository,
                        IUserRepository userRepository)
    {
        _rsvpRepository = rsvpRepository ?? throw new ArgumentNullException(nameof(rsvpRepository));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task CancelRSVP(int userId, int eventId)
    {
        var rsvps = await _rsvpRepository.GetRSVPsByEventId(eventId);
        var rsvp = rsvps.FirstOrDefault(r => r.UserID == userId);

        if (rsvp == null)
        {
            throw new ArgumentException($"RSVP associated with user with id {userId} not found.");
        }

        await _rsvpRepository.DeleteRSVP(rsvp);
    }

    public async Task<IEnumerable<RSVP>> GetRSVPsForEvent(int eventId)
    {
        var e = await _eventRepository.GetEventById(eventId);

        if (e == null)
        {
            throw new ArgumentException($"Event with id {eventId} not found.");
        }

        return await _rsvpRepository.GetRSVPsByEventId(eventId);
    }

    public async Task<IEnumerable<RSVP>> GetRSVPsForUser(int userId)
    {
        var user = _userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} no found.");
        }

        return await _rsvpRepository.GetRSVPsByUserId(userId);
    }

    public async Task<RSVP> RSVPToEvent(RSVPRequestDto rsvpRequest)
    {
        var e = await _eventRepository.GetEventById(rsvpRequest.EventID);
        if (e == null)
        {
            throw new ArgumentException($"Event with id {rsvpRequest.EventID} not found.");
        }

        var user = await _userRepository.GetUserById(rsvpRequest.UserID);
        if (user == null)
        {
            throw new ArgumentException($"User with id {rsvpRequest.UserID} not found.");
        }

        var rsvps = await _rsvpRepository.GetRSVPsByEventId(rsvpRequest.EventID);
        var existingRSVP = rsvps.FirstOrDefault(r => r.UserID == rsvpRequest.UserID);

        if (existingRSVP != null)
        {
            existingRSVP.Status = rsvpRequest.Status;
            await _rsvpRepository.UpdateRSVP(existingRSVP);
            return existingRSVP;
        }
        else
        {
            var rsvp = new RSVP
            {
                UserID = rsvpRequest.UserID,
                EventID = rsvpRequest.EventID,
                Status = rsvpRequest.Status
            };

            await _rsvpRepository.AddRSVP(rsvp);
            return rsvp;
        }
    }

    public async Task<RSVP> UpdateRSVP(int userId, int eventId, string newStatus)
    {
        var rsvps = await _rsvpRepository.GetRSVPsByEventId(eventId);
        var rsvp = rsvps.FirstOrDefault(r => r.UserID == userId);

        if (rsvp == null)
        {
            throw new ArgumentException($"RSVP associated to user with id {userId} not found.");
        }

        rsvp.Status = newStatus;
        await _rsvpRepository.UpdateRSVP(rsvp);
        return rsvp;
    }
}