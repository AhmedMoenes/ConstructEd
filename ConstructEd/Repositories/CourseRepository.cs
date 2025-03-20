﻿    using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _dataContext;

        public CourseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dataContext.Courses.Remove(entity);
                await SaveAsync(); 
            }
        }

        public async Task<ICollection<Course>> GetAllAsync()
        {
            return await _dataContext.Courses.ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _dataContext.Courses
                             .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task InsertAsync(Course obj)
        {
            await _dataContext.Courses.AddAsync(obj);
            await SaveAsync(); 
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course obj)
        {
            _dataContext.Courses.Update(obj);
            await SaveAsync(); 
        }

        public ICollection<string> GetCategories()
        {
            var categories = Enum.GetNames(typeof(Category));
            return categories;
        }

    }
}