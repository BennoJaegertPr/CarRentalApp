using CarRentalApp.Db;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Controllers
{
    public class RentalController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CarController> _logger;

        public RentalController(ApplicationDbContext db, ILogger<CarController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public ActionResult RentalLandingPage()
        {
            var rentals = _db.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Car)
                .Where(r => !r.RentComplete)
                .ToList();
            return View(rentals);
        }

        [HttpPost]
        public ActionResult Create()
        {
            if (ModelState.IsValid)
            {
                var customerId = Request.Form["CustomerId"];
                var carId = Request.Form["CarId"];
                var rentedWhen = Request.Form["RentedWhen"];
                var rentedPeriodInDays = Request.Form["RentedPeriodInDays"];
                if (DateTime.TryParse(rentedWhen, out var parsedRentedWhen) && int.TryParse(rentedPeriodInDays,
                                                                                out var parsedRentedPeriodInDays)
                                                                            && int.TryParse(customerId,
                                                                                out var parsedCustomerId) &&
                                                                            int.TryParse(carId, out var parsedCarId))
                {
                }
                else
                {
                    _logger.LogError("Error. Not able to parse rentedPeriodInDays or rentedWhen.");
                    return RedirectToAction("RentalLandingPage");
                }

                var car = _db.Cars.SingleOrDefault(ca => ca.Id == parsedCarId);
                var customer = _db.Customers.SingleOrDefault(cu => cu.Id == parsedCustomerId);

                if (car == null || customer == null)
                {
                    _logger.LogWarning(
                        $"Unable to map car or customer to rental, because one or all objects are null. (Id not found in DB) CustomerId: {customerId} CarId: {carId}",
                        customerId, carId);
                    return RedirectToAction("RentalLandingPage");
                }

                var customerHasCar = _db.Rentals.FirstOrDefault(x => x.Customer.Id == customerId) != null;
                var carIsAvailable = car.isAvailable;
                var cost = parsedRentedPeriodInDays * car.PricePerDay;
                if (!customerHasCar && carIsAvailable)
                {
                    var rental = new Rental
                    {
                        Customer = customer,
                        Car = car,
                        RentedWhen = parsedRentedWhen,
                        RentedPeriodInDays = parsedRentedPeriodInDays,
                        Cost = cost,
                        RentComplete = false,
                        RentedUntil = parsedRentedWhen.AddDays(parsedRentedPeriodInDays)
                    };

                    _db.Rentals.Add(rental);
                    _db.SaveChanges();
                    return RedirectToAction("RentalLandingPage");
                }
                else
                {
                    _logger.LogWarning(
                        $"Unable to map car or customer to rental, because Customer already is renting a car, or car is not available. CustomerId: {customerId} CarId: {carId}",
                        customerId, carId);

                    return RedirectToAction("RentalLandingPage");
                }
            }
            else
            {
                _logger.LogError("Error while trying to create a new Rental due to invalid data input.");
                return RedirectToAction("RentalLandingPage");
            }
        }

        [HttpPost]
        public ActionResult ReturnCar(int id)
        {
            var rentalToBeUpdated = _db.Rentals
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .SingleOrDefault(x => x.Id == id);
            return View("ReturnCar", rentalToBeUpdated);
        }

        [HttpPost]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                long.TryParse(Request.Form["Id"], out var parsedId);
                int.TryParse(Request.Form["newKmCount"], out var parsedNewKmCount);

                var rental = _db.Rentals.Include(r => r.Car).FirstOrDefault(r => r.Id == parsedId);

                if (rental != null)
                {
                        rental.Car.Km = parsedNewKmCount;
                        _db.SaveChanges();
                }
                else
                {
                    _logger.LogError($"No Rental found for RentId: {parsedId}", parsedId);
                }

                return RedirectToAction("RentalLandingPage");
            }

            return RedirectToAction("RentalLandingPage");
        }
    }
}