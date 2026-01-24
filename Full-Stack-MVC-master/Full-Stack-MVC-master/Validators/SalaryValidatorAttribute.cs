using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using mvcLab.Models;
using System.ComponentModel.DataAnnotations;

namespace mvcLab.Validators
{
    public class SalaryValidatorAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            decimal salary;
            Decimal.TryParse(value.ToString(), out salary);
            var Instructor = (Instructor)validationContext.ObjectInstance;
            var dept = context.Departments.FirstOrDefault(s => s.Id == Instructor.DepartmentId);
       
            switch (dept.Name)
            {
                case "SD":
                    if (salary != null) {
                        if (salary < 10000)
                        {
                            return new ValidationResult("salary must be more than 10,000");
                        }
                    }
                    break;
                case "HR":
                    if (salary != null)
                    {
                        if (salary < 10000||salary>50000)
                        {
                            return new ValidationResult("salary must be between 10,000 and 50,000");
                        }
                    }
                    break;
            }


            return ValidationResult.Success;
        }
    }
}
