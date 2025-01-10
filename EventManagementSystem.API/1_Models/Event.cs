using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.API.Data;

public class Event
{
    [Key]
    public int EventID { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public int CreatedBy { get; set; }

    [Required]
    public int CategoryID { get; set; }

    [Required]
    public int LocationID { get; set; }

    public User User { get; set; }
    public Category Category { get; set; }
    public Location Location { get; set; }
    public ICollection<RSVP> RSVPs { get; set; }
}