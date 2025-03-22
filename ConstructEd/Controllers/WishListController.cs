using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.Services;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConstructEd.Controllers
{
	public class WishListController : Controller
	{
		private readonly IWishListRepository _wishlistRepository;
		public WishListController(IWishListRepository whisListRepository)
		{
			_wishlistRepository = whisListRepository;
			
		}

        [HttpPost]
        public async Task<IActionResult> AddToWish(int id, string type)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) 
                return Unauthorized(new { message = "User not logged in" });

            var existingItem = await _wishlistRepository.GetByUserIdAsync(userId);
            bool isAlreadyInWishlist = existingItem.Any(sc =>
                (type == "Course" && sc.CourseId == id) ||
                (type == "Plugin" && sc.PluginId == id));

            if (!isAlreadyInWishlist)
            {
                var wish = new Wishlist
                {
                    UserId = userId,
                    CourseId = type == "Course" ? id : (int?)null,
                    PluginId = type == "Plugin" ? id : (int?)null
                };

                await _wishlistRepository.InsertAsync(wish);
                await _wishlistRepository.SaveAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Item is already in wishlist" });
        }

        // 🔹 Display the shopping cart
        public async Task<IActionResult> Index()
		{
			// Get logged-in user ID
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            // Get shopping cart items for the user
            var wish = await _wishlistRepository.GetByUserIdAsync(userId);

			// Map to ShoppingCartViewModel
			var viewModel = new WishListViewModel
			{
				UserFullName = User.Identity.Name,
				UserEmail = User.FindFirstValue(ClaimTypes.Email),
				Courses = wish
					.Where(sc => sc.Course != null)
					.Select(sc => new CourseViewModel
					{
						Id = sc.Course.Id,
						Title = sc.Course.Title,
						Description = sc.Course.Description,
						
						Duration = sc.Course.Duration,
						Category = sc.Course.Category
					}).ToList(),
				Plugins = wish
					.Where(sc => sc.Plugin != null)
					.Select(sc => new PluginViewModel
					{
						Id = sc.Plugin.Id,
						Title = sc.Plugin.Title,
						Description = sc.Plugin.Description,
						
					}).ToList()
			};

			return View(viewModel);
		}

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id, string type)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            bool removed = false;

            if (type == "Course")
            {
                removed = await _wishlistRepository.RemoveCourseFromCartAsync(userId, id);
            }
            else if (type == "Plugin")
            {
                removed = await _wishlistRepository.RemovePluginFromCartAsync(userId, id);
            }
            else
            {
                return Json(new { success = false, message = "Invalid item type" });
            }

            if (removed)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Item not found in wishlist" });
        }


        [HttpGet]
        public async Task<IActionResult> GetWishlistCount()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(0);
            }

            int count = await _wishlistRepository.GetCountByUserIdAsync(userId);
            return Json(count);
        }

    }
}
