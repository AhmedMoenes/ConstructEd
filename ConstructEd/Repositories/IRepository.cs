

namespace ConstructEd.Repositories

{
    public interface IRepository<T>
    {
        ICollection<T> GetAll();
        
        T GetById(int id);
        
        void Insert(T obj);
        
        void Update(T obj);
        
        void Delete(int id);
        
        int Save();
    }
}