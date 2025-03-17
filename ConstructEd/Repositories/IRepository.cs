namespace ConstructEd.Repositories
{
    public interface IRepository<T>
    {
        Task<ICollection<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task InsertAsync(T obj);

        Task UpdateAsync(T obj);

        Task DeleteAsync(int id);

        Task SaveAsync();
    }
}