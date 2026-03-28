using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcLab.Models;
using System.Security.Claims;
using BCrypt.Net;
using System.Linq;
using System.Collections.Generic;

namespace mvcLab.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Seeding logic for testing if no admin exists
            if (!_context.Admin.Any())
            {
                var admin = new Admin
                {
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456")
                };
                _context.Admin.Add(admin);
                _context.SaveChanges();
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and Password are required");
                return View();
            }

            var admin = _context.Admin.SingleOrDefault(a => a.Username == username);

            if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                // Create Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Identity.Application"); // Using Identity.Application to match existing Identity cookie
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                // Sign In using the existing Identity cookie scheme so [Authorize(Roles="Admin")] works
                await HttpContext.SignInAsync(Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                
                HttpContext.Session.SetString("AdminUser", admin.Username);

                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid Admin Credentials");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            var viewModel = new mvcLab.ViewModels.DashboardAnalyticsVM();

            // Total counts
            viewModel.TotalStudents = _context.Students.Count();
            viewModel.TotalCourses = _context.Courses.Count();
            viewModel.TotalInstructors = _context.Instructors.Count();
            viewModel.TotalDepartments = _context.Departments.Count();

            // Enrollment by Department
            viewModel.DeptEnrollment = _context.Departments
                .Select(d => new mvcLab.ViewModels.ChartDataItem { 
                    Label = d.Name, 
                    Value = _context.Students.Count(s => s.DepartmentId == d.Id) 
                }).ToList();

            // Course Average Marks
            viewModel.CourseAvgMarks = _context.Courses
                .Select(c => new mvcLab.ViewModels.ChartDataItem { 
                    Label = c.Name, 
                    Value = _context.Marks.Where(m => m.CourseId == c.Num).Any() 
                        ? (double)_context.Marks.Where(m => m.CourseId == c.Num).Average(m => m.Marks) 
                        : 0 
                }).ToList();

            // Gender Distribution
            viewModel.GenderDist = _context.Students
                .GroupBy(s => s.Gender)
                .Select(g => new mvcLab.ViewModels.ChartDataItem { 
                    Label = g.Key, 
                    Value = (double)g.Count() 
                }).ToList();

            // Popular Courses (Top 5)
            viewModel.PopularCourses = _context.Courses
                .Select(c => new mvcLab.ViewModels.ChartDataItem { 
                    Label = c.Name, 
                    Value = (double)_context.StudCourses.Count(sc => sc.CourseId == c.Num) 
                })
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
