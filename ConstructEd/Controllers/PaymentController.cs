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

        public PaymentController(IPaymentRepository paymentRepository, 
                                        IMapper mapper, FakePaymentService paymentService)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public IActionResult Checkout(List<int> courseIds, decimal totalAmount)
        {
            var model = new PaymentViewModel
            {
                CourseIds = courseIds,
                Amount = totalAmount
            };

            return View("PaymentForm", model); // Open the Payment Form with prefilled data
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

            var payment = _mapper.Map<Payment>(model);
            payment.TransactionID = Guid.NewGuid(); // Generate transaction ID

            bool isSuccessful = _paymentService.ProcessPayment(model); // Simulated processing

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


    //// GET: Payment/Index
    //public async Task<IActionResult> Index()
    //{
    //    var payments = await _paymentRepository.GetAllAsync();
    //    var paymentViewModels = _mapper.Map<List<PaymentViewModel>>(payments);
    //    return View("Index",paymentViewModels);
    //}

    //// GET: Payment/Details/1
    //public async Task<IActionResult> Details(int id)
    //{
    //    var payment = await _paymentRepository.GetByIdAsync(id);
    //    if (payment == null)
    //    {
    //        return NotFound();
    //    }

    //    var paymentViewModel = _mapper.Map<PaymentViewModel>(payment);
    //    return View("Details",paymentViewModel);
    //}

    //// GET: Payment/Create
    ////[Authorize(Roles = "Admin")]
    //public IActionResult Create()
    //{
    //    return View();
    //}

    //// POST: Payment/Create
    //[HttpPost]
    ////[ValidateAntiForgeryToken]
    ////[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> Create(PaymentViewModel paymentViewModel)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var payment = new Payment
    //        {
    //            Amount = paymentViewModel.Amount,
    //            PaymentDate = DateTime.Parse(paymentViewModel.PaymentDate),
    //            TransactionId = paymentViewModel.TransactionId,
    //            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
    //            CourseId = int.Parse(paymentViewModel.CourseTitle) // Assuming CourseTitle is used to pass CourseId
    //        };

    //        await _paymentRepository.InsertAsync(payment);
    //        await _paymentRepository.SaveAsync();
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(paymentViewModel);
    //}

    //// GET: Payment/Edit/5
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> Edit(int id)
    //{
    //    var payment = await _paymentRepository.GetByIdAsync(id);
    //    if (payment == null)
    //    {
    //        return NotFound();
    //    }

    //    var paymentViewModel = _mapper.Map<PaymentViewModel>(payment);
    //    return View(paymentViewModel);
    //}

    //// POST: Payment/Edit/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> Edit(int id, PaymentViewModel paymentViewModel)
    //{
    //    if (id != int.Parse(paymentViewModel.TransactionId))
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            var payment = await _paymentRepository.GetByIdAsync(id);
    //            if (payment == null)
    //            {
    //                return NotFound();
    //            }

    //            payment.Amount = paymentViewModel.Amount;
    //            payment.TransactionId = paymentViewModel.TransactionId;
    //            payment.Status = Enum.Parse<PaymentStatus>(paymentViewModel.Status);

    //            await _paymentRepository.UpdateAsync(payment);
    //            await _paymentRepository.SaveAsync();
    //        }
    //        catch (Exception)
    //        {
    //            if (!await PaymentExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(paymentViewModel);
    //}

    //// GET: Payment/Delete/5
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    var payment = await _paymentRepository.GetByIdAsync(id);
    //    if (payment == null)
    //    {
    //        return NotFound();
    //    }

    //    var paymentViewModel = _mapper.Map<PaymentViewModel>(payment);
    //    return View(paymentViewModel);
    //}

    //// POST: Payment/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    await _paymentRepository.DeleteAsync(id);
    //    await _paymentRepository.SaveAsync();
    //    return RedirectToAction(nameof(Index));
    //}

    //// GET: Payment/UserPayments
    //public async Task<IActionResult> UserPayments()
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var payments = await _paymentRepository.GetAllAsync();
    //    var userPayments = payments.Where(p => p.UserId == userId).ToList();

    //    var paymentViewModels = _mapper.Map<List<PaymentViewModel>>(userPayments);
    //    return View(paymentViewModels);
    //}

    //// GET: Payment/CoursePayments/5
    //[Authorize(Roles = "Admin,Instructor")]
    //public async Task<IActionResult> CoursePayments(int courseId)
    //{
    //    var payments = await _paymentRepository.GetAllAsync();
    //    var coursePayments = payments.Where(p => p.CourseId == courseId).ToList();

    //    var paymentViewModels = _mapper.Map<List<PaymentViewModel>>(coursePayments);
    //    return View(paymentViewModels);
    //}

    //// POST: Payment/ProcessPayment
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> ProcessPayment(int courseId)
    //{
    //    try
    //    {
    //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //        // In a real application, you would integrate with a payment gateway here
    //        // For this example, we'll simulate a successful payment

    //        var payment = new Payment
    //        {
    //            Amount = 99.99m, // This would come from the course price
    //            PaymentDate = DateTime.UtcNow,
    //            TransactionId = Guid.NewGuid().ToString("N").Substring(0, 20),
    //            Status = PaymentStatus.Completed,
    //            UserId = userId,
    //            CourseId = courseId
    //        };

    //        await _paymentRepository.InsertAsync(payment);
    //        await _paymentRepository.SaveAsync();

    //        // Redirect to a thank you page or course access page
    //        return RedirectToAction("ThankYou", new { id = payment.Id });
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the exception
    //        ModelState.AddModelError("", "Payment processing failed: " + ex.Message);
    //        return RedirectToAction("Checkout", "Course", new { id = courseId });
    //    }
    //}

    //// GET: Payment/ThankYou/5
    //public async Task<IActionResult> ThankYou(int id)
    //{
    //    var payment = await _paymentRepository.GetByIdAsync(id);
    //    if (payment == null)
    //    {
    //        return NotFound();
    //    }

    //    var paymentViewModel = _mapper.Map<PaymentViewModel>(payment);
    //    return View(paymentViewModel);
    //}

    //// GET: Payment/Receipt/5
    //public async Task<IActionResult> Receipt(int id)
    //{
    //    var payment = await _paymentRepository.GetByIdAsync(id);
    //    if (payment == null)
    //    {
    //        return NotFound();
    //    }

    //    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    if (payment.UserId != currentUserId && !User.IsInRole("Admin"))
    //    {
    //        return Forbid();
    //    }

    //    var paymentViewModel = _mapper.Map<PaymentViewModel>(payment);
    //    return View(paymentViewModel);
    //}

    //// Helper method to check if a payment exists
    //private async Task<bool> PaymentExists(int id)
    //{
    //    var payment = await _paymentRepository.GetByIdAsync(id);
    //    return payment != null;
    //}

    //// GET: Payment/VerifyPayment?transactionId=XXX
    //[AllowAnonymous]
    //public async Task<IActionResult> VerifyPayment(string transactionId)
    //{
    //    if (string.IsNullOrEmpty(transactionId))
    //    {
    //        return BadRequest("Transaction ID is required");
    //    }

    //    var payments = await _paymentRepository.GetAllAsync();
    //    var payment = payments.FirstOrDefault(p => p.TransactionId == transactionId);

    //    if (payment == null)
    //    {
    //        return NotFound("Payment not found");
    //    }

    //    return Json(new
    //    {
    //        transactionId = payment.TransactionId,
    //        status = payment.Status.ToString(),
    //        date = payment.PaymentDate.ToString("yyyy-MM-dd HH:mm:ss"),
    //        verified = true
    //    });
    //}
}
