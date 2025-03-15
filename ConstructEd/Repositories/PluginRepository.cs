using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public class PluginRepository: IPluginRepository
    {
        private readonly DataContext _dataContext;

        public PluginRepository(DataContext dataContext)
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
        public async Task<ICollection<Plugin>> GetAllAsync()
        {
            return await _dataContext.Plugins.ToListAsync();
        }

        public async Task<Plugin> GetByIdAsync(int id)
        {
            return await _dataContext.Plugins.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task InsertAsync(Course obj)
        {
            await _dataContext.AddAsync(obj);
        }
        public Task InsertAsync(Plugin obj)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
             await _dataContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Plugin obj)
        {
            _dataContext.Update(obj);
            await Task.CompletedTask;
        }

        Task<int> IRepository<Plugin>.SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
