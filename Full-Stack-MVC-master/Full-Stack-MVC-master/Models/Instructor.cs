using FullstackMVC.Validators;
using mvcLab.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcLab.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Degree is required.")]
        [Range(20,40,ErrorMessage ="age must be between 20-40")]
        public int Age { get; set; }
        [Required(ErrorMessage = "salary is required.")]
        [SalaryValidator]
        public decimal Salary { get; set; }
        [Required(ErrorMessage = "Degree is required.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Please enter a degree between (2,20) characters.")]
        public string Degree { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [UniqueEmail]
        public string Email { get; set; }

        [ForeignKey(nameof(Department))]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

 

        public ICollection<InstructorCourse>? InstructorCourses { get; set; }

    }
}
