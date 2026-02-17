using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcLab.Models;
using System.Security.Claims;

namespace mvcLab.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // HELPER: Get Current Teacher Entity
        private async Task<Teacher?> GetCurrentTeacherAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;
            return await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);
        }

        // DASHBOARD
        public async Task<IActionResult> Dashboard()
        {
            var teacher = await GetCurrentTeacherAsync();
            if (teacher == null)
            {
                // First time login setup or error
                return RedirectToAction("SetupProfile");
            }

            var courses = await _context.TeacherCourses
                .Where(tc => tc.TeacherId == teacher.Id)
                .Include(tc => tc.Course)
                .Select(tc => tc.Course)
                .ToListAsync();

            return View(courses);
        }

        [HttpGet]
        public IActionResult SetupProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetupProfile(Teacher model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            model.UserId = user.Id;
            model.User = null; // Prevent EF binding issues
            
            // Check if exists
            if (!_context.Teachers.Any(t => t.UserId == user.Id))
            {
                _context.Teachers.Add(model);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Dashboard");
        }

        // CREATE COURSE
        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            if (!ModelState.IsValid) return View(course);

            var teacher = await GetCurrentTeacherAsync();
            if (teacher == null) return RedirectToAction("SetupProfile");

            // 1. Create Course
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // 2. Link to Teacher
            var teacherCourse = new TeacherCourse
            {
                TeacherId = teacher.Id,
                CourseId = course.Num // Assuming 'Num' is PK as seen in Course.cs
            };
            _context.TeacherCourses.Add(teacherCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        // COURSE DETAILS
        public async Task<IActionResult> CourseDetails(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            var lessons = await _context.Lessons.Where(l => l.CourseId == id).ToListAsync();
            var assignments = await _context.Assignments.Where(a => a.CourseId == id).ToListAsync();

            ViewBag.Lessons = lessons;
            ViewBag.Assignments = assignments;

            return View(course);
        }

        // UPLOAD LESSON
        [HttpGet]
        public IActionResult UploadLesson(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadLesson(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction("CourseDetails", new { id = lesson.CourseId });
            }
            ViewBag.CourseId = lesson.CourseId;
            return View(lesson);
        }

        // CREATE ASSIGNMENT
        [HttpGet]
        public IActionResult CreateAssignment(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Assignments.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction("CourseDetails", new { id = assignment.CourseId });
            }
            ViewBag.CourseId = assignment.CourseId;
            return View(assignment);
        }

        // VIEW SUBMISSIONS
        public async Task<IActionResult> ViewSubmissions(int assignmentId)
        {
            var submissions = await _context.Submissions
                .Where(s => s.AssignmentId == assignmentId)
                .Include(s => s.Assignment)
                .ToListAsync();
            
            return View(submissions);
        }

        // GRADE SUBMISSION
        [HttpGet]
        public async Task<IActionResult> GradeSubmission(int id)
        {
            var submission = await _context.Submissions.Include(s => s.Assignment).FirstOrDefaultAsync(s => s.Id == id);
            if (submission == null) return NotFound();
            return View(submission);
        }

        [HttpPost]
        public async Task<IActionResult> GradeSubmission(int id, double grade, string feedback)
        {
            var submission = await _context.Submissions.FindAsync(id);
            if (submission == null) return NotFound();

            submission.Grade = grade;
            submission.Feedback = feedback;
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewSubmissions", new { assignmentId = submission.AssignmentId });
        }
    }
}
