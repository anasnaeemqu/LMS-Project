using mvcLab.Models;
using System.ComponentModel.DataAnnotations;

namespace FullstackMVC.Validators
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            string email = value as string;
            var instuctor = (Instructor)validationContext.ObjectInstance; //for editing check

            var emailfromDb = context.Instructors.FirstOrDefault(e => e.Email == email && e.Id!=instuctor.Id);
            if (emailfromDb != null)
            {
                return new ValidationResult("Email Already Exist");
            }
            return ValidationResult.Success;
        }
    }
}
