using System.ComponentModel.DataAnnotations;

namespace mvcLab.ViewModels
{
    public class LoginVM
    {
        [Display(Name ="User Name")]
        [Required(ErrorMessage ="*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
