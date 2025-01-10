using EventManagementSystem.API.Data;

namespace EventManagementSystem.API.Repository;

public interface IUserRepository
{
    Task<User> AddUser(User user);
    Task<User> GetUserById(int userId);
    Task<User> GetUserByEmail(string email);
    Task<User> UpdateUser(User user);
    Task DeleteUser(User user);

}