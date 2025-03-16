using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.Services;
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
		private readonly IPaymentRepository _paymentRepository;
		private readonly IEnrollmentRepository _enrollmentRepository;
		private readonly FakePaymentService _paymentService;

		public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
									   IPaymentRepository paymentRepository,
                                       IEnrollmentRepository enrollmentRepository,
									   FakePaymentService paymentService,
									   IMapper mapper)
		{
			_shoppingCartRepository = shoppingCartRepository;
			_paymentRepository = paymentRepository;
			_paymentService = paymentService;
            _enrollmentRepository = enrollmentRepository;
            this.mapper = mapper;
		}


		[HttpPost]
        //public async Task<IActionResult> AddToCart(int id, string type)
        //{
        //    // Get the logged-in user ID
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    // Check if the item is already in the shopping cart
        //    var existingItem = await _shoppingCartRepository.GetByUserIdAsync(userId);
        //    bool isAlreadyInCart = existingItem.Any(sc =>
        //        (type == "Course" && sc.CourseId == id) ||
        //        (type == "Plugin" && sc.PluginId == id));

        //    if (isAlreadyInCart)
        //    {
        //        return RedirectToAction(nameof(Index)); // Item already exists, no need to add again
        //    }

        //    var cartItem = new ShoppingCart
        //    {
        //        UserId = userId,
        //        CourseId = type == "Course" ? id : (int?)null,
        //        PluginId = type == "Plugin" ? id : (int?)null
        //    };

        //    await _shoppingCartRepository.InsertAsync(cartItem);
        //    await _shoppingCartRepository.SaveAsync();

        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, string type)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return NoContent(); // No action if user is not logged in
            }

            var existingItem = await _shoppingCartRepository.GetByUserIdAsync(userId);
            bool isAlreadyInCart = existingItem.Any(sc =>
                (type == "Course" && sc.CourseId == id) ||
                (type == "Plugin" && sc.PluginId == id));

            if (!isAlreadyInCart)
            {
                var cartItem = new ShoppingCart
                {
                    UserId = userId,
                    CourseId = type == "Course" ? id : (int?)null,
                    PluginId = type == "Plugin" ? id : (int?)null
                };

                await _shoppingCartRepository.InsertAsync(cartItem);
                await _shoppingCartRepository.SaveAsync();
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


        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _shoppingCartRepository.GetByUserIdAsync(userId);
            if (!cartItems.Any())
            {
                return RedirectToAction("Cart", "ShoppingCart");
            }

            var payment = mapper.Map<Payment>(model);
            payment.TransactionID = Guid.NewGuid();
            payment.UserId = userId;

            bool isSuccessful = _paymentService.ProcessPayment(model);

            if (!isSuccessful)
            {
                payment.Status = PaymentStatus.Failed;
                await _paymentRepository.InsertAsync(payment);
                await _paymentRepository.SaveAsync();

                return RedirectToAction("Failure", new { transactionId = payment.TransactionID });
            }

            // 🔹 Create Enrollment Records
            var enrollments = new List<Enrollment>();
            foreach (var item in cartItems)
            {
                var enrollment = new Enrollment
                {
                    UserId = userId,
                    EnrollmentDate = DateTime.UtcNow,
                    Progress = 0.00
                };

                if (item.CourseId.HasValue)
                {
                    enrollment.CourseId = item.CourseId;
                }
                else if (item.PluginId.HasValue)
                {
                    enrollment.PluginId = item.PluginId;
                }

                if (enrollment.IsValid)  // Ensure only one of Course or Plugin is assigned
                {
                    enrollments.Add(enrollment);
                }
            }

            await _enrollmentRepository.InsertRangeAsync(enrollments);
            await _enrollmentRepository.SaveAsync();

            // 🔹 Clear cart after successful payment
            await _shoppingCartRepository.ClearCartAsync(userId);

            payment.Status = PaymentStatus.Success;

            await _paymentRepository.InsertAsync(payment);
            await _paymentRepository.SaveAsync();

            return RedirectToAction("Success", new { transactionId = payment.TransactionID });
        }

        public IActionResult Success(Guid transactionId)
		{
			ViewBag.TransactionID = transactionId;
			return View();
		}

		public IActionResult Failure()
		{
			return View();
		}
	}
}

