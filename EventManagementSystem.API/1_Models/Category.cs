using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.API.Data;

public class Category
{
    [Key]
    public int CategoryID { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }

    public ICollection<Event> Events { get; set; }
}