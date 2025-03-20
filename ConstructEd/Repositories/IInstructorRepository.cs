using ConstructEd.Models;

namespace ConstructEd.Repositories
{
    public interface IInstructorRepository
    {
        Task<ICollection<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(string id);
        Task SaveAsync();
    }
}