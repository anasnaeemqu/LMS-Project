using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace mvcLab.Controllers
{
    public class TestAuthController : Controller
    {
        public IActionResult TestAuth()
        {
            if (User.Identity.IsAuthenticated)
            {
                Claim n = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
                string name = User.Identity.Name;
                return Content($"welcome {name},id is {n.Value}");
            }
            return Content("you must login first");
        }

    }
}
