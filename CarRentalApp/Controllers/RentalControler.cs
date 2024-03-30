using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers
{
    public class RentalControler : Controller
    {
        // GET: RentalControler
        public ActionResult RentalLandingPage()
        {
            return View();
        }

        // GET: RentalControler/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RentalControler/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RentalControler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RentalControler/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RentalControler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RentalControler/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RentalControler/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
