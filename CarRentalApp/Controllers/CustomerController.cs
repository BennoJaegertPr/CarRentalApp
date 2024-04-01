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

        /// <summary>
        /// Displays the CustomerLandingPage and passes all customers to the View
        /// </summary>
        public ActionResult CustomerLandingPage()
        {
            var customers = _db.Customers.ToList();
            return View(customers);
        }

		/// <summary>
		/// Create a new Customer by using the retrieved Data from the View
		/// </summary>
		/// <param name="customer"></param>
		[HttpPost]
        public ActionResult Create(Customer customer)
        {


            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                _logger.LogInformation($"Created. New Customer created with id: {customer.Id}" + customer.Id);
                return RedirectToAction(nameof(CustomerLandingPage));
            }
            _logger.LogError($"Not able to created Customer with id: {customer.Id} invalid Form.", customer.Id);
            return RedirectToAction(nameof(CustomerLandingPage));
		}

		/// <summary>
		/// Update a specific Customer by using the retrieved Data from the View
		/// </summary>
		/// <param name="customer"></param>
		[HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Update(customer);
                _db.SaveChanges(true);
                _logger.LogInformation($"Updated. Customer with id: {customer.Id}" + customer.Id);
                return RedirectToAction(nameof(CustomerLandingPage));
            }
            _logger.LogInformation($"Not able to update Customer with id: {customer.Id}", customer.Id);
            return RedirectToAction(nameof(CustomerLandingPage));
		}

        /// <summary>
        /// Retrieves the Customer requested by the form.
        /// Displays the EditCustomer View.
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public ActionResult EditCurrentCustomer(int id)
        {
            var customerToBeUpdated = _db.Customers.FirstOrDefault(x => x.Id == id);
            return View("EditCustomer", customerToBeUpdated);
        }

        /// <summary>
        /// Deletes a specific Customer from the Database
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public ActionResult Delete(long id)
        {
            var customerToDelete = _db.Customers.Find(id);
            if (customerToDelete == null)
            {
                _logger.LogError($"No entry found for Customer, Id: {id}" + id);
                return RedirectToAction(nameof(CustomerLandingPage));
			}
            else
            {
                _db.Customers.Remove(customerToDelete);
                _db.SaveChanges();
                _logger.LogInformation($"Deleted. Id of deleted Customer entity: {id}", id);
                return RedirectToAction(nameof(CustomerLandingPage));
            }
        }

    }
}
