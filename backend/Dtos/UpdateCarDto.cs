using System.ComponentModel.DataAnnotations;

namespace NexusDev.Dtos;

public class UpdateCarDto
{

    [Required] 
    [StringLength(50)]
    public string Make { get; set; }

    [Required]
    [StringLength(50)]
    public string Model { get; set; }

    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }
}
