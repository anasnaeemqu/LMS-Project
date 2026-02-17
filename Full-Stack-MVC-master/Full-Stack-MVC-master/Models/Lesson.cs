using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcLab.Models
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Content { get; set; }

        public string? VideoUrl { get; set; }
    }
}
