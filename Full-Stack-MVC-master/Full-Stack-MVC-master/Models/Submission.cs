using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcLab.Models
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Assignment")]
        public int AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }

        // We will link to Student via basic info since Student entity integration with Identity is unclear/custom
        // Assuming we capture the Student ID (SSN from Student model) if available, or just keeping it simple
        public int? StudentId { get; set; } 
        // Or if we want to link to the User logging in (if Students are users)
        public string? StudentUserId { get; set; }

        public string? SubmissionContent { get; set; } // Text or file path

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public double? Grade { get; set; }
        public string? Feedback { get; set; }
    }
}
