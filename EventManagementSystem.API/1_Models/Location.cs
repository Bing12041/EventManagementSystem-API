using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.API.Data;

public class Location
{
    [Key]
    public int LocationID { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 5)]
    public string Address { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string City { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string State { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Country { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Event Event { get; set; }
}