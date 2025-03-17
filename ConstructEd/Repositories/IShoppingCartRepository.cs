using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task<IEnumerable<ShoppingCart>> GetByUserIdAsync(string userId);
        Task ClearCartAsync(string userId);
        Task<bool> IsCourseInCartAsync(string userId, int courseId);
        Task<bool> RemoveCourseFromCartAsync(string userId, int courseId);
        Task<bool> RemovePluginFromCartAsync(string userId, int pluginId);
    }
}
