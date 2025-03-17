using ConstructEd.Models;

namespace ConstructEd.ViewModels
{
    public class ShoppingCartViewModel
    {
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }

        public List<CourseItemViewModel> Courses { get; set; } = new();
        public List<PluginItemViewModel> Plugins { get; set; } = new();

        public decimal TotalPrice =>
            Courses.Sum(c => c.Price) + Plugins.Sum(p => p.Price);
    }

    public class CourseItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Duration { get; set; }
        public Category Category { get; set; }
    }

    public class PluginItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}