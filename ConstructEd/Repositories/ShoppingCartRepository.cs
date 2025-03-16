using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories

{
    public class ShoppingCartRepository : IShoppingCartRepository
    {

        private readonly DataContext _context;

        public ShoppingCartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ShoppingCart>> GetAllAsync()
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.Course).Include(sc => sc.Plugin)
                .Include(sc => sc.User)
                .ToListAsync();
        }

        public async Task<ShoppingCart> GetByIdAsync(int id)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.Course).Include(sc => sc.Plugin)
                .Include(sc => sc.User)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<IEnumerable<ShoppingCart>> GetByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Where(sc => sc.UserId == userId).Include(sc => sc.Plugin)
                .Include(sc => sc.Course)
                .ToListAsync();
        }

        public async Task InsertAsync(ShoppingCart obj)
        {
            await _context.ShoppingCarts.AddAsync(obj);
            await SaveAsync();
        }

        public async Task UpdateAsync(ShoppingCart obj)
        {
            _context.ShoppingCarts.Update(obj);
            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shoppingCart = await GetByIdAsync(id);
            if (shoppingCart != null)
            {
                _context.ShoppingCarts.Remove(shoppingCart);
                await SaveAsync();
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }



        public async Task RemoveCourseFromCartAsync(string userId, int courseId)
        {

            var cartItem = await _context.ShoppingCarts
                .FirstOrDefaultAsync(sc => sc.UserId == userId && sc.CourseId == courseId);

            if (cartItem != null)
            {

                _context.ShoppingCarts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemovePluginFromCartAsync(string userId, int pluginId)
        {

            var cartItem = await _context.ShoppingCarts
                .FirstOrDefaultAsync(sc => sc.UserId == userId && sc.PluginId == pluginId);

            if (cartItem != null)
            {

                _context.ShoppingCarts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
        public async Task ClearCartAsync(string userId)

        {

            var cartItems = await _context.ShoppingCarts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                _context.ShoppingCarts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }

        }

    }
}
