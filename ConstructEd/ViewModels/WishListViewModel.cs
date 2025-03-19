using ConstructEd.Models;

namespace ConstructEd.ViewModels
{
    public class WishListViewModel
	{
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }

        public List<CourseViewModel> Courses { get; set; } = new();
        public List<PluginViewModel> Plugins { get; set; } = new();
 
    }  
}