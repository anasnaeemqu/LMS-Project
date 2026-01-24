using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcLab.ErrorHandler;
using mvcLab.Models;
using mvcLab.Repository.IRepository;

namespace mvcLab.Controllers
{
    public class DepartmentController : Controller
    {
        //private ApplicationDbContext context;
        IRepository<Department> repository;
        IRepository<Course> courseRepo;
        IRepository<DeptCourse> DeptCoursesRepository;
        public DepartmentController(IRepository<Department> _repo, IRepository<Course> _courseRepo,
        IRepository<DeptCourse> _DeptCoursesRepository)
        {
            //context = _context;
            repository = _repo;
            courseRepo = _courseRepo;
            DeptCoursesRepository = _DeptCoursesRepository;
        }
        public IActionResult Index()
        {
            //List<Department> department = context.Departments.Include(d => d.DeptCourse).ThenInclude(dc => dc.Course).ToList();
            IEnumerable<Department> departments = repository.GetAll("DeptCourse,DeptCourse.Course");
            return View(departments);
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            ViewBag.Courses = courseRepo.GetAll();
            return View();

        }
        //[HttpPost]
        //public IActionResult Create(Department dept, List<int> SelectedCourseIds)
        //{
        //    if (ModelState.IsValid) {
        //        dept.DeptCourse = new List<DeptCourse>();

        //        foreach (var courseId in SelectedCourseIds)
        //        {
        //            dept.DeptCourse.Add(new DeptCourse
        //            {
        //                CourseId = courseId,

        //            });
        //        }
        //        context.Departments.Add(dept);
        //        context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewBag.Courses = context.Courses.ToList();
        //    return View();
        //} 
        [HttpPost]
        [DeptActionFilter]
        [Authorize(Roles = "Admin")]

        public IActionResult Create(Department dept, List<int> SelectedCourseIds)
        {
            if (SelectedCourseIds == null)
            {
                throw new Exception("must select courses");
            }
            if (ModelState.IsValid)
            {
                dept.DeptCourse = new List<DeptCourse>();
                foreach (var courseId in SelectedCourseIds)
                {
                    dept.DeptCourse.Add(new DeptCourse
                    {
                        CourseId = courseId,

                    });
                }
                //context.Departments.Add(dept);
                //context.SaveChanges();
                repository.Add(dept);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Courses = courseRepo.GetAll();
            return View();
        }

        [Authorize(Roles = "Admin,Instructor")]

        public IActionResult Edit(int id)
        {
            //var department = context.Departments.FirstOrDefault(i => i.Id == id);
            //ViewBag.Courses = context.Courses.ToList();
            var department = repository.GetById(id);
            ViewBag.Courses = courseRepo.GetAll();
            return View(department);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]

        public IActionResult Edit(Department department, List<int> SelectedCourseIds)
        {
            if (SelectedCourseIds == null)
            {
                throw new Exception("must select courses");
            }
            if (ModelState.IsValid)
            {
                //var myDept = context.Departments.Include(s => s.DeptCourse).FirstOrDefault(i => i.Id == department.Id);
                var myDept = repository.GetById(department.Id, "DeptCourse");
                if (myDept.DeptCourse!=null)
                {
                    myDept.DeptCourse.Clear();
                }
                myDept.Name = department.Name;
                myDept.Manager = department.Manager;
                myDept.Location = department.Location;
                myDept.DeptCourse = new List<DeptCourse>();
                foreach (var courseId in SelectedCourseIds)
                {
                    myDept.DeptCourse.Add(new DeptCourse
                    {
                        CourseId = courseId,

                    });
                }
                //context.Departments.Update(myDept);
                //context.SaveChanges();
                repository.Update(myDept);
                return RedirectToAction(nameof(Index));
            }
            //ViewBag.Courses = context.Courses.ToList();
            ViewBag.Courses = courseRepo.GetAll();

            return View(department);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int id)
        {
            //var dept = context.Departments
            //    .Include(s => s.DeptCourse)
            //    .FirstOrDefault(s => s.Id == id);
            var dept= repository.GetById(id, "DeptCourse");

            if (dept != null)
            {
                //context.DeptCourses.RemoveRange(dept.DeptCourse);
                //context.Departments.Remove(dept);
                //context.SaveChanges();
                DeptCoursesRepository.RemoveRange(dept.DeptCourse);
                repository.Remove(dept);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
