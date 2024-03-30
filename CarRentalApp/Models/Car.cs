using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class Car
    {

        public int Id { get; set; }

        [Required]
        public string? Brand { get; set; }

        [Required]
        public string? Color { get; set; }

        [Required]
        public string? Km { get; set; }

        public bool isAvailable { get; set; } = true;

    }
}
