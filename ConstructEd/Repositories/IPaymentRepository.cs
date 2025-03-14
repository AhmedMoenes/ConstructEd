using ConstructEd.Data;
using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface IPaymentRepository : IRepository<Payment>
    {
        #region Asynchronous Methods

        Task ProcessRefundAsync(int paymentId, decimal refundAmount, string refundTransactionId, string modifiedBy);
        Task<int> GetTotalCountAsync();
        Task<ICollection<Payment>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<ICollection<Payment>> GetByStatusAsync(PaymentStatus status);
        Task<ICollection<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ICollection<Payment>> GetByCourseIdAsync(int courseId);
        Task<ICollection<Payment>> GetByUserIdAsync(string userId);
        Task<Payment> GetByTransactionIdAsync(string transactionId);

        #endregion


    }
}
