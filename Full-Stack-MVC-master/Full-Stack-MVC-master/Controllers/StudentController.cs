using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcLab.Models;
using mvcLab.ViewModels;

namespace mvcLab.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext context;
        public StudentController(ApplicationDbContext _context)
        {
            context = _context;
        }
        //lazy loading ---> get all data 
        //eager loading ---> nav prop hasnt been included --> = null
        [HttpGet]
        public IActionResult Index()
        {
            var students = context.Students
                .Include(s => s.Department)
                .Include(s => s.StudCourses)
                    .ThenInclude(c => c.Course)
                .ToList();

            return View(students);
        }

        [HttpGet("Course/{id:int}")]
        public IActionResult Details(int id)
        {
            var studentDetails = context.Students
                                .Include(s => s.Department)
                                .Include(s => s.StudCourses)
                                      .ThenInclude(c => c.Course)
                                .FirstOrDefault(s=>s.SSN == id);
            return View(studentDetails);

        }

        [HttpGet("DetailsVM/{id:int}")]
        public IActionResult DetailsVM(int id)
        {
            var student = context.Students
                                .Include(s => s.Department)
                                .Include(s => s.StudCourses)
                                      .ThenInclude(c => c.Course)
                                      .FirstOrDefault(s => s.SSN == id);

            List<StudCourseVM> studCourseVMList = new List<StudCourseVM>();
          
                foreach( var course in student.StudCourses)
                {
                StudCourseVM studCourseVM = new StudCourseVM();
                    studCourseVM.StudentId = (int)course.StudId;
                studCourseVM.CourseName = course.Course.Name;
                    studCourseVM.Grade = course.Grade??0;
                    if (studCourseVM.Grade <= 50) { studCourseVM.Color = "red"; } else { studCourseVM.Color = "green"; };
                studCourseVM.minDegree = course.Course.MinDegree;
                    studCourseVM.maxDegree = course.Course.Degree;
                    studCourseVMList.Add(studCourseVM);
                }
                    
     
           
                DeptVM deptVM = new DeptVM();
                deptVM.StudentId = student.SSN;
                deptVM.Name = student.Name;
                deptVM.Age = student.Age;
                deptVM.DeptName = student.Department.Name;
                deptVM.StudCourse = studCourseVMList;
            
            return View(deptVM);

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            ViewBag.Departments = context.Departments
                .Select(d => new { d.Id, d.Name })
                .ToList();

            ViewBag.Courses = context.Courses
                .Select(c => new { c.Num, c.Name })
                .ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Create(Student student, List<int> SelectedCourseIds)
        {
            if (ModelState.IsValid)
            {
                student.StudCourses = new List<StudCourse>();

                foreach (var courseId in SelectedCourseIds)
                {
                    student.StudCourses.Add(new StudCourse
                    {
                        CourseId = courseId,

                        Grade = null
                    });
                }

                context.Students.Add(student);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = context.Departments.Select(d => new { d.Id, d.Name }).ToList();
            ViewBag.Courses = context.Courses.Select(c => new { c.Num, c.Name }).ToList();
            return View(student);
        }




        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Edit(int id)
        {
            var student = context.Students
                .Include(s => s.Department)
                .Include(s => s.StudCourses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefault(s => s.SSN == id);

            if (student == null)
                return NotFound();
            ViewBag.Courses = context.Courses.Select(c => new { c.Num, c.Name }).ToList();

            ViewBag.Departments = context.Departments
                .Select(d => new { d.Id, d.Name })
                .ToList();

            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Edit(Student student, List<int> SelectedCourseIds)
        {
            if (SelectedCourseIds == null)
            {
                throw new Exception("must select courses");
            }
            if (ModelState.IsValid)
            {
                var myStudent = context.Students.Include(s => s.StudCourses).FirstOrDefault(i => i.SSN == student.SSN);
                if (myStudent.StudCourses != null)
                {
                    myStudent.StudCourses.Clear();
                }
                myStudent.Name = student.Name;
                myStudent.Age = student.Age;
                myStudent.Address = student.Address;
                myStudent.Gender = student.Gender;
                myStudent.StudCourses = new List<StudCourse>();
                foreach (var courseId in SelectedCourseIds)
                {
                    myStudent.StudCourses.Add(new StudCourse
                    {
                        CourseId = courseId,

                    });
                }
                context.Students.Update(myStudent);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = context.Departments.Select(d => new { d.Id, d.Name }).ToList();
            return View(student);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult EditStudCourse(int studentId, int courseId)
        {
            var studCourse = context.StudCourses
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .FirstOrDefault(sc => sc.StudId == studentId && sc.CourseId == courseId);
            ViewBag.student = studCourse.Student.Name;
            ViewBag.course = studCourse.Course.Name;
            if (studCourse == null)
            {
                return NotFound();
            }

            // Return the actual StudCourse entity, not a ViewModel
            return View(studCourse);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult EditStudCourse(StudCourse studCourse)
        {
            if (!ModelState.IsValid)
            {
                return View(studCourse);
            }

            try
            {
                // Find the existing record by Id, not by StudId and CourseId
                var existingStudCourse = context.StudCourses
                    .FirstOrDefault(sc => sc.Id == studCourse.Id);

                if (existingStudCourse == null)
                {
                    ModelState.AddModelError("", "No student course record found");
                    return View(studCourse);
                }

                // Update only the grade
                existingStudCourse.Grade = studCourse.Grade;

                context.SaveChanges(); // No need for Update() since EF tracks changes

                TempData["SuccessMessage"] = "Grade updated successfully!";
                return RedirectToAction("Details", "Student", new { id = studCourse.StudId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating record: {ex.Message}");
                return View(studCourse);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int id)
        {
            var student = context.Students
                .Include(s => s.StudCourses)
                .FirstOrDefault(s => s.SSN == id);

            if (student != null)
            {
                context.StudCourses.RemoveRange(student.StudCourses);
                context.Students.Remove(student);
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
