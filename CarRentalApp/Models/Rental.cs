using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApp.Models
{
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Customer_fk")]
        [Required]
        public required Customer Customer { get; set; }

        [ForeignKey("Car_fk")]
        [Required]
        public required Car Car { get; set; }

        [Required]
        public DateTime RentedWhen { get; set; }

        [Required]
        public int RentedPeriodInDays { get; set; }

        [Required]
        public int Cost{ get; set; }


        public DateTime? RentedUntil
        {
            get => RentedWhen.AddDays(RentedPeriodInDays);
            set { }
        }

        [Required]
        public bool RentComplete { get; set; } = true;

        public DateTime? ReturnedWhen { get; set; }
    }
}
