using CarRentalApp.Db;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ApplicationDbContext db, ILogger<CustomerController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public ActionResult CustomerLandingPage()
        {
            var customers = _db.Customers.ToList();
            return View(customers);
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {


            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                _logger.LogInformation("Created. New Customer created with id: " + customer.Id);
                return RedirectToAction(nameof(CustomerLandingPage));
            }
            return RedirectToAction(nameof(BadRequestObjectResult));
        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Update(customer);
                _db.SaveChanges(true);
                _logger.LogInformation("Updated. Customer with id: " + customer.Id);
                return RedirectToAction(nameof(CustomerLandingPage));
            }
            return RedirectToAction(nameof(BadRequestObjectResult));
        }

        [HttpPost]
        public ActionResult EditCurrentCustomer(int id)
        {
            var customerToBeUpdated = _db.Customers.FirstOrDefault(x => x.Id == id);
            return View("EditCustomer", customerToBeUpdated);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var customerToDelete = _db.Customers.Find(id);
            if (customerToDelete == null)
            {
                _logger.LogError("No entry found for Customer, Id: " + id);
                return RedirectToAction(nameof(BadHttpRequestException));
            }
            else
            {
                _db.Customers.Remove(customerToDelete);
                _db.SaveChanges();
                _logger.LogInformation("Deleted. Id of deleted Customer entity: " + id);
                return RedirectToAction(nameof(CustomerLandingPage));
            }
        }

    }
}
