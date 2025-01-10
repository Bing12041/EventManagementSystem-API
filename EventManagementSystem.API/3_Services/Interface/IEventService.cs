using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;

namespace EventManagementSystem.API.Service;

public interface IEventService
{
    Task<Event> CreateEvent(EventDto eventDto);
    Task<IEnumerable<Event>> GetAllEvents();
    Task<Event> GetEvent(int eventId);
    Task<Event> UpdateEvent(int eventId, EventDto eventDto);
    Task DeleteEvent(int eventId);
    Task<IEnumerable<Event>> GetEventsByCategory(int categoryId);
    Task<IEnumerable<Event>> GetEventsByDate(DateTime date);
    Task<IEnumerable<Event>> GetEventsByLocation(int locationId);
}