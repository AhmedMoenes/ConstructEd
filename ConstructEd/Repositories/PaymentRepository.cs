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

        public async Task<int> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }
    }

    #region old code

    //public async Task<Payment> GetByTransactionIdAsync(string transactionId)
    //{
    //    return await _dataContext.Payments
    //        .Include(p => p.Course)
    //        .Include(p => p.User)
    //        .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
    //}
    //public async Task<ICollection<Payment>> GetByUserIdAsync(string userId)
    //{
    //    return await _dataContext.Payments
    //        .Include(p => p.Course)
    //        .Include(p => p.User)
    //        .Where(p => p.UserId == userId)
    //        .ToListAsync();
    //}
    //public async Task<ICollection<Payment>> GetByCourseIdAsync(int courseId)
    //{
    //    return await _dataContext.Payments
    //        .Include(p => p.Course)
    //        .Include(p => p.User)
    //        .Where(p => p.CourseId == courseId)
    //        .ToListAsync();
    //}
    //public async Task<ICollection<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    //{
    //    return await _dataContext.Payments
    //        .Include(p => p.Course)
    //        .Include(p => p.User)
    //        .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
    //        .ToListAsync();
    //}
    //public async Task<ICollection<Payment>> GetPaginatedAsync(int pageNumber, int pageSize)
    //{
    //    return await _dataContext.Payments
    //        .Include(p => p.Course)
    //        .Include(p => p.User)
    //        .Skip((pageNumber - 1) * pageSize)
    //        .Take(pageSize)
    //        .ToListAsync();
    //}
    //public async Task<int> GetTotalCountAsync()
    //{
    //    return await _dataContext.Payments.CountAsync();
    //}

    //public Task<IEnumerable<Payment>> GetAllPaymentsAsync()
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<Payment> GetPaymentByIdAsync(int id)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task AddPaymentAsync(Payment payment)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task UpdatePaymentAsync(Payment payment)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task DeletePaymentAsync(int id)
    //{
    //    throw new NotImplementedException();
    //}
    #endregion
}
