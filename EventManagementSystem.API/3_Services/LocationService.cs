using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;

namespace EventManagementSystem.API.Service;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
    }

    public async Task<Location> CreateLocation(LocationDto locationDto)
    {
        var location = new Location
        {
            Address = locationDto.Address,
            City = locationDto.City,
            State = locationDto.State,
            Country = locationDto.Country
        };

        await _locationRepository.AddLocation(location);
        return location;
    }

    public async Task DeleteLocation(int locationId)
    {
        var location = await _locationRepository.GetLocationById(locationId);

        if (location == null)
        {
            throw new ArgumentException($"Location with id {locationId} not found.");
        }

        await _locationRepository.DeleteLocation(location);
    }

    public async Task<Location> GetLocation(int locationId)
    {
        var location = await _locationRepository.GetLocationById(locationId);

        if (location == null)
        {
            throw new ArgumentException($"Location with id {locationId} not found.");
        }

        return location;
    }

    public async Task<Location> UpdateLocation(int locationId, LocationDto locationDto)
    {
        var location = await _locationRepository.GetLocationById(locationId);

        if (location == null)
        {
            throw new ArgumentException($"Location with id {locationId} not found.");
        }

        location.Address = locationDto.Address;
        location.City = locationDto.City;
        location.State = locationDto.State;
        location.Country = locationDto.Country;

        await _locationRepository.UpdateLocation(location);
        return location;
    }

    public async Task<IEnumerable<Location>> GetAllLocations()
    {
        return await _locationRepository.GetAllLocations();
    }
}