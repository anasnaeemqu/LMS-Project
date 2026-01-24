using mvcLab.Validators;
using System.ComponentModel.DataAnnotations;

namespace mvcLab.Models
{
    public class Course
    {
        [Key]
        public int Num { get; set; }

        [Required(ErrorMessage = "Course name is required.")] 
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Please enter a name between (2,20) characters.")]
        [UniqueCourse]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")] 
        public string? Description { get; set; }

        [Required(ErrorMessage = "Degree is required.")] 
        [Range(100, 150, ErrorMessage = "Max degree must be between 100 and 150.")]
        public decimal Degree { get; set; }

        [Required(ErrorMessage = "MinDegree is required.")] 
        [Range(50, 60, ErrorMessage = "Min degree must be between 50 and 60.")]
        public decimal MinDegree { get; set; }

        public ICollection<StudCourse>? StudCourses { get; set; }
        public ICollection<DeptCourse>? DeptCourse { get; set; }
        public ICollection<InstructorCourse>? InstructorCourses { get; set; }
    }
}
