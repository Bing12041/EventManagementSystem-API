using EventManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.API.Repository;

public class RSVPRepository : IRSVPRepository
{
    private readonly EventDbContext _context;

    public RSVPRepository(EventDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task AddRSVP(RSVP rsvp)
    {
        await _context.RSVPs.AddAsync(rsvp);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRSVP(RSVP rsvp)
    {
        _context.RSVPs.Remove(rsvp);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RSVP>> GetRSVPsByEventId(int eventId)
    {
        return await _context.RSVPs.Where(r => r.EventID == eventId).ToListAsync();
    }

    public async Task<IEnumerable<RSVP>> GetRSVPsByUserId(int userId)
    {
        return await _context.RSVPs.Where(r => r.UserID == userId).ToListAsync();
    }

    public async Task UpdateRSVP(RSVP rsvp)
    {
        _context.RSVPs.Update(rsvp);
        await _context.SaveChangesAsync();
    }
}