using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {

        Task<IEnumerable<ShoppingCart>> GetByUserIdAsync(string userId);
        Task RemoveCourseFromCartAsync(string userId, int courseId);
        Task RemovePluginFromCartAsync(string userId, int pluginId);



    }
}
