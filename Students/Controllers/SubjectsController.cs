using System.Linq;
using System.Web.Mvc;
using Students.Models;

namespace Students.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Subjects/
        public ActionResult Index()
        {
            return View(db.Subjects.ToList());
        }

        //
        // GET: /Subjects/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Subjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        //
        // GET: /Subjects/Delete/5
        public ActionResult Delete(string id = null)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
      
            db.Subjects.Remove(subject);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}