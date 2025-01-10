using EventManagementSystem.API.Data;

namespace EventManagementSystem.API.Repository;

public interface IEventRepository
{
    Task<Event> AddEvent(Event e);
    Task<IEnumerable<Event>> GetAllEvents();
    Task<Event> GetEventById(int eventId);
    Task UpdateEvent(Event e);
    Task DeleteEvent(Event e);
    Task<IEnumerable<Event>> GetEventsByCategoryId(int categoryId);
    Task<IEnumerable<Event>> GetEventsByDate(DateTime date);
    Task<IEnumerable<Event>> GetEventsByLocationId(int locationId);
}