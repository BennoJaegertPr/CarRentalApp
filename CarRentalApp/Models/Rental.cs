namespace CarRentalApp.Models
{
    public class Rental
    {
        public int Id { get; set; }

        public Customer? Customers { get; set; }

        public Car? Car { get; set; }
        
    }
}
