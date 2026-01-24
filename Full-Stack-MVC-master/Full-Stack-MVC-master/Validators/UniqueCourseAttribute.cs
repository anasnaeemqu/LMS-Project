using Microsoft.EntityFrameworkCore;
using mvcLab.Models;
using System.ComponentModel.DataAnnotations;

namespace mvcLab.Validators
{
    public class UniqueCourseAttribute: ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            string CourseName = value as string;
            if (string.IsNullOrWhiteSpace(CourseName))
                return ValidationResult.Success; // required annotation will handle it
            var currentContext = validationContext.ObjectInstance as Course; // for edit check

            var CourseFromDb = context.Courses.FirstOrDefault(e => e.Name == CourseName && e.Num != currentContext.Num);
            if (CourseFromDb != null)
            {
                return new ValidationResult("Course Already Exists");
            }
            return ValidationResult.Success;
        }

    }
}
