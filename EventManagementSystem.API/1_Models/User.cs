using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.API.Data;

public class User
{
    [Key]
    public int UserID { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }

    public ICollection<Event> CreatedEvents { get; set; }
    public ICollection<RSVP> RSVPs { get; set; }

}