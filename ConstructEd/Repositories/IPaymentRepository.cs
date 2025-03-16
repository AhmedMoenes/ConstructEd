using ConstructEd.Data;
using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface IPaymentRepository : IRepository<Payment>
    {

        #region old code
        //Task<int> GetTotalCountAsync();
        //Task<ICollection<Payment>> GetPaginatedAsync(int pageNumber, int pageSize);
        //Task<ICollection<Payment>> GetByCourseIdAsync(int courseId);
        //Task<ICollection<Payment>> GetByUserIdAsync(string userId);
        //Task<Payment> GetByTransactionIdAsync(string transactionId);
        #endregion
    }
}
