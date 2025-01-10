namespace EventManagementSystem.API.DTOs
{
    public class EventDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required int CreatedBy { get; set; }
        public required int CategoryID { get; set; }
        public required int LocationID { get; set; }
    }
}