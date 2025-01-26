using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;

namespace EventManagementSystem.API.Service;

public interface ILocationService
{
    Task<Location> CreateLocation(LocationDto locationDto);
    Task<Location> GetLocation(int locationId);
    Task<Location> UpdateLocation(int locationId, LocationDto locationDto);
    Task DeleteLocation(int locationId);
    Task<IEnumerable<Location>> GetAllLocations();
}