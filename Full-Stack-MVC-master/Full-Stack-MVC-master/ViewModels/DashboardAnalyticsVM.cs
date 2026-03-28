using System.Collections.Generic;

namespace mvcLab.ViewModels
{
    public class DashboardAnalyticsVM
    {
        public List<ChartDataItem> DeptEnrollment { get; set; } = new List<ChartDataItem>();
        public List<ChartDataItem> CourseAvgMarks { get; set; } = new List<ChartDataItem>();
        public List<ChartDataItem> GenderDist { get; set; } = new List<ChartDataItem>();
        public List<ChartDataItem> PopularCourses { get; set; } = new List<ChartDataItem>();
        
        // Total counts for the top cards
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalDepartments { get; set; }
    }

    public class ChartDataItem
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
