using System.Security.Cryptography;
using System.Text;
using EventManagementSystem.API.Data;
using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Repository;

namespace EventManagementSystem.API.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<string> AuthenticateUser(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);

        if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
        {
            throw new ArgumentException("Invalid email or password");
        }

        // You should generate and return a JWT token here, not just "Success".
        return "Success"; // TODO: Replace with token generation
    }

    public async Task<User> CreateUser(UserRegistrationDto userDto)
    {
        var existingUser = await _userRepository.GetUserByEmail(userDto.Email);

        if (existingUser != null)
        {
            throw new ArgumentException($"User with email {userDto.Email} already exists.");
        }

        var passwordHash = HashPassword(userDto.Password);

        var user = new User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = passwordHash,
            Name = userDto.Name
        };

        return await _userRepository.AddUser(user);
    }

    public async Task DeleteUser(int userId)
    {
        var user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} not found.");
        }

        await _userRepository.DeleteUser(user);
    }

    public async Task<User> GetUser(int userId)
    {
        var user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} does not exist.");
        }

        return user;
    }

    public async Task<User> UpdateUser(int userId, UserUpdateDto userDto)
    {
        var user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} does not exist.");
        }

        if (!string.IsNullOrEmpty(userDto.Username))
        {
            user.Username = userDto.Username;
        }

        if (!string.IsNullOrEmpty(userDto.Email))
        {
            var existingUser = await _userRepository.GetUserByEmail(userDto.Email);

            if (existingUser != null && existingUser.UserID != userId)
            {
                throw new ArgumentException("Email already taken.");
            }

            user.Email = userDto.Email;
        }

        if (!string.IsNullOrEmpty(userDto.Name))
        {
            user.Name = userDto.Name;
        }

        return await _userRepository.UpdateUser(user);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        var hashOfInput = HashPassword(password);

        return string.Equals(hashOfInput, storedHash, StringComparison.OrdinalIgnoreCase);
    }
}