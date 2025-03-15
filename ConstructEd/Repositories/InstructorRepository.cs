using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly DataContext _dataContext;

        public InstructorRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<Instructor>> GetAllAsync()
        {
            return await _dataContext.Instructors.ToListAsync();
        }

        public async Task<Instructor> GetByIdAsync(int id)
        {
            return await _dataContext.Instructors.FindAsync(id);
        }

        public async Task InsertAsync(Instructor obj)
        {
            _dataContext.Instructors.Add(obj);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Instructor obj)
        {
            _dataContext.Instructors.Update(obj);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var instructor = await _dataContext.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _dataContext.Instructors.Remove(instructor);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}