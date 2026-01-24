using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcLab.Models;
using System.Security.Claims;
using BCrypt.Net;

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
            // Ensure session is set (redundant check if cookie is valid but good for strictness)
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
            {
                // In case session expired but cookie didn't, optional re-sync or continue.
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
