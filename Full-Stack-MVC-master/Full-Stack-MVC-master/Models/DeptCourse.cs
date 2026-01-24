using mvcLab.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcLab.Models
{
    public class DeptCourse
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Course))]
        //[UniqueCourseForDept]
        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        [ForeignKey(nameof(Department))]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

    }
}
