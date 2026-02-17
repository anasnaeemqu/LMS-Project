using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcLab.Models
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
