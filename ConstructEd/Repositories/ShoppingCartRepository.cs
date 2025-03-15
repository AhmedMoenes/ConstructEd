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
                .Include(sc => sc.Course)
                .Include(sc => sc.User)
                .ToListAsync();
        }

        public async Task<ShoppingCart> GetByIdAsync(int id)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.Course)
                .Include(sc => sc.User)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<IEnumerable<ShoppingCart>> GetByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Where(sc => sc.UserId == userId)
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

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        Task IRepository<ShoppingCart>.SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
