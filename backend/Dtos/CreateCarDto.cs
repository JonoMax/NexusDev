using System.ComponentModel.DataAnnotations;

namespace NexusDev.Dtos
{
    public class CreateCarDto
    {
        // Rules I attatch to properties so ASP.NET can protect my API before my code even runs.

        [Required] //The client must send this value, and it must not be null / empty.
        [StringLength(50)] //Don’t let this be ridiculous
        public string Make { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public required string Model { get; set; } = string.Empty;

        [Range(1886, 2100)]
        public int Year { get; set; }

        [Range(0.01, 1_000_000)]
        public decimal Price { get; set; }
    }
}
