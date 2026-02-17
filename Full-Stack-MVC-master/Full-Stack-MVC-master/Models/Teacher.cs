using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace mvcLab.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Bio { get; set; }

        public virtual ICollection<TeacherCourse> TeacherCourses { get; set; }
    }
}
