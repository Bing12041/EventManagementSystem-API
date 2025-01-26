using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.API.Service;
using EventManagementSystem.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RSVPController : ControllerBase
    {
        private readonly IRSVPService _rsvpService;

        public RSVPController(IRSVPService rsvpService)
        {
            _rsvpService = rsvpService ?? throw new ArgumentNullException(nameof(rsvpService));
        }

        /// <summary>
        /// RSVP to an event.
        /// </summary>
        /// <param name="rsvpRequest">The RSVP request data.</param>
        [HttpPost]
        public async Task<IActionResult> RSVPToEvent([FromBody] RSVPRequestDto rsvpRequest)
        {
            try
            {
                var rsvp = await _rsvpService.RSVPToEvent(rsvpRequest);
                return CreatedAtAction(nameof(GetRSVPForUserAndEvent), new { userId = rsvp.UserID, eventId = rsvp.EventID }, rsvp);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while RSVPing to the event: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all RSVPs for an event.
        /// </summary>
        /// <param name="eventId">The ID of the event to get RSVPs for.</param>
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetRSVPsForEvent(int eventId)
        {
            try
            {
                var rsvps = await _rsvpService.GetRSVPsForEvent(eventId);
                return Ok(rsvps);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving RSVPs for the event: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all RSVPs for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to get RSVPs for.</param>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRSVPsForUser(int userId)
        {
            try
            {
                var rsvps = await _rsvpService.GetRSVPsForUser(userId);
                return Ok(rsvps);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving RSVPs for the user: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing RSVP.
        /// </summary>
        /// <param name="userId">The ID of the user who made the RSVP.</param>
        /// <param name="eventId">The ID of the event the RSVP is for.</param>
        /// <param name="newStatus">The new status of the RSVP.</param>
        [HttpPut("{userId}/{eventId}")]
        public async Task<IActionResult> UpdateRSVP(int userId, int eventId, [FromBody] string newStatus)
        {
            try
            {
                var rsvp = await _rsvpService.UpdateRSVP(userId, eventId, newStatus);
                return Ok(rsvp);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the RSVP: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancel an RSVP.
        /// </summary>
        /// <param name="userId">The ID of the user who made the RSVP.</param>
        /// <param name="eventId">The ID of the event the RSVP is for.</param>
        [HttpDelete("{userId}/{eventId}")]
        public async Task<IActionResult> CancelRSVP(int userId, int eventId)
        {
            try
            {
                await _rsvpService.CancelRSVP(userId, eventId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while canceling the RSVP: {ex.Message}");
            }
        }

        /// <summary>
        /// Get an RSVP for a specific user and event combination.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="eventId">The ID of the event.</param>
        [HttpGet("user/{userId}/event/{eventId}")]
        public async Task<IActionResult> GetRSVPForUserAndEvent(int userId, int eventId)
        {
            try
            {
                var rsvp = await _rsvpService.GetRSVPsForEvent(eventId);
                var specificRSVP = rsvp.FirstOrDefault(r => r.UserID == userId);
                if (specificRSVP == null)
                {
                    return NotFound();
                }
                return Ok(specificRSVP);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the RSVP: {ex.Message}");
            }
        }
    }
}