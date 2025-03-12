using ConstructEd.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class PaymentController1 : Controller
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController1(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;

        }
    }
}
