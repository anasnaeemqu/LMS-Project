using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcLab.Models
{
    public class TeacherCourse
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        // Linking to existing Course entity
        // Note: Course PK is 'Num' based on previous view_file of Course.cs
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
