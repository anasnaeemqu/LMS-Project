using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvcLab.ViewModels;
using System.Threading.Tasks;

namespace mvcLab.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleVM roleVM)
        {
            IdentityRole role = new IdentityRole();
            role.Name = roleVM.Type;

            IdentityResult res = await _roleManager.CreateAsync(role);
            if (res.Succeeded) {
                return View();
            }
            foreach(var e in res.Errors)
            {
                ModelState.AddModelError("", e.Description);
            }
            return View(roleVM);

        }
    }
}
