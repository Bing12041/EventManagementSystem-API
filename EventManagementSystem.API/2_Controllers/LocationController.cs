using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.API.Service;
using EventManagementSystem.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        }

        /// <summary>
        /// Creates a new location.
        /// </summary>
        /// <param name="locationDto">The location data to create.</param>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateLocation([FromBody] LocationDto locationDto)
        {
            try
            {
                var location = await _locationService.CreateLocation(locationDto);
                return CreatedAtAction(nameof(GetLocation), new { id = location.LocationID }, location);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the location: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific location by its ID.
        /// </summary>
        /// <param name="id">The ID of the location to retrieve.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            try
            {
                var location = await _locationService.GetLocation(id);
                if (location == null)
                {
                    return NotFound();
                }
                return Ok(location);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the location: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing location.
        /// </summary>
        /// <param name="id">The ID of the location to update.</param>
        /// <param name="locationDto">The updated location data.</param>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationDto locationDto)
        {
            try
            {
                var updatedLocation = await _locationService.UpdateLocation(id, locationDto);
                return Ok(updatedLocation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the location: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a specific location by its ID.
        /// </summary>
        /// <param name="id">The ID of the location to delete.</param>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                await _locationService.DeleteLocation(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the location: {ex.Message}");
            }
        }
    }
}