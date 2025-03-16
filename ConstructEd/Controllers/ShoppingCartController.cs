using AutoMapper;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConstructEd.Controllers
{
   // [Authorize] // 🔹 Ensures only logged-in users can access this controller
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IMapper mapper;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository ,IMapper mapper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            this.mapper = mapper;
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
                    .Select(sc => new CourseViewModel
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
                    .Select(sc => new PluginViewModel
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
        public async Task<IActionResult> RemoveFromCart(int id, string type)
        {
            // Get the logged-in user ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (type == "course")
            {
                await _shoppingCartRepository.RemoveCourseFromCartAsync(userId, id);
            }
            else if (type == "plugin")
            {
                await _shoppingCartRepository.RemovePluginFromCartAsync(userId, id);
            }
            else
            {
                return BadRequest("Invalid item type.");
            }

            return RedirectToAction(nameof(Index));
        }


        // 🔹 Proceed to checkout
        //public IActionResult Checkout()
        //{

        //    return RedirectToAction("Payment", "Order");
        //}
    }
}

