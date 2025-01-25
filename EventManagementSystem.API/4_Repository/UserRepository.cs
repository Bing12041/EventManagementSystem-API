using EventManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.API.Repository;

public class UserRepository : IUserRepository
{
    private readonly EventDbContext _context;

    public UserRepository(EventDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User> AddUser(User user)
    {
        try
        {
            var userAdded = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return userAdded.Entity;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database update error: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteUser(User user)
    {
        try
        {
            var userToDelete = await _context.Users.FindAsync(user.UserID);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database delete error: {ex.Message}");
            throw;
        }
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetUserById(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new InvalidOperationException($"User with Id {userId} not found.");
        }

        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database update error: {ex.Message}");
            throw;
        }
    }
}