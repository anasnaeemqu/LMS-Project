namespace mvcLab.ViewModels
{
    public class DeptVM
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string DeptName { get; set; }

        public ICollection<StudCourseVM> StudCourse { get; set; } = new List<StudCourseVM>();
    }
}
