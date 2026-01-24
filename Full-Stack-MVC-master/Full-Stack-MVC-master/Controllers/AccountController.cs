using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvcLab.Models;
using mvcLab.ViewModels;
using System.Threading.Tasks;

namespace mvcLab.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> _userManager , SignInManager<ApplicationUser> _signInManager)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = userVm.UserName;
                user.Address = userVm.Address;
                IdentityResult result = await userManager.CreateAsync(user,userVm.PasswordHash);
                if (result.Succeeded)
                {
                    //create cookie
                    await signInManager.SignInAsync(user, true);
                    await userManager.AddToRoleAsync(user, "Student");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);
                    }
                }
            }
            return View(userVm);
        }
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(userVM.UserName);

                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, userVM.Password);

                    if (found)
                    {
                        await signInManager.SignInAsync(user, userVM.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "incorrect username or password");
            }

            return View(userVM);
        }



    }
}
