using EventManagementSystem.API.Data;

namespace EventManagementSystem.API.Repository;

public interface ILocationRepository{
    Task<Location> GetLocationById(int locationId);
    Task AddLocation(Location location);
    Task UpdateLocation(Location location);
    Task DeleteLocation(Location location);
}