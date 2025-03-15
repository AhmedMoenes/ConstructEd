using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _dataContext;

        public PaymentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<Payment>> GetAllAsync()
        {
            return await _dataContext.Payments.Include(p => p.User).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId)
        {
            return await _dataContext.Payments.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _dataContext.Payments.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id); // Fix: Use PaymentId
        }

        public async Task InsertAsync(Payment payment)
        {
            await _dataContext.Payments.AddAsync(payment);
            await SaveAsync(); // Ensure changes are saved
        }

        public async Task UpdateAsync(Payment payment)
        {
            _dataContext.Payments.Update(payment);
            await SaveAsync(); // Ensure changes are saved
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _dataContext.Payments.FindAsync(id);
            if (payment != null)
            {
                _dataContext.Payments.Remove(payment);
                await SaveAsync(); // Ensure changes are saved
            }
        }

        public async Task SaveAsync()
        {
             _dataContext.SaveChangesAsync();
        }

    }
}
