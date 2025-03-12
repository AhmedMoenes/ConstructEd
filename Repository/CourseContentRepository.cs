using ConstructEd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructEd.Data;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal class CourseContentRepository : ICourseContentRepository
    {
        private readonly DataContext dataContext;

        public CourseContentRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public void Delete(int id)
        {
            CourseContent emp = GetById(id);
            dataContext.Remove(emp);
        }

        public ICollection<CourseContent> GetAll()
        {
            return dataContext.CourseContents.Include(m => m.Course).ToList();
        }

        public CourseContent GetById(int id)
        {
            
            return dataContext.CourseContents.Include(m=>m.Course).FirstOrDefault(e => e.Id == id);
        }

        public void Insert(CourseContent obj)
        {
            dataContext.Add(obj);
        }

        public int Save()
        {
           return dataContext.SaveChanges();
        }

        public void Update(CourseContent obj)
        {
            dataContext.Update(obj);
        }
    }
}
