using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace mvcLab.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudCourse> StudCourses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<InstructorCourse> InstructorCourses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DeptCourse> DeptCourses { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Mark> Marks { get; set; }
       
    }
}
