namespace CarRentalApp.Models
{
    using System.ComponentModel.DataAnnotations;



    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string? Firstname { get; set; }

        [Required]
        public string? Lastname { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        [Required]
        public string? Phonenumber { get; set;}
    }
}
