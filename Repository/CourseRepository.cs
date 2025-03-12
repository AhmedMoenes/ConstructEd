using ConstructEd.Data;
using ConstructEd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal class CourseRepository : ICourseRepository
    {
        private readonly DataContext dataContext;

        public CourseRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public void Delete(int id)
        {
            Course emp = GetById(id);
            dataContext.Remove(emp);
        }

        public ICollection<Course> GetAll()
        {
            return dataContext.Courses.ToList();
        }

        public Course GetById(int id)
        {

            return dataContext.Courses.FirstOrDefault(e => e.Id == id);
        }

        public void Insert(Course obj)
        {
            dataContext.Add(obj);
        }

        public int Save()
        {
            return dataContext.SaveChanges();
        }

        public void Update(Course obj)
        {
            dataContext.Update(obj);
        }
    }
}
