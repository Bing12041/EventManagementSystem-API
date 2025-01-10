using EventManagementSystem.API.Data;

namespace EventManagementSystem.API.Repository;

public interface IEventRepository
{
    Task<Event> AddEvent(Event @event);
    Task<IEnumerable<Event>> GetAllEvents();
    Task<Event> GetEventById(int eventId);
    Task UpdateEvent(Event @event);
    Task DeleteEvent(Event @event);
    Task<IEnumerable<Event>> GetEventsByCategoryId(int categoryId);
    Task<IEnumerable<Event>> GetEventsByDate(DateTime date);
    Task<IEnumerable<Event>> GetEventsByLocationId(int locationId);
}