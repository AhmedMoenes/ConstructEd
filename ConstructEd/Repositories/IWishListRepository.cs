using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface IWishListRepository : IRepository<Wishlist>
    {

        Task<IEnumerable<Wishlist>> GetByUserIdAsync(string userId);
        Task RemoveCourseFromCartAsync(string userId, int courseId);
        Task RemovePluginFromCartAsync(string userId, int pluginId);
        Task ClearCartAsync(string userId);
        Task<int> GetCountByUserIdAsync(string userId);
        Task<bool> IsCourseInWishlistAsync(string userId, int courseId);

    }
}
