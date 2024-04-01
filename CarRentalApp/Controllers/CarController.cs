using CarRentalApp.Db;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers
{
    public class CarController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly ILogger<CarController> _logger;

        public CarController(ApplicationDbContext db, ILogger<CarController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Display CarLandingPage View and pass all Cars in
        /// the Database to it
        /// </summary>
        /// <returns></returns>
        public ActionResult CarLandingPage()
        {
            var cars = _db.Cars.ToList();
            return View(cars);
        }

        /// <summary>
        /// Create Car by using the retrieved Data from the View
        /// </summary>
        /// <param name="car"></param>
        [HttpPost]
        public ActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _db.Cars.Add(car);
                _db.SaveChanges();
                _logger.LogInformation("Created. New Car created with id: " + car.Id);
                return RedirectToAction(nameof(CarLandingPage));
            }
            _logger.LogError($"Not able to create Car Id: {car.Id}, invalid Form.", car.Id);
			return RedirectToAction(nameof(CarLandingPage));
		}

        /// <summary>
        /// Update Car by using the retrieved Data from the View
        /// </summary>
        /// <param name="car"></param>
        [HttpPost]
        public ActionResult Edit(Car car)
        {

            if (ModelState.IsValid)
            {
                _db.Cars.Update(car);
                _db.SaveChanges();
                _logger.LogInformation("Updated. Car updated with id: " + car.Id);
                return RedirectToAction(nameof(CarLandingPage));
            }
            _logger.LogError($"Not able to edit Car Id: {car.Id}, invalid Form.", car.Id);
			return RedirectToAction(nameof(CarLandingPage));
		}

        /// <summary>
        /// Get Car with id from the Database to pass it to the "EditCar" View
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public ActionResult EditCurrentCar(int id)
        {

            var carToBeUpdated = _db.Cars.SingleOrDefault(x => x.Id == id);
            return View("EditCar", carToBeUpdated);
        }

        /// <summary>
        /// Delete Car with specific id from the Database
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public ActionResult Delete(long id)
        {
            var carToDelete = _db.Cars.Find(id);
            if (carToDelete == null)
            {
                _logger.LogError($"No entry found for Car, {id}: " + id);
				return RedirectToAction(nameof(CarLandingPage));
			}
            else {
                _db.Cars.Remove(carToDelete);
                _db.SaveChanges();
                _logger.LogInformation($"Deleted. {id} of deleted Car entity: " + id);
                return RedirectToAction(nameof(CarLandingPage));
            }
        }
    }
}
