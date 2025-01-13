using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.API.Service;
using EventManagementSystem.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userDto)
        {
            try
            {
                var user = await _userService.CreateUser(userDto);
                return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var token = await _userService.AuthenticateUser(loginDto.Email, loginDto.Password);
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }
                return Ok(new { Token = token });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUser(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            try
            {
                var user = await _userService.UpdateUser(id, userDto);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }
    }
}