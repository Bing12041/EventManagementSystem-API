using EventManagementSystem.API.Data;

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
        var useradded = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return useradded.Entity;
    }

    public async Task DeleteUser(User user)
    {
        var userToDelete = await _context.Users.FindAsync(user.UserID);

        if (userToDelete != null)
        {
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public Task<User> GetUserByEmail(string email)
    {
        throw new NotImplementedException();
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
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}