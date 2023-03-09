using System.ComponentModel.DataAnnotations;

namespace ActorModelExample.WebApp.Models;

public class BookingModel
{
    public BookingModel()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Fullname must be longer than 2 characters")]
    public string Name { get; set; } = null!;
}
