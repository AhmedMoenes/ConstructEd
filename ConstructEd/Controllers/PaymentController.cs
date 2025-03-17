using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.Services;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConstructEd.Controllers
{
    //[Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly FakePaymentService _paymentService;
        private readonly IShoppingCartRepository _shoppingCartRepository;


        public PaymentController(IPaymentRepository paymentRepository,
                                 IShoppingCartRepository shoppingCartRepository,
                                 IMapper mapper,
                                 FakePaymentService paymentService)
        {
            _paymentRepository = paymentRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Checkout()
        {
            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect if not logged in
            }

            // Retrieve shopping cart items for the user
            var cartItems = await _shoppingCartRepository.GetByUserIdAsync(userId);

            if (!cartItems.Any())
            {
                return RedirectToAction("Cart", "ShoppingCart"); // Redirect if cart is empty
            }

            // Calculate the total price (Sum of Course & Plugin prices)
            decimal totalAmount = cartItems.Sum(item =>
                (item.Course != null ? item.Course.Price : 0) +
                (item.Plugin != null ? item.Plugin.Price : 0));

            // Get course & plugin IDs for reference
            var courseIds = cartItems.Where(c => c.CourseId.HasValue).Select(c => c.CourseId.Value).ToList();
            var pluginIds = cartItems.Where(p => p.PluginId.HasValue).Select(p => p.PluginId.Value).ToList();

            var model = new PaymentViewModel
            {
                UserId = userId,
                CourseIds = courseIds,
                PluginIds = pluginIds,
                Amount = totalAmount // 💰 Set total amount dynamically
            };

            return View("PaymentForm", model); // Load the payment form with the correct total
        }

        public IActionResult Create()
        {
            return View(new PaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("PaymentForm", model); // Show validation errors
            }

            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve shopping cart items for validation
            var cartItems = await _shoppingCartRepository.GetByUserIdAsync(userId);
            if (!cartItems.Any())
            {
                return RedirectToAction("Cart", "ShoppingCart");
            }

            // Calculate total price again (to prevent manipulation)
            decimal expectedAmount = cartItems.Sum(item =>
                (item.Course != null ? item.Course.Price : 0) +
                (item.Plugin != null ? item.Plugin.Price : 0));

            if (model.Amount != expectedAmount)
            {
                ModelState.AddModelError("", "Invalid total amount.");
                return View("PaymentForm", model);
            }

            var payment = _mapper.Map<Payment>(model);
            payment.TransactionID = Guid.NewGuid();
            payment.Status = PaymentStatus.Pending;
            payment.PaymentDate = DateTime.UtcNow;

            bool isSuccessful = _paymentService.ProcessPayment(model);

            if (!isSuccessful)
            {
                payment.Status = PaymentStatus.Failed;
                await _paymentRepository.InsertAsync(payment);
                await _paymentRepository.SaveAsync();

                return RedirectToAction("Failure", new { transactionId = payment.TransactionID });
            }

            payment.Status = PaymentStatus.Success;
            await _paymentRepository.InsertAsync(payment);
            await _paymentRepository.SaveAsync();

            // 🎯 After successful payment, remove items from the shopping cart !!!!!!
            //await _shoppingCartRepository.ClearCartAsync(userId);

            return RedirectToAction("Success", new { transactionId = payment.TransactionID });
        }

        private bool IsExpiryDateValid(string expiryDate)
        {
            if (expiryDate.Length != 5 || !expiryDate.Contains('/'))
                return false;

            string[] parts = expiryDate.Split('/');
            if (!int.TryParse(parts[0], out int month) || !int.TryParse(parts[1], out int year))
                return false;

            int currentYear = DateTime.UtcNow.Year % 100;
            int currentMonth = DateTime.UtcNow.Month;

            return year > currentYear || (year == currentYear && month >= currentMonth);
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
