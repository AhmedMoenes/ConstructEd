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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _shoppingCartRepository.GetByUserIdAsync(userId);
            if (!cartItems.Any())
            {
                return RedirectToAction("Cart", "ShoppingCart");
            }

            decimal totalAmount = cartItems.Sum(item =>
                (item.Course?.Price ?? 0) + (item.Plugin?.Price ?? 0));

            var model = new PaymentViewModel
            {
                UserId = userId,
                CourseIds = cartItems.Where(c => c.CourseId.HasValue).Select(c => c.CourseId.Value).ToList(),
                PluginIds = cartItems.Where(p => p.PluginId.HasValue).Select(p => p.PluginId.Value).ToList(),
                Amount = totalAmount
            };

            return await ProcessPayment(model); // 🚀 Call ProcessPayment directly!
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
                return View("Create", model); // Show validation errors
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
                return View("Create", model);
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
