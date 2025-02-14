using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BorrowingService.Controllers
{
    public class BorrowingController : Controller
    {
        // GET: BorrowingController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BorrowingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BorrowingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BorrowingController/Create
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

        // GET: BorrowingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BorrowingController/Edit/5
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

        // GET: BorrowingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BorrowingController/Delete/5
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
