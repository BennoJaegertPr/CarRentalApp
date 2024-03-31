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

        public ActionResult CarLandingPage()
        {
            var cars = _db.Cars.ToList();
            return View(cars);
        }

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
            return RedirectToAction(nameof(BadRequestObjectResult));
        }

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
            return RedirectToAction(nameof(BadRequestObjectResult));
        }

        [HttpPost]
        public ActionResult EditCurrentCar(int id)
        {

            var carToBeUpdated = _db.Cars.SingleOrDefault(x => x.Id == id);
            return View("EditCar", carToBeUpdated);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var carToDelete = _db.Cars.Find(id);
            if (carToDelete == null)
            {
                _logger.LogError("No entry found for Car, Id: " + id);
                return RedirectToAction(nameof(BadHttpRequestException));
            }
            else {
                _db.Cars.Remove(carToDelete);
                _db.SaveChanges();
                _logger.LogInformation("Deleted. Id of deleted Car entity: " + id);
                return RedirectToAction(nameof(CarLandingPage));
            }
        }
    }
}
