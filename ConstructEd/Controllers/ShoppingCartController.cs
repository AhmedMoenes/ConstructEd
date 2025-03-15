using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConstructEd.Controllers
{
    [Authorize] // 🔹 Ensures only logged-in users can access this controller
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        // 🔹 Display the shopping cart
        public async Task<IActionResult> Index()
        {
            // Get logged-in user ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            // Get shopping cart items for the user
            var cartItems = await _shoppingCartRepository.GetByUserIdAsync(userId);

            // Map to ShoppingCartViewModel
            var viewModel = new ShoppingCartViewModel
            {
                UserFullName = User.Identity.Name,
                UserEmail = User.FindFirstValue(ClaimTypes.Email),
                Courses = cartItems
                    .Where(sc => sc.Course != null)
                    .Select(sc => new CourseItemViewModel
                    {
                        Id = sc.Course.Id,
                        Title = sc.Course.Title,
                        Description = sc.Course.Description,
                        Price = sc.Course.Price,
                        Duration = sc.Course.Duration,
                        Category = sc.Course.Category
                    }).ToList(),
                Plugins = cartItems
                    .Where(sc => sc.Plugin != null)
                    .Select(sc => new PluginItemViewModel
                    {
                        Id = sc.Plugin.Id,
                        Title = sc.Plugin.Title,
                        Description = sc.Plugin.Description,
                        Price = sc.Plugin.Price
                    }).ToList()
            };

            return View(viewModel);
        }

        // 🔹 Remove an item from the cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _shoppingCartRepository.GetByIdAsync(id);
            if (cartItem == null) return NotFound();

            await _shoppingCartRepository.DeleteAsync(id);
            await _shoppingCartRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // 🔹 Proceed to checkout
        public IActionResult Checkout()
        {
            // This would typically redirect to a payment page or process the order
            return RedirectToAction("Payment", "Order");
        }
    }
}

