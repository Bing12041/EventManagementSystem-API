using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;

namespace EventManagementSystem.API.Service;

public interface IUserService
{
    Task<User> CreateUser(UserRegistrationDto userDto);
    Task<(string Token, User User)> AuthenticateUser(string email, string password);
    Task<User> GetUser(int userId);
    Task<User> UpdateUser(int userId, UserUpdateDto userDto);
    Task DeleteUser(int userId);
    Task<User> GetUserByEmail(string email);
}