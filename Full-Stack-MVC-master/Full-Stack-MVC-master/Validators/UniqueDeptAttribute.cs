using mvcLab.Models;
using System.ComponentModel.DataAnnotations;

namespace mvcLab.Validators
{
    public class UniqueDeptAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            string DeptName = value as string;
            var department = (Department)validationContext.ObjectInstance; //for editing check

            var DeptFromDb = context.Departments.FirstOrDefault(e => e.Name == DeptName && e.Id!=department.Id);
            if (DeptFromDb != null)
            {
                return new ValidationResult("Department is Already Exist");
            }
            return ValidationResult.Success;
        }
    }
}
