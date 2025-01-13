using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.API.Service;
using EventManagementSystem.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="eventDto">The event data to create.</param>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
        {
            try
            {
                var @event = await _eventService.CreateEvent(eventDto);
                return CreatedAtAction(nameof(GetEvent), new { id = @event.EventID }, @event);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the event: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await _eventService.GetAllEvents();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving events: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            try
            {
                var @event = await _eventService.GetEvent(id);
                if (@event == null)
                {
                    return NotFound();
                }
                return Ok(@event);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the event: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="id">The ID of the event to update.</param>
        /// <param name="eventDto">The updated event data.</param>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto eventDto)
        {
            try
            {
                var updatedEvent = await _eventService.UpdateEvent(id, eventDto);
                return Ok(updatedEvent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the event: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a specific event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                await _eventService.DeleteEvent(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the event: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves events by category ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to filter events by.</param>
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetEventsByCategory(int categoryId)
        {
            try
            {
                var events = await _eventService.GetEventsByCategory(categoryId);
                return Ok(events);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving events by category: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves events by date.
        /// </summary>
        /// <param name="date">The date to filter events by.</param>
        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetEventsByDate(DateTime date)
        {
            try
            {
                var events = await _eventService.GetEventsByDate(date);
                return Ok(events);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving events by date: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves events by location ID.
        /// </summary>
        /// <param name="locationId">The ID of the location to filter events by.</param>
        [HttpGet("location/{locationId}")]
        public async Task<IActionResult> GetEventsByLocation(int locationId)
        {
            try
            {
                var events = await _eventService.GetEventsByLocation(locationId);
                return Ok(events);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving events by location: {ex.Message}");
            }
        }
    }
}