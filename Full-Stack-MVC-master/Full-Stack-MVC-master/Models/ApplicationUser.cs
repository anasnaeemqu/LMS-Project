using Microsoft.AspNetCore.Identity;

namespace mvcLab.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; }
    }
}
