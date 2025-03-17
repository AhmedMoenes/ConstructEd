using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface IWishListRepository : IRepository<Wishlist>
    {

        Task<IEnumerable<Wishlist>> GetByUserIdAsync(string userId);
        Task<bool> RemovePluginFromCartAsync(string userId, int pluginId);
        Task<bool> RemoveCourseFromCartAsync(string userId, int courseId);
        Task ClearCartAsync(string userId);
        Task<int> GetCountByUserIdAsync(string userId);
        Task<bool> IsCourseInWishlistAsync(string userId, int courseId);
        Task<Wishlist?> GetCourseByUserIdAsync(string userId, int courseId);
        Task<Wishlist?> GetPluginByUserIdAsync(string userId, int pluginId);
    }
}
