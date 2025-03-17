using ConstructEd.Data;
using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {

         Task<IEnumerable<ShoppingCart>> GetByUserIdAsync(string userId);
    }
}
