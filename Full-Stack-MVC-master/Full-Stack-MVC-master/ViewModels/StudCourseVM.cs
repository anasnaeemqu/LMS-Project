namespace mvcLab.ViewModels
{
    public class StudCourseVM
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public double Grade { get; set; }
        public bool IsAssigned { get; set; }
        public string Color { get; set; }
        public decimal minDegree { get; set; }
        public decimal maxDegree { get; set; }
    }
}
