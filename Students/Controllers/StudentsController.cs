using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Students.Models;

namespace Students.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Students/
        // obtem os 10 alunos mais recentes
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Courses).OrderByDescending(s => s.JoinedOn).Take(10);
            return View(students.ToList());
        }

        // Lista de aluno spor curso
        public ActionResult List(int id)
        {
            @Session.Add("message", "");
            var course = db.Courses.Include("Subjects").Where(c => c.Code == id).Single();

            var students = from s in db.Students.ToList<Student>()
                           where s.CourseCode == id
                           orderby s.RollNo
                           select s;

            ViewBag.Course = course; 
            return View(students);
        }


        //
        // GET: /Students/Add
        public ActionResult Add()
        {
            ViewBag.CourseCode = new SelectList(db.Courses, "Code", "CourseName");
            return View();
        }


        // POST: /Students/Add

        [HttpPost]
        public ActionResult Add(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseCode = new SelectList(db.Courses, "Code", "SubjectCode", student.CourseCode);
            return View(student);
        }


        public ActionResult AddToCourse(int id)
        {
          
            var course = db.Courses.Where(c => c.Code == id).Single(); 
            //ViewBag.CourseCode = id;
            ViewBag.CourseName = course.CourseName;

            int rollno = db.Students.Where(s => s.CourseCode == id).Select(s => s.RollNo).DefaultIfEmpty(0).Max();
            // ViewBag.RollNo = rollno + 1;
            Student student = new Student();
            student.RollNo = rollno + 1;
            student.CourseCode = id;

            return View(student);
        }

        [HttpPost]
        public ActionResult AddToCourse(Student student)
        {
            Session.Add("message", "");
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                Session.Add("message", "Student [" + student.Fullname + "] foi adicionado com sucesso !");

                return RedirectToAction("AddToCourse", new { id = student.CourseCode });
            }
            Session.Add("message", "Corrija os erros e tente novamente!");
            return View(student);
        }

        //
        // GET: /Students/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseCode = new SelectList(db.Courses, "Code", "CourseName", student.CourseCode);
            return View(student);
        }

        //
        // POST: /Students/Edit/5
        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Detalhes do Aluno foram atualizados com sucesso!";
            }
            else
            {
                ViewBag.Message = "Corrija os erros e tente novamente!";
               
            }

            ViewBag.CourseCode = new SelectList(db.Courses, "Code", "CourseName", student.CourseCode);
            return View(student);
        }

        //
        // GET: /Students/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Student student = db.Students.Find(id); 
            @ViewBag.Fullname = student.Fullname;
            @ViewBag.CourseName = student.Courses.CourseName;
            @ViewBag.CourseCode = student.CourseCode;
            db.Students.Remove(student);
            db.SaveChanges();
            return View();
        }

        //  Students/Rollno/id
        public ActionResult Rollno(int id)
        {

            var rollno = db.Students.Where(s => s.CourseCode == id).Select( s => s.RollNo).DefaultIfEmpty(0).Max();
            var courseFee = db.Courses.Where(c => c.Code == id).Select(c => c.CourseFee).Single();
            return Json(new { Rollno = rollno + 1, CourseFee = courseFee }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchname)
        {
            var students = db.Students.Include("Courses").
                Where(s => s.Fullname.Contains(searchname)).ToList <Student>();

            return PartialView("_search",students);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}