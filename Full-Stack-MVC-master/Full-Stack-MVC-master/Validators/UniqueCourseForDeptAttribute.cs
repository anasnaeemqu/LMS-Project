using Microsoft.EntityFrameworkCore;
using mvcLab.Models;
using System.ComponentModel.DataAnnotations;

namespace mvcLab.Validators
{
    public class UniqueCourseForDeptAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            var CourseName = value as string;
            var deptCourse = (DeptCourse)validationContext.ObjectInstance;

            var course = context.Courses.FirstOrDefault(e => e.Name == CourseName);


            bool exists = context.DeptCourses
                .Any(dc=>deptCourse.DepartmentId==dc.DepartmentId && dc.CourseId == course.Num);

            if (exists)
            {
                return new ValidationResult("This course already exists in this department.");
            }

            return ValidationResult.Success;
        }

    }
}
