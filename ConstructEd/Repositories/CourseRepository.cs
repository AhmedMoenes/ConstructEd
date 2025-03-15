﻿using ConstructEd.Data;
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
                _dataContext.Remove(entity);
            }
        }

        public async Task<ICollection<Course>> GetAllAsync()
        {
            return await _dataContext.Courses.ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _dataContext.Courses.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task InsertAsync(Course obj)
        {
            await _dataContext.AddAsync(obj);
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course obj)
        {
            _dataContext.Update(obj); 
            await Task.CompletedTask; 
        }
    }
}