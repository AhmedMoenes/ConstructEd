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

        public async Task DeleteAsync(int id)
        {
            var emp = await GetByIdAsync(id);
            if (emp != null)
            {
                dataContext.Remove(emp);
            }
        }

        public async Task<ICollection<Enrollment>> GetAllAsync()
        {
            return await dataContext.Enrollments
                .Include(n => n.Course)
                .Include(b => b.User)
                .ToListAsync();
        }

        public Task<List<Enrollment>> GetByCourseIdAsync(string courseId)
        {
            throw new NotImplementedException();
        }

        public async Task<Enrollment> GetByIdAsync(int id)
        {
            return await dataContext.Enrollments
                .Include(n => n.Course)
                .Include(b => b.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Enrollment?> GetByUserAndCourseAsync(string userId, int courseId)
        {
            return await dataContext.Enrollments
                         .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        public async Task<List<Enrollment>> GetByUserIdAsync(string userId)
        {
            return await dataContext.Enrollments
                        .Include(e => e.Course)
                        .Where(e => e.UserId == userId)
                        .ToListAsync();
        }

        public async Task InsertAsync(Enrollment obj)
        {
            await dataContext.AddAsync(obj);
        }

        public async Task<int> SaveAsync()
        {
            return await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment obj)
        {
            dataContext.Update(obj); 
            await Task.CompletedTask;
        }
    }
}