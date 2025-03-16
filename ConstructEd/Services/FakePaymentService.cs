using ConstructEd.ViewModels;

namespace ConstructEd.Services
{
    public class FakePaymentService
    {
        private static readonly Random _random = new();

        public bool ProcessPayment(PaymentViewModel payment)
        {
            // Simulate failure in 5% of cases haahahahahaha
            return _random.Next(1, 101) > 5;
        }
    }
}
