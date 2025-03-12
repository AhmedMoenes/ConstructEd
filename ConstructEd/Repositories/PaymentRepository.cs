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
        public void Delete(int id)
        {
            Payment emp = GetById(id);
            dataContext.Remove(emp);
        }

        public ICollection<Payment> GetAll()
        {
            return dataContext.Payments.Include(n => n.Course).Include(b => b.User).ToList();
        }

        public Payment GetById(int id)
        {

            return dataContext.Payments.Include(n => n.Course).Include(b => b.User).FirstOrDefault(e => e.Id == id);
        }

        public void Insert(Payment obj)
        {
            dataContext.Add(obj);
        }

        public int Save()
        {
            return dataContext.SaveChanges();
        }

        public void Update(Payment obj)
        {
            dataContext.Update(obj);
        }
    }
}
