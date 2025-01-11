using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;

namespace EventManagementSystem.API.Service;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILocationRepository _locationRepository;

    public EventService(IEventRepository eventRepository,
                        IUserRepository userRepository,
                        ICategoryRepository categoryRepository,
                        ILocationRepository locationRepository)
    {
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
    }

    public async Task<Event> CreateEvent(EventDto eventDto)
    {
        var user = await _userRepository.GetUserById(eventDto.CreatedBy);
        if (user == null)
        {
            throw new ArgumentException($"User wit id {eventDto.CreatedBy} not exist.");
        }

        var category = await _categoryRepository.GetCategoryById(eventDto.CategoryID);
        if (category == null)
        {
            throw new ArgumentException($"Category with id {eventDto.CategoryID} not exist.");
        }

        var location = await _locationRepository.GetLocationById(eventDto.LocationID);
        if (location == null)
        {
            throw new ArgumentException($"Location with id {eventDto.LocationID} not exist.");
        }

        var newEvent = new Event
        {
            Title = eventDto.Title,
            Description = eventDto.Description,
            StartDate = eventDto.StartDate,
            EndDate = eventDto.EndDate,
            CreatedBy = eventDto.CreatedBy,
            CategoryID = eventDto.CategoryID,
            LocationID = eventDto.LocationID
        };

        return await _eventRepository.AddEvent(newEvent);
    }

    public async Task DeleteEvent(int eventId)
    {
        var eventToDelete = await _eventRepository.GetEventById(eventId);

        if (eventToDelete == null)
        {
            throw new ArgumentException($"Event with id {eventId} not exist.");
        }

        await _eventRepository.DeleteEvent(eventToDelete);
    }

    public async Task<IEnumerable<Event>> GetAllEvents()
    {
        return await _eventRepository.GetAllEvents();
    }

    public async Task<Event> GetEvent(int eventId)
    {
        var eventFound = await _eventRepository.GetEventById(eventId);

        if (eventFound == null)
        {
            throw new ArgumentException($"Event with id {eventId} not found.");
        }
        
        return eventFound;
    }

    public async Task<IEnumerable<Event>> GetEventsByCategory(int categoryId)
    {
        return await _eventRepository.GetEventsByCategoryId(categoryId);
    }

    public async Task<IEnumerable<Event>> GetEventsByDate(DateTime date)
    {
        return await _eventRepository.GetEventsByDate(date);
    }

    public async Task<IEnumerable<Event>> GetEventsByLocation(int locationId)
    {
        return await _eventRepository.GetEventsByLocationId(locationId);
    }

    public async Task<Event> UpdateEvent(int eventId, EventDto eventDto)
    {
        var existingEvent = await _eventRepository.GetEventById(eventId);
        if (existingEvent == null)
        {
            throw new ArgumentException($"Event with id {eventId} not exist.");
        }

        if (eventDto.CreatedBy != existingEvent.CreatedBy)
        {
            throw new ArgumentException($"User with Id {eventDto.CreatedBy} not found.");
        }

        if (eventDto.CategoryID != existingEvent.CategoryID)
        {
            throw new ArgumentException($"Category with Id {eventDto.CategoryID} not found.");
        }

        if (eventDto.LocationID != existingEvent.LocationID)
        {
            throw new ArgumentException($"Location with Id {eventDto.LocationID} not found.");
        }

        existingEvent.Title = eventDto.Title;
        existingEvent.Description = eventDto.Description;
        existingEvent.StartDate = eventDto.StartDate;
        existingEvent.EndDate = eventDto.EndDate;
        existingEvent.CreatedBy = eventDto.CreatedBy;
        existingEvent.CategoryID = eventDto.CategoryID;
        existingEvent.LocationID = eventDto.LocationID;

        await _eventRepository.UpdateEvent(existingEvent);

        return existingEvent;
    }
}