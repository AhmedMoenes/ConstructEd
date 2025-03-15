using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DataContext _dataContext;

        public EnrollmentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dataContext.Remove(entity);
            }
        }

        public async Task<ICollection<Enrollment>> GetAllAsync()
        {
            return await _dataContext.Enrollments
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
            return await _dataContext.Enrollments
                .Include(n => n.Course)
                .Include(b => b.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Enrollment?> GetByUserAndCourseAsync(string userId, int courseId)
        {
            return await _dataContext.Enrollments
                         .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        public async Task<List<Enrollment>> GetByUserIdAsync(string userId)
        {
            return await _dataContext.Enrollments
                        .Include(e => e.Course)
                        .Where(e => e.UserId == userId)
                        .ToListAsync();
        }

        public async Task InsertAsync(Enrollment obj)
        {
            await _dataContext.AddAsync(obj);
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment obj)
        {
            _dataContext.Update(obj); 
            await Task.CompletedTask;
        }
    }
}