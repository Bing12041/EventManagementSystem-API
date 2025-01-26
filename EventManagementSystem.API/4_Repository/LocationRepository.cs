using EventManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.API.Repository;

public class LocationRepository : ILocationRepository
{
    private readonly EventDbContext _context;

    public LocationRepository(EventDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddLocation(Location location)
    {
        await _context.Locations.AddAsync(location);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteLocation(Location location)
    {
        var locationToDelete = await _context.Locations.FindAsync(location.LocationID);

        if (locationToDelete != null)
        {
            _context.Locations.Remove(locationToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Location> GetLocationById(int locationId)
    {
        var location = await _context.Locations.FindAsync(locationId);

        if (location == null)
        {
            throw new InvalidOperationException($"Location with id {locationId} not found.");
        }

        return location;
    }

    public async Task UpdateLocation(Location location)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Location>> GetAllLocations()
    {
        return await _context.Locations.ToListAsync();
    }
}