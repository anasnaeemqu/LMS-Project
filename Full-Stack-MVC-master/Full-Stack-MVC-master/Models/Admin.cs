using System.ComponentModel.DataAnnotations;

namespace mvcLab.Models
{
    public class Admin
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
