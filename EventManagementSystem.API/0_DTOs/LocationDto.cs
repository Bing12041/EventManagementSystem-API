namespace EventManagementSystem.API.DTOs
{
    public class LocationDto
    {
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
    }
}