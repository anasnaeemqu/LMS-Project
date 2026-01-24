using mvcLab.Validators;
using System.ComponentModel.DataAnnotations;

namespace mvcLab.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(20, MinimumLength =2, ErrorMessage = "Please enter a name between (2,20) characters.")]
        [UniqueDept]
        public string Name { get; set; }
        [Required(ErrorMessage = "Department Manager is required.")]
        public string Manager { get; set; }
        [Required(ErrorMessage = "Department Location is required.")]
        public string Location { get; set; }

        public ICollection<Instructor>? Instructors { get; set; }
        public ICollection<DeptCourse>? DeptCourse { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
