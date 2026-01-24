using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcLab.Models;

namespace mvcLab.Controllers
{
    public class InstructorController : Controller
    {
        private ApplicationDbContext context;
        public InstructorController(ApplicationDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            List<Instructor> instructors = context.Instructors.Include(d => d.Department).Include(e=>e.InstructorCourses).ThenInclude(e=>e.Course).ToList();
            return View(instructors);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            ViewBag.Departments = context.Departments.ToList();
            ViewBag.Courses = context.Courses.ToList();
            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Create(Instructor instructor, List<int> SelectedCourseIds)
        {
            if (ModelState.IsValid)
            {
                instructor.InstructorCourses = new List<InstructorCourse>();

                foreach (var courseId in SelectedCourseIds)
                {
                    instructor.InstructorCourses.Add(new InstructorCourse
                    {
                        CourseId = courseId,

                    });
                }
                context.Instructors.Add(instructor);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = context.Departments.ToList();
            ViewBag.Courses = context.Courses.ToList();
            return View();
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Edit()
        {
            ViewBag.Departments = context.Departments.ToList();
            ViewBag.Courses = context.Courses.ToList();
            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Edit(Instructor instructor, List<int> SelectedCourseIds)
        {
            if (SelectedCourseIds == null)
            {
                throw new Exception("must select courses");
            }
            if (ModelState.IsValid)
            {
                var myInstructor = context.Instructors.Include(s => s.InstructorCourses).FirstOrDefault(i => i.Id == instructor.Id);
                if (myInstructor.InstructorCourses != null)
                {
                    myInstructor.InstructorCourses.Clear();
                }
                myInstructor.Name = instructor.Name;
                myInstructor.Address = instructor.Address;
                myInstructor.Salary = instructor.Salary;
                myInstructor.Age = instructor.Age;
                myInstructor.Degree = instructor.Degree;
                myInstructor.Email = instructor.Email;
                myInstructor.InstructorCourses = new List<InstructorCourse>();
                foreach (var courseId in SelectedCourseIds)
                {
                    myInstructor.InstructorCourses.Add(new InstructorCourse
                    {
                        CourseId = courseId,

                    });
                }
                context.Instructors.Update(myInstructor);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = context.Departments.ToList();
            ViewBag.Courses = context.Courses.ToList();

            return View(instructor);
        }
    }
}
