using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcLab.Models
{
    public class Student
    {
        [Key] 
        public int SSN { get; set; }
        [Required]
        [StringLength(20,MinimumLength =5,ErrorMessage ="please enter valid name with min 5 chars")]
        public string Name { get; set; }
        [StringLength(20, MinimumLength = 4, ErrorMessage = "please enter valid Address with min 4 chars")]
        public string Address { get; set; }
        public string? Image { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public int Age { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<StudCourse>? StudCourses { get; set; } 
    }
}
