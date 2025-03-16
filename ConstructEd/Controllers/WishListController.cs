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
			{
				return NoContent(); // No action if user is not logged in
			}

			var existingItem = await _wishlistRepository.GetByUserIdAsync(userId);
			bool isAlreadyInCart = existingItem.Any(sc =>
				(type == "Course" && sc.CourseId == id) ||
				(type == "Plugin" && sc.PluginId == id));

			if (!isAlreadyInCart)
			{
				var wish = new Wishlist
				{
					UserId = userId,
					CourseId = type == "Course" ? id : (int?)null,
					PluginId = type == "Plugin" ? id : (int?)null
				};

				await _wishlistRepository.InsertAsync(wish);
				await _wishlistRepository.SaveAsync();
			}

			return NoContent(); // Return nothing, just process the request
		}


		// 🔹 Display the shopping cart
		public async Task<IActionResult> Index()
		{
			// Get logged-in user ID
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
				await _wishlistRepository.RemoveCourseFromCartAsync(userId, id);
			}
			else if (type == "plugin")
			{
				await _wishlistRepository.RemovePluginFromCartAsync(userId, id);
			}
			else
			{
				return BadRequest("Invalid item type.");
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
