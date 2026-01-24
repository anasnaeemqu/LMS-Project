using System.ComponentModel.DataAnnotations;

namespace mvcLab.ViewModels
{
    public class RegisterVM
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(PasswordHash),ErrorMessage ="Passwords are not matching")]
        public string ConfirmPassword { get; set; }

    }
}
