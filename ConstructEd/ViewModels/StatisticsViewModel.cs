namespace ConstructEd.ViewModels
{
    public class StatisticsViewModel
    {
        public int TotalCourses { get; set; } 
        public int ActiveCourses { get; set; } // Courses with enrollments
        public string MostPopularCourse { get; set; } 

        public int TotalPlugins { get; set; } 
        public int ActivePlugins { get; set; } // Plugins with enrollments
        public string MostPopularPlugin { get; set; } 

        public int TotalEnrollments { get; set; } 
        public int CourseEnrollments { get; set; }
        public int PluginEnrollments { get; set; }

        public int TotalUsers { get; set; } 
        public int ActiveUsers { get; set; } // Users with at least one enrollment
        public int TotalInstructors { get; set; }

        public int TotalPayments { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
