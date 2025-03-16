using ConstructEd.Models;

namespace ConstructEd.ViewModels
{
    public class ShoppingCartViewModel
    {
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }

        public List<CourseViewModel> Courses { get; set; } = new();
        public List<PluginViewModel> Plugins { get; set; } = new();

        public decimal TotalPrice =>
            Courses.Sum(c => c.Price) + Plugins.Sum(p => p.Price);
    }

    
}