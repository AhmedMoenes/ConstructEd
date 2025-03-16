using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface ICourseRepository : IRepository<Course>
    {
        public ICollection<string> GetCategories();
    }
}
