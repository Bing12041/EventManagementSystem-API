using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementSystem.API.Data;

public class RSVP
{
    [Key]
    [Column(Order = 0)]
    public int UserID { get; set; }
    [Key]
    [Column(Order = 1)]
    public int EventID { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; }

    public User User { get; set; }
    public Event Event { get; set; }
}