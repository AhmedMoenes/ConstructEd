using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface ICourseRepository : IRepository<Course>
    {
        public ICollection<string> GetCategories();
        public Task<ICollection<Course>> GetByCategoryAsync(Category? category);
    }
}
