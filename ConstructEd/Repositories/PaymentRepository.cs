using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext dataContext;

        public PaymentRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region Asynchronous Methods
        public async Task DeleteAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment != null)
            {
                dataContext.Payments.Remove(payment);
            }
        }

        public async Task<ICollection<Payment>> GetAllAsync()
        {
            return await dataContext.Payments
                .Include(p => p.Course)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            return await dataContext.Payments
                .Include(p => p.Course)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            return await dataContext.Payments
                .Include(p => p.Course)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task InsertAsync(Payment payment)
        {
            await dataContext.Payments.AddAsync(payment);
        }

        public async Task<int> SaveAsync()
        {
            return await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            payment.ModifiedAt = DateTime.UtcNow;
            dataContext.Payments.Update(payment);
        }

        public async Task<ICollection<Payment>> GetByUserIdAsync(string userId)
        {
            return await dataContext.Payments
                .Include(p => p.Course)
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<ICollection<Payment>> GetByCourseIdAsync(int courseId)
        {
            return await dataContext.Payments
                .Include(p => p.Course)
                .Include(p => p.User)
                .Where(p => p.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<ICollection<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await dataContext.Payments
                .Include(p => p.Course)
                .Include(p => p.User)
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .ToListAsync();
        }

        public async Task<ICollection<Payment>> GetByStatusAsync(PaymentStatus status)
        {
            return await dataContext.Payments
                .Include(p => p.Course)
                .Include(p => p.User)
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<ICollection<Payment>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            return await dataContext.Payments
                .Include(p => p.Course)
                .Include(p => p.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await dataContext.Payments.CountAsync();
        }

        public async Task ProcessRefundAsync(int paymentId, decimal refundAmount, string refundTransactionId, string modifiedBy)
        {
            var payment = await GetByIdAsync(paymentId);

            if (payment != null)
            {
                payment.RefundedAmount = refundAmount;
                payment.RefundDate = DateTime.UtcNow;
                payment.RefundTransactionId = refundTransactionId;
                payment.Status = refundAmount >= payment.Amount ?
                    PaymentStatus.Refunded : PaymentStatus.PartiallyRefunded;
                payment.ModifiedAt = DateTime.UtcNow;
                payment.ModifiedBy = modifiedBy;

                dataContext.Payments.Update(payment);
                await SaveAsync();
            }
        }
        #endregion
    }
}