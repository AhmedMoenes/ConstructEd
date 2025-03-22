using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories

{
    public class WishListRepository : IWishListRepository
	{

        private readonly DataContext _context;

        public WishListRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Wishlist>> GetAllAsync()
        {
            return await _context.Wishlists
                .Include(sc => sc.Course).Include(sc => sc.Plugin)
                .Include(sc => sc.User)
                .ToListAsync();
        }

        public async Task<Wishlist> GetByIdAsync(int id)
        {
            return await _context.Wishlists
                .Include(sc => sc.Course).Include(sc => sc.Plugin)
                .Include(sc => sc.User)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<IEnumerable<Wishlist>> GetByUserIdAsync(string userId)
        {
            return await _context.Wishlists
                .Where(sc => sc.UserId == userId).Include(sc => sc.Plugin)
                .Include(sc => sc.Course)
                .ToListAsync();
        }

        public async Task InsertAsync(Wishlist obj)
        {
            await _context.Wishlists.AddAsync(obj);
            await SaveAsync();
        }

        public async Task UpdateAsync(Wishlist obj)
        {
            _context.Wishlists.Update(obj);
            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Wish = await GetByIdAsync(id);
            if (Wish != null)
            {
                _context.Wishlists.Remove(Wish);
                await SaveAsync();
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)

        {

            var Wishlist = await _context.Wishlists
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (Wishlist.Any())
            {
                _context.Wishlists.RemoveRange(Wishlist);
                await _context.SaveChangesAsync();
            }

        }
        public async Task<int> GetCountByUserIdAsync(string userId)
        {
            return await _context.Wishlists
                .Where(w => w.UserId == userId && (w.CourseId>0 || w.PluginId > 0))
                .CountAsync();
        }
        public async Task<bool> IsCourseInWishlistAsync(string userId, int courseId)
        {
            return await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.CourseId == courseId);
        }
        public async Task<bool> IsPluginInWishlistAsync(string userId, int pluginId)
        {
            return await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.PluginId == pluginId);
        }

        public async Task<Wishlist?> GetCourseByUserIdAsync(string userId, int courseId)
        {
            return await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.CourseId == courseId);
        }

        public async Task<Wishlist?> GetPluginByUserIdAsync(string userId, int pluginId)
        {
            return await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.PluginId == pluginId);
        }
        public async Task<bool> RemoveCourseFromCartAsync(string userId, int courseId)
        {
            var item = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.CourseId == courseId);

            if (item != null)
            {
                _context.Wishlists.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> RemovePluginFromCartAsync(string userId, int pluginId)
        {
            var item = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.PluginId == pluginId);

            if (item != null)
            {
                _context.Wishlists.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

    }
}
