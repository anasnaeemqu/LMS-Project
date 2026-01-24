using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcLab.ErrorHandler;
using mvcLab.Models;

namespace mvcLab.Controllers
{
    public class CourseController : Controller
    {
        private ApplicationDbContext context;
        public CourseController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [AddHeaderResultFilter]
        public IActionResult Index()
        {
            var courses = context.Courses.ToList();
            return View(courses);
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var courseDetails = context.Courses
                                .Include(s => s.StudCourses)
                                .ThenInclude(c => c.Student)
                                .FirstOrDefault(s => s.Num == id);

            return View(courseDetails);

        }
        [Authorize(Roles = "Admin")]

        public IActionResult Create(int id)
        {
            var course = context.Courses.Find(id);

            return View(course);
        }

        [HttpPost]
        [AddHeaderResultFilter]
        [Authorize(Roles = "Admin")]

        public IActionResult Create(Course course)
        {
           
            if (ModelState.IsValid)
            {

                context.Courses.Add(course);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }
        public IActionResult Edit(int id)
        {
           var course= context.Courses.FirstOrDefault(i=>i.Num==id);
            return View(course);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]

        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {

                context.Courses.Update(course);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]

        public IActionResult Delete(int id)
        {
            var course = context.Courses.Include(s=>s.StudCourses).Include(s=>s.InstructorCourses).FirstOrDefault(i=>i.Num==id);
            if(course == null)
            {
                return View("Error");
            }
            else
            {
                context.InstructorCourses.RemoveRange(course.InstructorCourses);
                context.StudCourses.RemoveRange(course.StudCourses);
                context.Courses.Remove(course);
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
