using EventManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.API.Repository;

public class EventRepository : IEventRepository
{
    private readonly EventDbContext _context;

    public EventRepository(EventDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Event> AddEvent(Event e)
    {
        await _context.Events.AddAsync(e);
        await _context.SaveChangesAsync();
        return e;
    }

    public async Task DeleteEvent(Event e)
    {
        var eventFound = await _context.Events.FindAsync(e.EventID);

        if (eventFound != null)
        {
            _context.Events.Remove(eventFound);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Event>> GetAllEvents()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<Event> GetEventById(int eventId)
    {
        var eventFound = await _context.Events.FindAsync(eventId);

        if (eventFound == null)
        {
            throw new InvalidOperationException($"Event with id {eventId} not found.");
        }

        return eventFound;
    }

    public async Task<IEnumerable<Event>> GetEventsByCategoryId(int categoryId)
    {
        return await _context.Events.Where(e => e.CategoryID == categoryId).ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByDate(DateTime date)
    {
        return await _context.Events.Where(e => e.StartDate == date || e.EndDate == date).ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByLocationId(int locationId)
    {
        return await _context.Events.Where(e => e.LocationID == locationId).ToListAsync();
    }

    public async Task UpdateEvent(Event e)
    {
        _context.Events.Update(e);
        await _context.SaveChangesAsync();
    }
}