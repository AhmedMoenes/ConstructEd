using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories

{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DataContext dataContext;

        public EnrollmentRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public void Delete(int id)
        {
            Enrollment emp = GetById(id);
            dataContext.Remove(emp);
        }

        public ICollection<Enrollment> GetAll()
        {
            return dataContext.Enrollments.Include(n=>n.Course).Include(b=>b.User).ToList();
        }

        public Enrollment GetById(int id)
        {

            return dataContext.Enrollments.Include(n => n.Course).Include(b => b.User).FirstOrDefault(e => e.Id == id);
        }

        public void Insert(Enrollment obj)
        {
            dataContext.Add(obj);
        }

        public int Save()
        {
            return dataContext.SaveChanges();
        }

        public void Update(Enrollment obj)
        {
            dataContext.Update(obj);
        }
    }
}
