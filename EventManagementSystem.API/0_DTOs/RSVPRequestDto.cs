namespace EventManagementSystem.API.DTOs
{
    public class RSVPRequestDto
    {
        public required int UserID { get; set; }
        public required int EventID { get; set; }
        public required string Status { get; set; }
    }
}