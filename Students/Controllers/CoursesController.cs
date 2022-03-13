using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Students.Models;

namespace Students.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Courses/

        public ActionResult Index()
        {
            var courses = from c in db.Courses.ToList<Course>()
                          where c.StartDate <= DateTime.Today
                          orderby c.Timing
                          select c; 

            return View(courses);
        }


        public ActionResult Upcoming()
        {
            var courses = from c in db.Courses.ToList<Course>()
                          where c.StartDate > DateTime.Today
                          orderby c.Timing
                          select c;

            return View(courses);
        }

        public ActionResult History()
        {
            var courses = from c in db.Courses.ToList<Course>()
                          where c.EndDate != null 
                          orderby c.EndDate 
                          orderby c.Timing
                          select c;

            return View(courses);
        }



        public ActionResult List(string id)
        {
            var courses = from c in db.Courses.ToList<Course>()
                          where c.SubjectCode == id
                          orderby c.StartDate
                          select c;

            return View(courses);
        }



        //
        // GET: /Courses/Create

        public ActionResult Create()
        {
            ViewBag.SubjectCode = new SelectList(db.Subjects, "Code", "Title");
            return View();
        }

        //
        // POST: /Courses/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectCode = new SelectList(db.Subjects, "Code", "Title", course.SubjectCode);
            return View(course);
        }

        //
        // GET: /Courses/Edit/5

        public ActionResult Edit(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectCode = new SelectList(db.Subjects, "Code", "Title", course.SubjectCode);
            return View(course);
        }

        //
        // POST: /Courses/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectCode = new SelectList(db.Subjects, "Code", "Title", course.SubjectCode);
            return View(course);
        }

        //
        // GET: /Courses/Delete/5

        public ActionResult Delete(int id)
        {
            Course course = null;
            try
            {
                course = db.Courses.Find(id);
                db.Courses.Remove(course);
                db.SaveChanges();
                ViewBag.Message =  "[" + course.CourseName + "] foi deletado com sucesso !";

            }
            catch (Exception)
            {
                ViewBag.Message = "[" + course.CourseName + "]  não pode ser deletado, tente novamente !";
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}